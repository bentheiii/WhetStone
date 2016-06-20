using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using WhetStone.Arrays;
using WhetStone.Looping;
using WhetStone.Path;
using WhetStone.Serializations;
using WhetStone.Units.Time;
using WhetStone.WordsPlay;

namespace WhetStone.PermanentObject
{
    public interface ISafeDeletable : IDisposable
    {
        bool DeleteOnDispose { get; set; }
        FileAccess access { get; }
        FileShare share { get; }
        bool AllowCaching { get; }
    }
    public interface IPermaObject<T> : ISafeDeletable
    {
        T tryParse(out Exception ex);
        T value { get; set; }
        string name { get; }
    }
    public static class PermaObject
    {
        public static bool Exists(string name)
        {
            return File.Exists(name);
        }
        public static void MutauteValue<T>(this IPermaObject<T> @this, Func<T, T> mutation)
        {
            @this.value = mutation(@this.value);
        }
        public static void MutauteValue<T>(this IPermaObject<T> @this, Action<T> mutation)
        {
            var v = @this.value;
            mutation(v);
            @this.value = v;
        }
        public static string LocalName<T>(this IPermaObject<T> @this)
        {
            var s = @this.name;
            return System.IO.Path.GetFileName(s);
        }
        public static bool Readable<T>(this IPermaObject<T> @this)
        {
            Exception ex;
            var temp = @this.tryParse(out ex);
            return ex == null;
        }
        public static TimeSpan timeSinceUpdate<T>(this ISyncPermaObject<T> @this)
        {
            return DateTime.Now.Subtract(@this.getLatestUpdateTime());
        }
    }
    public class PermaObject<T> : IPermaObject<T>
    {
        private readonly FileStream _stream;
        public FileAccess access { get; }
        public FileShare share { get; }
        public bool AllowCaching { get; }
        public bool DeleteOnDispose { get; set; }
        private readonly Func<byte[], T> _read;
        private readonly Func<T, byte[]> _write;
         private Tuple<T,bool> _cache;
        public PermaObject(string name, bool deleteOnDispose = false, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.None, FileMode mode = FileMode.OpenOrCreate, T valueIfCreated = default(T), bool allowCaching = true) : this(a => (T)Serialization.Deserialize(a), a=>Serialization.Serialize(a), name, deleteOnDispose, access, share, mode, valueIfCreated,allowCaching) { }
        public PermaObject(Func<byte[], T> read, Func<T, byte[]> write, string name, bool deleteOnDispose = false, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.None, FileMode mode = FileMode.OpenOrCreate, T valueIfCreated = default(T),bool allowCaching = true)
        {
            name = System.IO.Path.GetFullPath(name);
            if (mode == FileMode.Truncate || mode == FileMode.Append)
                throw new ArgumentException("truncate and append modes are not supported", nameof(mode));
            bool create = !PermaObject.Exists(name);
            if (mode != FileMode.Open && valueIfCreated == null)
                throw new ArgumentException("is the default value is null, the PermaObject cannot be newly created");
            if (deleteOnDispose && (!create && share != FileShare.None))
                throw new ArgumentException("delete on dispose demands the file not previously exist or that sharing will be none", nameof(deleteOnDispose));
            this._read = read;
            this._write = write;
            this.access = access;
            this.share = share;
            this.AllowCaching = allowCaching;
            FileOptions options = FileOptions.SequentialScan;
            if (share != FileShare.None)
                options |= FileOptions.Asynchronous;
            DeleteOnDispose = deleteOnDispose;
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(name));
            _stream = new FileStream(name, mode, access, share,4096,options);
            if (create)
                this.value = valueIfCreated;
            _cache = (this.share == FileShare.None && allowCaching) ? Tuple.Create(default(T), false) : null;
        }
        public T tryParse(out Exception ex)
        {
            ex = null;
            if (!access.HasFlag(FileAccess.Read))
                throw new AccessViolationException("permaobject is set not to read");
            if (_cache?.Item2 ?? false)
                return _cache.Item1;
            try
            {
                var b = LoadFiles.loadAsBytes(_stream);
                var ret = _read(b);
                if (_cache != null)
                    _cache = Tuple.Create(ret, true);
                return ret;
            }
            catch (Exception e)
            {
                ex = e;
                return default(T);
            }
        }
        public T value
        {
            get
            {
                Exception prox;
                T ret = tryParse(out prox);
                if (prox != null)
                    throw prox;
                return ret;
            }
            set
            {
                if (!access.HasFlag(FileAccess.Write))
                    throw new AccessViolationException("permaobject is set not to write");
                byte[] buffer = _write(value);
                _stream.Seek(0, SeekOrigin.Begin);
                _stream.SetLength(0);
                _stream.Write(buffer,0,buffer.Length);
                _stream.Flush(true);
                if (_cache != null)
                    _cache = Tuple.Create(value, true);
            }
        }
        public string name
        {
            get
            {
                return _stream.Name;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                (this._stream as IDisposable).Dispose();
                if (DeleteOnDispose)
                    File.Delete(this.name);
            }
        }
    }
    public interface ISyncPermaObject<T> : IPermaObject<T>
    {
        T getFresh(DateTime earliestTime);
        T getFresh(TimeSpan maxInterval);
        DateTime getLatestUpdateTime();
    }
    public class SyncPermaObject<T> : ISyncPermaObject<T>
    {
        internal const string PERMA_OBJ_UPDATE_EXTENSION = ".permaobjupdate";
        private readonly PermaObject<T> _int;
        private readonly PermaObject<DateTime> _update;
        public SyncPermaObject( string name, bool deleteOnDispose = false, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.None, FileMode mode = FileMode.OpenOrCreate, T valueIfCreated = default(T), bool allowCaching = true) : this(a => (T)Serialization.Deserialize(a), a=>Serialization.Serialize(a), name, deleteOnDispose, access, share, mode, valueIfCreated,allowCaching) { }
        public SyncPermaObject(Func<byte[], T> read, Func<T, byte[]> write, string name, bool deleteOnDispose = false, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.None, FileMode mode = FileMode.OpenOrCreate, T valueIfCreated = default(T), bool allowCaching = true)
        {
            _int = new PermaObject<T>(read,write,name,deleteOnDispose, access, share, mode, valueIfCreated,allowCaching);
            _update = new PermaObject<DateTime>(FilePath.MutateFileName(name, a=> "__LATESTUPDATE_"+a), deleteOnDispose, access, share, mode, DateTime.Now,allowCaching);
        }
        public T getFresh(TimeSpan maxInterval)
        {
            return getFresh(maxInterval, maxInterval.Divide(2));
        }
        public T getFresh(TimeSpan maxInterval, TimeSpan checkinterval)
        {
            while (this.timeSinceUpdate() > maxInterval)
            {
                Thread.Sleep(checkinterval);
            }
            return this.value;
        }
        public T getFresh(DateTime earliestTime)
        {
            return getFresh(earliestTime, TimeSpan.FromSeconds(0.5));
        }
        public T getFresh(DateTime earliestTime, TimeSpan checkinterval)
        {
            while (getLatestUpdateTime() < earliestTime)
            {
                Thread.Sleep(checkinterval);
            }
            return this.value;
        }
        public DateTime getLatestUpdateTime()
        {
            Exception e;
            var a = _update.tryParse(out e);
            return (e == null) ? a : DateTime.MinValue;
        }
        public T tryParse(out Exception ex)
        {
            return _int.tryParse(out ex);
        }
        public T value
        {
            get
            {
                return _int.value;
            }
            set
            {
                _update.value = DateTime.Now;
                _int.value = value;
            }
        }
        public string name
        {
            get
            {
                return _int.name;
            }
        }
        public FileAccess access
        {
            get
            {
                return _int.access;
            }
        }
        public FileShare share
        {
            get
            {
                return _int.share;
            }
        }
        public bool AllowCaching
        {
            get
            {
                return _int.AllowCaching;
            }
        }
        public bool DeleteOnDispose
        {
            get
            {
                return _int.DeleteOnDispose;
            }
            set
            {
                _int.DeleteOnDispose = value;
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _int.Dispose();
                _update.Dispose();
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
    namespace Enumerable
    {
        public class PermaList<T> : IList<T>, ISafeDeletable
        {
            [Serializable]
            private class PermaObjArrayData
            {
                public int length { get; }
                public int offset { get; }
                public PermaObjArrayData(int length, int offset )
                {
                    this.length = length;
                    this.offset = offset;
                }
            }
            private IList<IPermaObject<T>> _array;
            private readonly IPermaObject<PermaObjArrayData> _data;
            private readonly Func<byte[], T> _read;
            private readonly Func<T, byte[]>_write;
            public bool DeleteOnDispose { get; set; }
            public FileAccess access
            {
                get
                {
                    return _data.access;
                }
            }
            public FileShare share
            {
                get
                {
                    return _data.share;
                }
            }
            public bool AllowCaching
            {
                get
                {
                    return _data.AllowCaching;
                }
            }
            private readonly T _valueIfCreated;
            public bool SupportMultiAccess => _data.share != FileShare.None;
            public PermaList(string name, int length=0, int offset = 2, bool deleteOnDispose = false, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.None, FileMode mode = FileMode.OpenOrCreate, T valueIfCreated = default(T), bool allowCaching = true) : this(length, offset,a => (T)Serialization.Deserialize(a), a => Serialization.Serialize(a), name, deleteOnDispose, access, share, mode, valueIfCreated,allowCaching) { }
            //if array already exists, the length and offset parameters are ignored
            public PermaList(int length, int offset ,Func<byte[], T> read, Func<T, byte[]> write, string name, bool deleteOnDispose = false, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.None, FileMode mode = FileMode.OpenOrCreate, T valueIfCreated = default(T), bool allowCaching = true)
            {
                _read = read;
                _write = write;
                this.DeleteOnDispose = deleteOnDispose;
                _valueIfCreated = valueIfCreated;
                this.name = name;
                _data = new PermaObject<PermaObjArrayData>(FilePath.MutateFileName(name, a => "__ARRAYDATA_" + a), deleteOnDispose, access, share, mode, new PermaObjArrayData(length,offset),allowCaching);
                this.updateArr(true);
            }
            private string getname(int ind)
            {
                return FilePath.MutateFileName(name, k => "__ARRAYMEMBER_" + ind + "_" + k);
            }
            private IPermaObject<T> getperma(int index)
            {
                return new PermaObject<T>(_read, _write, getname(index), DeleteOnDispose, _data.access, _data.share, valueIfCreated: _valueIfCreated,
                    allowCaching: _data.AllowCaching);
            }
            private void updateArr(bool overridemulti = false)
            {
                if (SupportMultiAccess || overridemulti)
                {
                    _array?.Do(a=> { a.DeleteOnDispose = false; a.Dispose();});
                    this._array = Loops.Range(this._data.value.offset, _data.value.offset + _data.value.length).Select(getperma).ToList();
                }
            }
            public int IndexOf(T item)
            {
                return (_array.CountBind().FirstOrDefault(a => a.Item1.value.Equals(item)) ?? Tuple.Create((IPermaObject<T>)null, -1)).Item2;
            }
            public void Insert(int index, T item)
            {
                updateArr();
                if (index < 0 || index > length)
                    throw new ArgumentOutOfRangeException(nameof(index),"out of range");
                if (index == 0 && length > 0)
                {
                    if (_data.value.offset <= 0)
                        offsetfiles(this.length/4);
                    _data.MutauteValue(a => new PermaObjArrayData(a.length + 1, a.offset - 1));
                    IPermaObject<T> newval = getperma(_data.value.offset);
                    newval.value = item;
                    _array.Insert(0, newval);
                }
                else
                {
                    _data.MutauteValue(a => new PermaObjArrayData(a.length + 1, a.offset));
                    updateArr(true);
                    foreach (var i in Loops.Range(index, this.length - 1).Reverse())
                    {
                        this[i + 1] = this[i];
                    }
                    this[index] = item;
                }
            }
            public void RemoveAt(int index)
            {
                if (index < 0 || index >= length)
                    throw new ArgumentOutOfRangeException(nameof(index), "out of range");
                updateArr();
                if (index == 0)
                {
                    var todel = this._array[0];
                    todel.DeleteOnDispose = true;
                    todel.Dispose();
                    _array.RemoveAt(0);
                    _data.MutauteValue(a=>new PermaObjArrayData(a.length-1,a.offset+1));
                }
                else
                {
                    foreach (var i in Loops.Range(index, this.length - 1))
                    {
                        this[i] = this[i + 1];
                    }
                    this._array.Last().DeleteOnDispose = true;
                    this._array.Last().Dispose();
                    this._array.RemoveAt(this._array.Count-1);
                    _data.MutauteValue(a => new PermaObjArrayData(a.length - 1,a.offset));
                }
            }
            public T this[int i]
            {
                get
                {
                    this.updateArr();
                    if (i < 0 || i >= _data.value.length)
                        throw new ArgumentOutOfRangeException("index "+i+" is outside bounds of permaArray");
                    return _array[i].value;
                }
                set
                {
                    this.updateArr();
                    if (i < 0 || i >= _data.value.length)
                        throw new ArgumentOutOfRangeException("index " + i + " is outside bounds of permaArray");
                    _array[i].value = value;
                }
            }
            public void MutauteValue(int i, Func<T, T> mutation)
            {
                this.updateArr();
                _array[i].MutauteValue(mutation);
            }
            public T tryParse(int i, out Exception ex)
            {
                this.updateArr();
                return _array[i].tryParse(out ex);
            }
            public string name { get; }
            public string LocalName() => _data.LocalName();
            public IEnumerator<T> GetEnumerator()
            {
                this.updateArr();
                return _array.Select(a => a.value).GetEnumerator();
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable<T>) this).GetEnumerator();
            }
            protected virtual void Dispose(bool disposing)
            {
                this.updateArr();
                if (disposing)
                {
                    foreach (IPermaObject<T> iPermaObject in _array)
                    {
                        iPermaObject.DeleteOnDispose = this.DeleteOnDispose;
                        iPermaObject.Dispose();
                    }
                    _data.DeleteOnDispose = this.DeleteOnDispose;
                    _data.Dispose();
                }
            }
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            private void offsetfiles(int offset)
            {
                updateArr();
                foreach (IPermaObject<T> iPermaObject in _array)
                {
                    iPermaObject.DeleteOnDispose = false;
                    iPermaObject.Dispose();
                }
                foreach (var fileindex in Loops.Range(_data.value.offset,_data.value.offset+_data.value.length).Reverse())
                {
                    File.Move(getname(fileindex),getname(fileindex+offset));
                }
                _data.MutauteValue(a=>new PermaObjArrayData(a.length,a.offset+offset));
                updateArr(true);
            }
            public int length
            {
                get
                {
                    this.updateArr();
                    return _data.value.length;
                }
            }
            public void Add(T item)
            {
                Insert(length,item);
            }
            public void Clear()
            {
                updateArr();
                foreach (var iPermaObject in _array)
                {
                    iPermaObject.DeleteOnDispose = true;
                    iPermaObject.Dispose();
                }
                _data.value = new PermaObjArrayData(0,0);
                updateArr(true);
            }
            public bool Contains(T item)
            {
                return IndexOf(item) > 0;
            }
            public void CopyTo(T[] array, int arrayIndex)
            {
                foreach (var t in _array.CountBind(arrayIndex))
                {
                    array[t.Item2] = t.Item1.value;
                }
            }
            public bool Remove(T item)
            {
                var i = IndexOf(item);
                if (i < 0)
                    return false;
                RemoveAt(i);
                return true;
            }
            public int Count
            {
                get
                {
                    return _data.value.length;
                }
            }
            public bool IsReadOnly => false;
        }
        public class PermaDictionary<K, V> : IDictionary<K,V>, ISafeDeletable
        {
            [Serializable]
            private class PermaDictionaryData
            {
                public PermaDictionaryData(int nextname, string definitions)
                {
                    this.nextname = nextname;
                    this.definitions = definitions;
                }
                public int nextname { get; }
                public string definitions { get; }
            }
            private readonly PermaObject<PermaDictionaryData> _data;
            private IDictionary<K, IPermaObject<V>> _dic;

            private readonly Func<byte[], K> _kread;
            private readonly Func<K, byte[]> _kwrite;
            private readonly Func<byte[], V> _vread;
            private readonly Func<V, byte[]> _vwrite;
            public bool DeleteOnDispose { get; set; }
            public FileAccess access
            {
                get
                {
                    return _data.access;
                }
            }
            public FileShare share
            {
                get
                {
                    return _data.share;
                }
            }
            public bool AllowCaching
            {
                get
                {
                    return _data.AllowCaching;
                }
            }
            public bool allowCaching { get; }
            private readonly V _vvalueIfCreated;
            public PermaDictionary(string name, bool allowCaching = true, FileAccess access = FileAccess.ReadWrite, bool deleteOnDispose = false, FileShare share = FileShare.None, FileMode mode = FileMode.OpenOrCreate, V vvalueIfCreated = default(V)) : this(a=> (K)Serialization.Deserialize(a), a=>Serialization.Serialize(a), a => (V)Serialization.Deserialize(a), a => Serialization.Serialize(a), name, allowCaching, access, deleteOnDispose, share, mode, vvalueIfCreated) { }
            public PermaDictionary(Func<byte[], K> kread, Func<K, byte[]> kwrite, Func<byte[], V> vread, Func<V, byte[]> vwrite, string name, bool allowCaching, FileAccess access = FileAccess.ReadWrite, bool deleteOnDispose = false, FileShare share = FileShare.None, FileMode mode = FileMode.OpenOrCreate,  V vvalueIfCreated = default(V))
            {
                _kread = kread;
                _kwrite = kwrite;
                _vread = vread;
                _vwrite = vwrite;
                this.allowCaching = allowCaching;
                this.DeleteOnDispose = deleteOnDispose;
                _vvalueIfCreated = vvalueIfCreated;
                _data = new PermaObject<PermaDictionaryData>(FilePath.MutateFileName(name, x=> "__DICTIONARY_DATA_"+x),deleteOnDispose,access,share,mode,new PermaDictionaryData(0,""),allowCaching);
                LoadDictionary(true);
            }
            public bool SupportMultiAccess => _data.share != FileShare.None;
            
            private IPermaObject<V> getVPerma(string name)
            {
                return new PermaObject<V>(_vread,_vwrite,System.IO.Path.Combine(System.IO.Path.GetDirectoryName(_data.name),"__DICTIONARYVALUE_"+name),DeleteOnDispose,_data.access, _data.share, valueIfCreated: _vvalueIfCreated, allowCaching: _data.AllowCaching);
            }
            private void LoadDictionary(bool @override = false)
            {
                if (!SupportMultiAccess && !@override)
                    return;
                string val = _data.value.definitions;
                List<string> split = new List<string>();
                while (val != "")
                {
                    split.Add(NumberSerialization.FullCodeSerializer.DecodeSpecifiedLength(val,out val));
                }
                _dic = split.Group2().Select(a => Tuple.Create(_kread(a.Item1.Select(x=>(byte)x).ToArray()), getVPerma(a.Item2))).ToDictionary();
            }
            private string SaveDictionaryToString()
            {
                return
                    _dic.SelectMany(
                        a => new [] { NumberSerialization.FullCodeSerializer.EncodeSpecificLength(_kwrite(a.Key).Select(x=>(char)x).ConvertToString()), NumberSerialization.FullCodeSerializer.EncodeSpecificLength(a.Value.name)})
                        .ToPrintable("", "", "");
            }
            
            public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
            {
                LoadDictionary();
                return _dic.Select(a => new KeyValuePair<K,V>(a.Key, a.Value.value)).GetEnumerator();
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
            public void Add(KeyValuePair<K, V> item)
            {
                LoadDictionary();
                if (!_dic.ContainsKey(item.Key))
                {
                    _dic.Add(item.Key, getVPerma(NumberSerialization.AlphaNumbreicSerializer.ToString((ulong)(_data.value.nextname + 1)).Reverse().ConvertToString()));
                    _data.MutauteValue(a => new PermaDictionaryData(a.nextname + 1, SaveDictionaryToString()));
                }
                _dic[item.Key].value = item.Value;
            }
            public void Clear()
            {
                LoadDictionary();
                foreach (var permaObject in _dic.Values)
                {
                    permaObject.DeleteOnDispose = true;
                    permaObject.Dispose();
                }
                _data.value = new PermaDictionaryData(0,"");
                _dic.Clear();
            }
            public bool Contains(KeyValuePair<K, V> item)
            {
                LoadDictionary();
                return _dic.ContainsKey(item.Key) && _dic[item.Key].value.Equals(item.Value);
            }
            public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
            {
                LoadDictionary();
                foreach (var t in _dic.CountBind())
                {
                    array[t.Item2] = new KeyValuePair<K, V>(t.Item1.Key,t.Item1.Value.value);
                }
            }
            public bool Remove(KeyValuePair<K, V> item)
            {
                LoadDictionary();
                if (!Contains(item))
                    return false;
                Remove(item.Key);
                return true;
            }
            public int Count {
                get
                {
                    LoadDictionary();
                    return _dic.Count;
                }
            }
            public bool IsReadOnly
            {
                get
                {
                    return _data.access == FileAccess.Read;
                }
            }
            public bool ContainsKey(K key)
            {
                LoadDictionary();
                return _dic.ContainsKey(key);
            }
            public void Add(K key, V value)
            {
                Add(new KeyValuePair<K, V>(key,value));
            }
            public bool Remove(K key)
            {
                LoadDictionary();
                if (!_dic.ContainsKey(key))
                    return false;
                var torem = _dic[key];
                torem.DeleteOnDispose = true;
                torem.Dispose();
                _dic.Remove(key);
                _data.MutauteValue(a=>new PermaDictionaryData(a.nextname,SaveDictionaryToString()));
                return true;
            }
            public bool TryGetValue(K key, out V value)
            {
                if (ContainsKey(key))
                {
                    value = _dic[key].value;
                    return true;
                }
                value = default(V);
                return false;
            }
            public V this[K key]
            {
                get
                {
                    V ret;
                    if (!TryGetValue(key,out ret))
                        throw new ArgumentOutOfRangeException(nameof(key));
                    return ret;
                }
                set
                {
                    Add(key,value);
                }
            }
            public ICollection<K> Keys
            {
                get
                {
                    return _dic.Keys;
                }
            }
            public ICollection<V> Values
            {
                get
                {
                    return new List<V>(_dic.Values.Select(a => a.value));
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            public void Dispose(bool disposing)
            {
                if (!disposing)
                    return;
                _data.DeleteOnDispose = this.DeleteOnDispose;
                _data.Dispose();
                foreach (var iPermaObject in _dic.Values)
                {
                    iPermaObject.DeleteOnDispose = DeleteOnDispose;
                    iPermaObject.Dispose();
                }
            }
        }
        public class PermaLabeledDictionary<T> : ISafeDeletable, IDictionary<string,T>
        {
            private readonly IPermaObject<string> _definitions;
            private IDictionary<string, IPermaObject<T>> _dic;
            public bool DeleteOnDispose { get; set; }
            public FileAccess access
            {
                get
                {
                    return _definitions.access;
                }
            }
            public FileShare share
            {
                get
                {
                    return _definitions.share;
                }
            }
            public bool AllowCaching
            {
                get
                {
                    return _definitions.AllowCaching;
                }
            }
            private readonly Func<byte[], T> _read;
            private readonly Func<T, byte[]> _write;
            private readonly string _defSeperator;
            private int _holdUpdateFlag = 0;
            public string name { get; }
            public bool SupportMultiAccess => (_definitions.share != FileShare.None);
            public PermaLabeledDictionary(string name, string defSeperator=null, bool deleteOnDispose = false, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.None, FileMode mode = FileMode.OpenOrCreate) : this(a => (T)Serialization.Deserialize(a), a => Serialization.Serialize(a), name, defSeperator, deleteOnDispose,access,share,mode) { }
            public PermaLabeledDictionary(Func<byte[], T> read, Func<T, byte[]> write, string name, string defSeperator=null, bool deleteOnDispose = false, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.None, FileMode mode = FileMode.OpenOrCreate)
            {
                _definitions = new PermaObject<string>(name, deleteOnDispose,access,share, mode,"");
                DeleteOnDispose = deleteOnDispose;
                _read = read;
                _write = write;
                this.name = name;
                _defSeperator = defSeperator ?? Environment.NewLine;
                this.RefreshDefinitions(true);
            }
            private void RefreshDefinitions(bool overridemulti = false)
            {
                if ((SupportMultiAccess || overridemulti) && (_holdUpdateFlag == 0))
                {
                    Exception ex;
                    this._definitions.tryParse(out ex);
                    if (ex != null)
                        this._definitions.value = "";
                    var defstring = this._definitions.value;
                    this._dic = new Dictionary<string, IPermaObject<T>>(defstring.Count(_defSeperator));
                    var keys = (defstring == ""
                        ? System.Linq.Enumerable.Empty<string>() : defstring.Split(new [] {_defSeperator}, StringSplitOptions.None));
                    foreach (string s in keys.Take(Math.Max(0,keys.Count()-1)))
                    {
                        this._dic[s] = new PermaObject<T>(_read, _write,
                            FilePath.MutateFileName(name, k => "__DICTIONARYMEMBER_" + s + "_" + k), DeleteOnDispose,_definitions.access, _definitions.share, FileMode.Open );
                    }
                }
            }
            public void MutauteValue(string i, Func<T, T> mutation)
            {
                _dic[i].MutauteValue(mutation);
            }
            public T tryParse(string i, out Exception ex)
            {
                this.RefreshDefinitions();
                return _dic[i].tryParse(out ex);
            }
            public bool ContainsKey(string key)
            {
                this.RefreshDefinitions();
                return _dic.ContainsKey(key);
            }
            public void Add(string key, T value)
            {
                this.RefreshDefinitions();
                _holdUpdateFlag++;
                this[key] = value;
                _holdUpdateFlag--;
            }
            public bool Remove(string key)
            {
                this.RefreshDefinitions();
                if (!_dic.ContainsKey(key))
                    return false;
                StringBuilder newdef = new StringBuilder(_definitions.value.Length + Environment.NewLine.Length * 2);
                foreach (string s in _definitions.value.Split(new[] {_defSeperator}, StringSplitOptions.None))
                {
                    if (s.Equals(key))
                        continue;
                    newdef.Append(s + _defSeperator);
                }
                _definitions.value = newdef.ToString();
                _dic[key].Dispose();
                if (!DeleteOnDispose)
                    File.Delete(_dic[key].name);
                _dic.Remove(key);
                return true;
            }
            public bool TryGetValue(string key, out T value)
            {
                this.RefreshDefinitions();
                value = default(T);
                if (!ContainsKey(key))
                    return false;
                Exception e;
                value = tryParse(key, out e);
                return e == null;
            }
            public T this[string identifier]
            {
                get
                {
                    this.RefreshDefinitions();
                    return _dic[identifier].value;
                }
                set
                {
                    this.RefreshDefinitions();
                    if (identifier.Contains(_defSeperator))
                        throw new Exception("cannot create entry with the separator in it");
                    if (!_dic.ContainsKey(identifier))
                    {
                        _dic[identifier] = new PermaObject<T>(_read, _write,
                            FilePath.MutateFileName(name, k => "__DICTIONARYMEMBER_" + identifier + "_" + k), DeleteOnDispose, _definitions.access, _definitions.share, FileMode.Create);
                        _definitions.value += identifier + _defSeperator;
                    }
                    _dic[identifier].value = value;
                }
            }
            public ICollection<string> Keys
            {
                get
                {
                    this.RefreshDefinitions();
                    return _dic.Keys;
                }
            }
            public ICollection<T> Values
            {
                get
                {
                    this.RefreshDefinitions();
                    return _dic.Values.SelectToArray(a => a.value);
                }
            }
            public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
            {
                this.RefreshDefinitions();
                return _dic.Select(a => new KeyValuePair<string,T>(a.Key, a.Value.value)).GetEnumerator();
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                this.RefreshDefinitions();
                return this.GetEnumerator();
            }
            public void Add(KeyValuePair<string, T> item)
            {
                this.RefreshDefinitions();
                _holdUpdateFlag++;
                Add(item.Key,item.Value);
                _holdUpdateFlag--;
            }
            public void Clear()
            {
                this.RefreshDefinitions();
                _holdUpdateFlag++;
                foreach (string key in Keys)
                {
                    this.Remove(key);
                }
                _holdUpdateFlag--;
            }
            public bool Contains(KeyValuePair<string, T> item)
            {
                this.RefreshDefinitions();
                _holdUpdateFlag++;
                var contains = ContainsKey(item.Key) && this[item.Key].Equals(item.Value);
                _holdUpdateFlag--;
                return contains;
            }
            public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
            {
                this.RefreshDefinitions();
                _holdUpdateFlag++;
                foreach (var key in Keys.CountBind(arrayIndex))
                {
                    array[key.Item2] = new KeyValuePair<string, T>(key.Item1,this[key.Item1]);
                }
                _holdUpdateFlag--;
            }
            public bool Remove(KeyValuePair<string, T> item)
            {
                this.RefreshDefinitions();
                _holdUpdateFlag++;
                var t = Contains(item);
                if (t)
                {
                    Remove(item.Key);
                    _holdUpdateFlag--;
                    return true;
                }
                _holdUpdateFlag--;
                return false;
            }
            public int Count
            {
                get
                {
                    this.RefreshDefinitions();
                    return _dic.Count;
                }
            }
            public bool IsReadOnly
            {
                get
                {
                    return !_definitions.access.HasFlag(FileAccess.Write);
                }
            }
            protected virtual void Dispose(bool disposing)
            {
                this.RefreshDefinitions();
                if (disposing)
                {
                    foreach (KeyValuePair<string, IPermaObject<T>> iPermaObject in _dic)
                    {
                        iPermaObject.Value.DeleteOnDispose = DeleteOnDispose;
                        iPermaObject.Value.Dispose();
                    }
                    _definitions.DeleteOnDispose = DeleteOnDispose;
                    _definitions.Dispose();
                }
            }
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }
        public class PermaCollection<T> : ICollection<T>, ISafeDeletable
        {
            private readonly PermaLabeledDictionary<T> _int;
            private readonly PermaObject<long> _maxname;
            public PermaCollection(string name,  bool deleteOnDispose = false, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.None, FileMode mode = FileMode.OpenOrCreate) : this(a => (T)Serialization.Deserialize(a), a => Serialization.Serialize(a), name, deleteOnDispose,access,share,mode) { }
            public PermaCollection(Func<byte[], T> read, Func<T, byte[]> write, string name, bool deleteOnDispose = false, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.None, FileMode mode = FileMode.OpenOrCreate)
            {
                _int = new PermaLabeledDictionary<T>(read,write,name,null, deleteOnDispose, access, share, mode);
                _maxname = new PermaObject<long>(FilePath.MutateFileName(name, k => "__COLLECTIONMAXINDEX_" + k), deleteOnDispose, access, share, mode);
            }
            public IEnumerator<T> GetEnumerator()
            {
                return _int.Values.GetEnumerator();
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
            public void Add(T item)
            {
                BigInteger i = _maxname.value++;
                _int[i.ToString("X")] = item;
            }
            public void Clear()
            {
                _int.Clear();
                _maxname.value = 0;
            }
            public bool Contains(T item)
            {
                return _int.Values.Contains(item);
            }
            public void CopyTo(T[] array, int arrayIndex)
            {
                _int.Values.CopyTo(array,arrayIndex);
            }
            public bool Remove(T item)
            {
                foreach (var p in _int)
                {
                    if (p.Value.Equals(item))
                    {
                        _int.Remove(p);
                        return true;
                    }
                }
                return false;
            }
            public int Count
            {
                get
                {
                    return _int.Count;
                }
            }
            public bool IsReadOnly
            {
                get
                {
                    return _int.IsReadOnly;
                }
            }
            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _int.Dispose();
                    _maxname.Dispose();
                }
            }
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            public bool DeleteOnDispose
            {
                get
                {
                    return _int.DeleteOnDispose;
                }
                set
                {
                    _int.DeleteOnDispose = value;
                    _maxname.DeleteOnDispose = value;
                }
            }
            public FileAccess access
            {
                get
                {
                    return _maxname.access;
                }
            }
            public FileShare share
            {
                get
                {
                    return _maxname.share;
                }
            }
            public bool AllowCaching
            {
                get
                {
                    return _maxname.AllowCaching;
                }
            }
        }
    }
}
