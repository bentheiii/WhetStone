using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WhetStone.Serializations
{
    public static class serialize
    {
        public static T Deserialize<T>(byte[] arr)
        {
            return (T)Deserialize(arr);
        }
        public static object Deserialize(byte[] arr)
        {
            if (arr == null)
                throw new ArgumentNullException(nameof(arr));
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arr, 0, arr.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            return binForm.Deserialize(memStream);
        }
        public static byte[] Serialize(object o)
        {
            if (o == null)
                throw new ArgumentNullException(nameof(o));
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream()) {
                bf.Serialize(ms, o);
                return ms.ToArray();
            }
        }
        public static byte[] Serialize<T>(T o)
        {
            return Serialize((object)o);
        }
    }
}
