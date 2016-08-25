using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Arrays;
using WhetStone.Comparison;
using WhetStone.Complex;
using WhetStone.Dictionaries;
using WhetStone.Factory;
using WhetStone.Fielding;
using WhetStone.Funnels;
using WhetStone.Looping;
using WhetStone.Matrix;
using WhetStone.NumbersMagic;
//todo keep this?
namespace WhetStone.Formulas
{
    using static Fields;
    public class FormulaField<G> : Field<Formula<G>>
    {
        private readonly Field<G> _int;
        public FormulaField(Field<G> i)
        {
            this._int = i;
        }
        public override Formula<G> zero => this._int.zero;
        public override Formula<G> one => _int.one;
        public override Formula<G> naturalbase => _int.naturalbase;
        public override Formula<G> add(Formula<G> a, Formula<G> b) => a + b;
        public override Formula<G> pow(Formula<G> a, Formula<G> b) => a ^ b;
        public override Formula<G> mod(Formula<G> a, Formula<G> b)
        {
            throw new NotSupportedException();
        }
        public override int Compare(Formula<G> x, Formula<G> y) => _int.Compare(x[_int.zero], y[_int.zero]);
        public override Formula<G> fromInt(int x) => this._int.fromInt(x);
        public override Formula<G> fromInt(ulong x) => this._int.fromInt(x);
        public override Formula<G> abs(Formula<G> x) => new AbsFormula<G>(x);
        public override Formula<G> Conjugate(Formula<G> a) => a;
        public override Formula<G> divide(Formula<G> a, Formula<G> b) => a / b;
        public override Formula<G> Invert(Formula<G> x) => _int.one / x;
        public override Formula<G> log(Formula<G> a) => new LogFormula<G>(a);
        public override Formula<G> multiply(Formula<G> a, Formula<G> b) => a * b;
        public override Formula<G> Negate(Formula<G> x) => -x;
        public override Formula<G> subtract(Formula<G> a, Formula<G> b) => a - b;
        public override double? toDouble(Formula<G> a) => _int.toDouble(a[_int.zero]);
        public override bool ModduloAble => false;
        public override Formula<G> fromFraction(double a)
        {
            return new ConstantFormula<G>(_int.fromFraction(a));
        }
        public override Formula<G> fromFraction(int a, int b)
        {
            return new ConstantFormula<G>(_int.fromFraction(a,b));
        }
    }
    public abstract class Formula<T> : IEquatable<Formula<T>>
    {
        static Formula()
        {
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle);
            setField(new FormulaField<T>(getField<T>()));
        }
        public abstract T this[params T[] x0] { get; }
        public abstract Formula<T> derive(int deriveindex = 0);
        public Formula<T> derive(IEnumerable<int> indices)
        {
            var ret = this;
            foreach (int index in indices)
            {
                ret = ret.derive(index);
            }
            return ret;
        }   
        public abstract bool hasValue(params T[] x0);
        public abstract bool Equals(Formula<T> other);
        public abstract override string ToString();
        public virtual Funnel<Tuple<Formula<T>, FormulaAggregator<T>>> getOptimiserFunnel()
        {
            Funnel<Tuple<Formula<T>, FormulaAggregator<T>>> defFunnel = new Funnel<Tuple<Formula<T>, FormulaAggregator<T>>>();
            return defFunnel;
        }
        //output type must be of same type as initial type
        public virtual Formula<T> optimisedInternals()
        {
            return this;
        }  
        public static Formula<T> Add(Formula<T> a, Formula<T> b)
        {
            return a + b;
        }
        public static Formula<T> Multiply(Formula<T> a, Formula<T> b)
        {
            return a * b;
        }
        public static implicit operator Formula<T>(T val)
        {
            return new ConstantFormula<T>(val);
        }
        public static implicit operator Func<T,T>(Formula<T> val)
        {
            return a => val[a] ;
        }
        public static Formula<T> operator -(Formula<T> a)
        {
            return getField<T>().negativeone * a;
        }
        public static Formula<T> operator ^(Formula<T> a, T b)
        {
            return new ExponentFormula<T>(a, b);
        }
        public static Formula<T> operator ^(Formula<T> a, int b)
        {
            return a ^ getField<T>().fromInt(b);
        }
        public static Formula<T> operator ^(Formula<T> a, Formula<T> b)
        {
            return new ExponentFormula<T>(a, b);
        }
        public static Formula<T> operator *(Formula<T> a, Formula<T> b)
        {
            return new ProductFormula<T>(a, b);
        }
        public static Formula<T> operator +(Formula<T> a, Formula<T> b)
        {
            return new SumFormula<T>(a, b);
        }
        public static Formula<T> operator -(Formula<T> a, Formula<T> b)
        {
            return new DifferenceFormula<T>(a, b);
        }
        public static Formula<T> operator /(Formula<T> a, Formula<T> b)
        {
            return new QuotientFormula<T>(a, b);
        }
        public static Formula<T> x { get; } = new IdentFormula<T>(0,"x");
        public static Formula<T> y { get; } = new IdentFormula<T>(1, "y");
        public static Formula<T> z { get; } = new IdentFormula<T>(2, "z");
        public Formula<T> log()
        {
            return new LogFormula<T>(this);
        }
        public Formula<T> log(T @base)
        {
            return new LogFormula<T>(this) / getField<T>().log(@base);
        }
        public bool Equals(T val)
        {
            return this is ConstantFormula<T> && (this as ConstantFormula<T>)._val.Equals(val);
        }
        public override bool Equals(object obj)
        {
            if (obj is T)
                return this.Equals((T)obj);
            var a = obj as Formula<T>;
            return a != null && this.Equals(a);
        }
        public override int GetHashCode()
        {
            var f = getField<T>();
            int ret = 0;
            foreach (int i in range.Range(10))
            {
                if (this.hasValue(f.fromInt(i)))
                    ret ^= this[f.fromInt(i)].GetHashCode();
                else
                    ret ^= i.GetHashCode();
            }
            return ret;
        }
    }
    public static class Formula
    {
        public static Formula<T> interpolatePolynomial<T>(this Formula<T> f, T start, T end, int order)
        {
            var field = getField<T>();
            T step = field.divide(field.subtract(end, start), field.fromInt(order+1));
            var x = range.IRange(start, end, step);
            var y = x.Select(a => f[a]);
            return fromDataPoints(x, y);
        }
        
        public static Formula<T> fromDataPoints<T>(IEnumerable<T> x, IEnumerable<T> y)
        {
            var f = getField<T>();
            var com = x.Zip(y);
            int c = x.Count();
            T[,] pa = new T[c,c];
            T[,] pb = new T[c,1];
            int row = 0;
            foreach (Tuple<T, T> tuple in com)
            {
                T pow = f.one;
                foreach (int col in range.Range(c))
                {
                    pa[row, col] = pow;
                    pow = f.multiply(pow, tuple.Item1);
                }
                pb[row, 0] = tuple.Item2;
                row++;
            }
            Matrix<T> a = Matrix<T>.fromArr(pa);
            Matrix<T> b = Matrix<T>.fromArr(pb);
            if (!a.invertible())
                return null;
            T[] factors = ((IEnumerable<T>)(Array)(a.inverse().Multiply(b).to2DArr())).ToArray();
            FormulaAggregator<T> ret = new FormulaAggregator<T>();
            for (int i = 0; i < factors.Length; i++)
            {
                ret.Add(factors[i] * (Formula<T>.x ^ i));
            }
            return ret.Create(Formula<T>.Add);
        }
        public static Formula<T> fromRoots<T>(params T[] roots)
        {
            FormulaAggregator<T> ret = new FormulaAggregator<T>();
            foreach (T t in roots)
            {
                ret.Add(Formula<T>.x - t);
            }
            return ret.Create(Formula<T>.Multiply);
        }
        public static T ApproximateSolution<T>(this Formula<T> a, T start1, T start2, T root, T tolerance)
        {
            var f = getField<T>();
            if (f.Compare(root, a[start1]) == 0)
                return start1;
            if (f.Compare(root, a[start2]) == 0)
                return start2;
            if (f.Compare(root, a[start2]) * f.Compare(root, a[start1]) > 0)
                throw new ArgumentException("the starting points are not above and below the root");
            T two = f.add(f.one, f.one);
            T x1 = start1;
            T x2 = start2;
            T x3 = f.divide(f.add(x1, x2), two);
            while (f.Compare(tolerance, f.abs(f.subtract(x1, x2))) < 0)
            {
                if (f.Compare(root, a[x2]) * f.Compare(root, a[x3]) < 0)
                    x1 = x3;
                else
                    x2 = x3;
                x3 = f.divide(f.add(x1, x2), two);
            }
            return x3;
        }
        public static T ApproximateSolution<T>(this Formula<T> a, T start1, T tolerance, params T[] additionalCoordinates)
        {
            var x3 = start1.ToFieldWrapper();
            var tol = tolerance.ToFieldWrapper();
            var d = a.derive();
            T[] coors;
            FieldWrapper<T> val;
            while ((val = a[(coors = x3.val.Enumerate().Concat(additionalCoordinates).ToArray())]).abs() > tol)
            {
                x3 = x3 - (val/d[coors].ToFieldWrapper());
            }
            return x3;
        }
        public static Formula<T> getTaylor<T>(this Formula<T> @this, T arountpoint, int componentcount)
        {
            Field<T> f = getField<T>();
            FormulaAggregator<T> ret = new FormulaAggregator<T>();
            Formula<T> cur = @this;
            T factorial = f.one;
            T count = f.zero;
            foreach (int i in range.Range(componentcount))
            {
                ret.Add(f.multiply(f.Invert(factorial), cur[arountpoint]) * ((arountpoint - Formula<T>.x) ^ i));
                count = f.add(count, f.one);
                factorial = f.multiply(factorial, count);
                cur = cur.derive();
            }
            return ret.Create(Formula<T>.Add);
        }
        public static Formula<T> Optimise<T>(this Formula<T> @this)
        {
            var preOpt = @this.optimisedInternals();
            var f = preOpt.getOptimiserFunnel();
            f.Add(processed =>
            {
                processed.Item2.Add(processed.Item1);
                return true;
            });
            FormulaAggregator<T> ag = new FormulaAggregator<T>();
            f.Process(new Tuple<Formula<T>, FormulaAggregator<T>>(preOpt, ag));
            return ag.count == 0 ? getField<T>().zero : ag.Create(Formula<T>.Multiply);
        }
        public static ISet<Formula<T>> getAdditonComponents<T>(this Formula<T> @this, out bool optimised)
        {
            IDictionary<Formula<T>, Formula<T>> dic = new Dictionary<Formula<T>, Formula<T>>();
            var field = getField<T>();
            @this.getAdditonComponents(dic, out optimised);
            ISet<Formula<T>> ret = new HashSet<Formula<T>>();
            int constants = 0;
            foreach (KeyValuePair<Formula<T>, Formula<T>> formula in dic)
            {
                if (formula.Key is ConstantFormula<T>)
                    constants++;
                if (formula.Value.Equals(field.zero) || formula.Key.Equals(field.zero))
                    continue;
                if (formula.Value.Equals(field.one))
                {
                    ret.Add(formula.Key);
                    continue;
                }
                ret.Add(formula.Value * formula.Key);
            }
            optimised |= constants > 1;
            return ret;
        }
        private static void getAdditonComponents<T>(this Formula<T> @this, IDictionary<Formula<T>, Formula<T>> ret, out bool optimised)
        {
            optimised = false;
            SumFormula<T> sumFormula = @this as SumFormula<T>;
            if (sumFormula != null)
            {
                bool temp;
                sumFormula._arg1.getAdditonComponents(ret, out temp);
                optimised |= temp;
                sumFormula._arg2.getAdditonComponents(ret, out temp);
                optimised |= temp;
                return;
            }
            ProductFormula<T> pformula = @this as ProductFormula<T>;
            if (pformula != null)
            {
                if (pformula._arg2 is ConstantFormula<T>)
                {
                    ret.AggregateValue(pformula._arg1, pformula._arg2, (a, b) => a + b);
                    optimised |= ret.ContainsKey(pformula._arg1);
                    return;
                }
                ret.AggregateValue(pformula._arg2, pformula._arg1, (a, b) => a + b);
                optimised |= ret.ContainsKey(pformula._arg2);
                return;
            }
            ret.AggregateValue(@this, getField<T>().one, (a, b) => a + b);
            optimised |= ret.ContainsKey(@this);
        }
        public static ISet<Formula<T>> getProductComponents<T>(this Formula<T> @this, out bool optimised)
        {
            IDictionary<Formula<T>, Formula<T>> dic =
                new Dictionary<Formula<T>, Formula<T>>(
                    EqualityComparer<Formula<T>>.Default);
            var field = getField<T>();
            @this.getProductComponents(dic, out optimised);
            ISet<Formula<T>> ret = new HashSet<Formula<T>>();
            int constants = 0;
            foreach (KeyValuePair<Formula<T>, Formula<T>> formula in dic)
            {
                if (formula.Key is ConstantFormula<T>)
                    constants++;
                if (formula.Key.Equals(field.zero))
                {
                    optimised = true;
                    ret.Clear();
                    ret.Add(field.zero);
                    break;
                }
                if (formula.Value.Equals(field.zero))
                {
                    ret.Add(field.one);
                    continue;
                }
                if (formula.Value.Equals(field.one))
                {
                    ret.Add(formula.Key);
                    continue;
                }
                ret.Add(formula.Key ^ formula.Value);
            }
            optimised |= constants > 1;
            return ret;
        }
        private static void getProductComponents<T>(this Formula<T> @this, IDictionary<Formula<T>, Formula<T>> ret, out bool optimised)
        {
            optimised = false;
            ProductFormula<T> sumFormula = @this as ProductFormula<T>;
            if (sumFormula != null)
            {
                bool temp;
                sumFormula._arg1.getProductComponents(ret, out temp);
                optimised |= temp;
                sumFormula._arg2.getProductComponents(ret, out temp);
                optimised |= temp;
                return;
            }
            ExponentFormula<T> pformula = @this as ExponentFormula<T>;
            if (pformula != null)
            {
                ret.AggregateValue(pformula._base, pformula._pow, (a, b) => a + b);
                optimised |= ret.ContainsKey(pformula._base);
                return;
            }
            ret.AggregateValue(@this, getField<T>().one, (a, b) => a + b);
            optimised |= ret.ContainsKey(@this);
        }
        public static T approximateIntegral<T>(this Formula<T> @this, T start, T end, int segments)
        {
            return approximateIntegral(@this, start, end, segments, new T[] {});
        }
        public static T approximateIntegral<T>(this Formula<T> @this, T start, T end, int segments, T[] additionalCoordinates)
        {
            var field = getField<T>();
            return approximateIntegral(@this, start, end, field.divide(field.subtract(end,start),field.fromInt(segments)),additionalCoordinates);
        }
        public static T approximateIntegral<T>(this Formula<T> @this, T start, T end, T interval)
        {
            return approximateIntegral(@this, start, end, interval, new T[] {});
        }
        public static T approximateIntegral<T>(this Formula<T> @this, T start, T end, T interval, T[] additionalCoordinates)
        {
            var field = getField<T>();
            T ret = field.zero;
            T x = field.add(start, field.divide(interval, field.fromInt(2)));
            while (field.Compare(x,end) < 0)
            {
                ret = field.add(ret, @this[(x.Enumerate().Concat(additionalCoordinates)).ToArray()]);
                x = field.add(x, interval);
            }
            return field.multiply(interval, ret);
        }
        public static T approximateDerivitive<T>(this Formula<T> @this, T x, T interval)
        {
            var field = getField<T>();
            return field.divide(field.subtract(@this[field.add(x, interval)], @this[field.subtract(x, interval)]),
                                field.multiply(interval, field.fromInt(2)));
        }
        public static bool neatlyDevisibleBy<T>(this Formula<T> component, Formula<T> quotient)
        {
            return component.Equals(quotient) || ((component as ExponentFormula<T>)?._base?.Equals(quotient)).GetValueOrDefault(false);
        }
        public static Formula<T> neatlydivide<T>(this Formula<T> @this, IEnumerable<Formula<T>> quotient)
        {
            bool op;
            ISet<Formula<T>> undividedby = new HashSet<Formula<T>>(quotient);
            ISet<Formula<T>> ret = new HashSet<Formula<T>>();
            foreach (var productComponent in @this.getProductComponents(out op))
            {
                var comp = productComponent;
                foreach (Formula<T> formula in undividedby)
                {
                    if (productComponent.neatlyDevisibleBy(formula))
                    {
                        var expo = comp as ExponentFormula<T>;
                        if (expo != null)
                            comp = expo._base ^ (expo._pow - getField<T>().one);
                        else
                            comp = getField<T>().one;
                        undividedby.Remove(formula);
                        break;
                    }
                }
                ret.Add(comp);
            }
            return
                ret.Aggregate<Formula<T>, Formula<T>>(new ConstantFormula<T>(getField<T>().one), (i, j) => i * j)
                   .Optimise();
        }
        public static MemoryFormula<T> toMemory<T>(this Formula<T> @this)
        {
            return new MemoryFormula<T>(@this);
        }
    }
    public class FormulaAggregator<T> : ICreator<Func<Formula<T>, Formula<T>, Formula<T>>, Formula<T>>
    {
        private readonly ISet<Formula<T>> _formulas = new HashSet<Formula<T>>();
        public int count => this._formulas.Count;
        public void Add(Formula<T> f)
        {
            _formulas.Add(f);
        }
        public Formula<T> Create(Func<Formula<T>, Formula<T>, Formula<T>> f)
        {
            if (this.count == 0)
                throw new Exception("Aggregator is empty");
            Formula<T> ret = null;
            foreach (Formula<T> formula in _formulas)
            {
                ret = ret == null ? formula : f(ret, formula);
            }
            return ret;
        }
    }
    public class ConstantFormula<T> : Formula<T>
    {
        internal readonly T _val;
        public ConstantFormula() : this(getField<T>().zero) { }
        public ConstantFormula(T val)
        {
            this._val = val;
        }
        public override T this[params T[] x0]
        {
            get
            {
                return this._val;
            }
        }
        public override Formula<T> derive(int deriveindex = 0)
        {
            return new ConstantFormula<T>();
        }
        public override bool hasValue(params T[] x0)
        {
            return true;
        }
        public override bool Equals(Formula<T> other)
        {
            return other is ConstantFormula<T> && this._val.Equals(((ConstantFormula<T>)other)._val);
        }
        public override string ToString()
        {
            Field<T> f = getField<T>();
            return f.String(this._val);
        }
        public override Funnel<Tuple<Formula<T>, FormulaAggregator<T>>> getOptimiserFunnel()
        {
            Funnel<Tuple<Formula<T>, FormulaAggregator<T>>> constoptimizer = new Funnel<Tuple<Formula<T>, FormulaAggregator<T>>>
            {
                a =>
                {
                    var field = getField<T>();
                    return a.Item1.Equals(field.zero);
                }
            };
            //0=>()
            return constoptimizer;
        }
    }
    public class IdentFormula<T> : Formula<T>
    {
        public override T this[params T[] x0]
        {
            get
            {
                return x0.Length <= this._coorIndex ? getField<T>().zero : x0[this._coorIndex];
            }
        }
        private readonly int _coorIndex;
        private readonly string _name;
        public IdentFormula(int coorIndex) : this(coorIndex, "x_"+coorIndex) { }
        public IdentFormula(int coorIndex, string name)
        {
            this._coorIndex = coorIndex;
            _name = name;
        }
        public override Formula<T> derive(int deriveindex = 0)
        {
            return new ConstantFormula<T>(deriveindex == _coorIndex ? getField<T>().one : getField<T>().zero);
        }
        public override bool hasValue(params T[] x0)
        {
            return true;
        }
        public override bool Equals(Formula<T> other)
        {
            IdentFormula<T> formula = other as IdentFormula<T>;
            return formula != null && formula._coorIndex == this._coorIndex;
        }
        public override string ToString()
        {
            return _name;
        }
    }
    public class SumFormula<T> : Formula<T>
    {
        internal readonly Formula<T> _arg1, _arg2;
        public SumFormula(Formula<T> arg1, Formula<T> arg2)
        {
            this._arg1 = arg1;
            this._arg2 = arg2;
        }
        public override T this[params T[] x0]
        {
            get
            {
                return getField<T>().add(this._arg1[x0], this._arg2[x0]);
            }
        }
        public override Formula<T> derive(int deriveindex = 0)
        {
            return this._arg1.derive(deriveindex) + this._arg2.derive(deriveindex);
        }
        public override bool hasValue(params T[] x0)
        {
            return this._arg1.hasValue(x0) && this._arg2.hasValue(x0);
        }
        public override bool Equals(Formula<T> other)
        {
            var sum = other as SumFormula<T>;
            if (sum == null)
                return false;
            return (sum._arg1.Equals(this._arg1) && sum._arg2.Equals(this._arg2)) || (sum._arg1.Equals(this._arg2) && sum._arg2.Equals(this._arg1));
        }
        public override string ToString()
        {
            return this._arg1 + " + " + this._arg2;
        }
        public override Funnel<Tuple<Formula<T>, FormulaAggregator<T>>> getOptimiserFunnel()
        {
            Funnel<Tuple<Formula<T>, FormulaAggregator<T>>> sumoptimizer = new Funnel<Tuple<Formula<T>, FormulaAggregator<T>>>
            {
                a =>
                {
                    var formula = a.Item1 as SumFormula<T>;
                    var field = getField<T>();
                    if (formula._arg1 is ConstantFormula<T> && formula._arg2 is ConstantFormula<T>)
                    {
                        a.Item2.Add(a.Item1[field.zero]);
                        return true;
                    }
                    return false;
                },
                a =>
                {
                    var formula = a.Item1 as SumFormula<T>;
                    var field = getField<T>();
                    if (formula._arg1.Equals(field.zero))
                    {
                        a.Item2.Add(formula._arg2);
                        return true;
                    }
                    if (formula._arg2.Equals(field.zero))
                    {
                        a.Item2.Add(formula._arg1);
                        return true;
                    }
                    return false;
                },
                a =>
                {
                    var formula = a.Item1 as SumFormula<T>;
                    var field = getField<T>();
                    ProductFormula<T> internalproduct = formula._arg1 as ProductFormula<T>;
                    if (internalproduct != null)
                    {
                        if (internalproduct._arg1.Equals(field.fromInt(-1)))
                        {
                            a.Item2.Add(formula._arg2 - internalproduct._arg2);
                            return true;
                        }
                        if (internalproduct._arg2.Equals(field.fromInt(-1)))
                        {
                            a.Item2.Add(formula._arg2 - internalproduct._arg1);
                            return true;
                        }
                    }
                    internalproduct = formula._arg2 as ProductFormula<T>;
                    if (internalproduct != null)
                    {
                        if (internalproduct._arg1.Equals(field.fromInt(-1)))
                        {
                            a.Item2.Add(formula._arg1 - internalproduct._arg2);
                            return true;
                        }
                        if (internalproduct._arg2.Equals(field.fromInt(-1)))
                        {
                            a.Item2.Add(formula._arg1 - internalproduct._arg1);
                            return true;
                        }
                    }
                    return false;
                },
                a =>
                {
                    bool op;
                    var field = getField<T>();
                    var components = a.Item1.getAdditonComponents(out op);
                    ISet<Formula<T>> devisable = new HashSet<Formula<T>>();
                    foreach (Formula<T> productComponent in components.First().getProductComponents(out op))
                    {
                        if (components.Skip(1).All(b => b.getProductComponents(out op).Any(c => c.neatlyDevisibleBy(productComponent))))
                        {
                            devisable.Add(productComponent);
                        }
                        else
                        {
                            ExponentFormula<T> formula = productComponent as ExponentFormula<T>;
                            if (formula != null)
                            {
                                if (components.Skip(1).All(b => b.getProductComponents(out op).Any(c => c.neatlyDevisibleBy(formula._base))))
                                {
                                    devisable.Add(formula._base);
                                }
                            }
                        }
                    }
                    if (devisable.Count == 0)
                        return false;
                    var @out = devisable.Aggregate((i, j) => i*j);
                    var @in =
                        components.Select(
                            formula => formula.neatlydivide(devisable)).Aggregate((i, j) => i + j).Optimise();
                    a.Item2.Add(@out*@in);
                    return true;
                },
                a =>
                {
                    bool op;
                    var components = a.Item1.getAdditonComponents(out op);
                    if (!op)
                        return false;
                    FormulaAggregator<T> aggregator = new FormulaAggregator<T>();
                    Field<T> field = getField<T>();
                    T constants = field.zero;
                    foreach (Formula<T> component in components)
                    {
                        var optimise = component.Optimise();
                        var formula = optimise as ConstantFormula<T>;
                        if (formula != null)
                        {
                            if (!formula.Equals(field.zero))
                                constants = field.add(constants, formula._val);
                        }
                        else
                        {
                            aggregator.Add(optimise);
                        }
                    }
                    if (!field.zero.Equals(constants))
                        aggregator.Add(constants);
                    a.Item2.Add(aggregator.Create(Add));
                    return true;
                }
            };
            //c0+c1=>(c0+c1)
            //0+f=>f
            //a+(-1)*b => a-b
            //(ab+bc) -> a(b+c)
            //component grouper
            return sumoptimizer;
        }
        public override Formula<T> optimisedInternals()
        {
            return _arg1.Optimise() + _arg2.Optimise();
        }
    }
    public class DifferenceFormula<T> : SumFormula<T>
    {
        private readonly Formula<T> _natarg2;
        public DifferenceFormula(Formula<T> arg1, Formula<T> arg2) : base(arg1, -arg2)
        {
            this._natarg2 = arg2;
        }
        public override string ToString()
        {
            return this._arg1 + "-(" + this._natarg2 + ")";
        }
    }
    public class ProductFormula<T> : Formula<T>
    {
        internal readonly Formula<T> _arg1, _arg2;
        public ProductFormula(Formula<T> arg1, Formula<T> arg2)
        {
            this._arg1 = arg1;
            this._arg2 = arg2;
        }
        public override T this[params T[] x0]
        {
            get
            {
                return getField<T>().multiply(this._arg1[x0], this._arg2[x0]);
            }
        }
        public override Formula<T> derive(int deriveindex = 0)
        {
            return this._arg1.derive(deriveindex) * this._arg2 + this._arg2.derive(deriveindex) * this._arg1;
        }
        public override bool hasValue(params T[] x0)
        {
            return this._arg1.hasValue(x0) && this._arg2.hasValue(x0);
        }
        public override bool Equals(Formula<T> other)
        {
            var sum = other as ProductFormula<T>;
            if (sum == null)
                return false;
            return (sum._arg1.Equals(this._arg1) && sum._arg2.Equals(this._arg2)) || (sum._arg1.Equals(this._arg2) && sum._arg2.Equals(this._arg1));
        }
        public override string ToString()
        {
            return "(" + this._arg1 + ") * (" + this._arg2 + ")";
        }
        public override Funnel<Tuple<Formula<T>, FormulaAggregator<T>>> getOptimiserFunnel()
        {
            Funnel<Tuple<Formula<T>, FormulaAggregator<T>>> productoptimizer = new Funnel<Tuple<Formula<T>, FormulaAggregator<T>>>
            {
                a =>
                {
                    var formula = a.Item1 as ProductFormula<T>;
                    var field = getField<T>();
                    if (formula._arg1 is ConstantFormula<T> && formula._arg2 is ConstantFormula<T>)
                    {
                        a.Item2.Add(a.Item1[field.zero]);
                        return true;
                    }
                    return false;
                },
                a =>
                {
                    var formula = a.Item1 as ProductFormula<T>;
                    var field = getField<T>();
                    if (formula._arg1.Equals(field.one))
                    {
                        a.Item2.Add(formula._arg2);
                        return true;
                    }
                    if (formula._arg2.Equals(field.one))
                    {
                        a.Item2.Add(formula._arg1);
                        return true;
                    }
                    return false;
                },
                a =>
                {
                    bool op;
                    var components = a.Item1.getProductComponents(out op);
                    if (!op)
                        return false;
                    FormulaAggregator<T> aggregator = new FormulaAggregator<T>();
                    Field<T> field = getField<T>();
                    T constants = field.one;
                    foreach (Formula<T> component in components)
                    {
                        var optimise = component.Optimise();
                        var formula = optimise as ConstantFormula<T>;
                        if (formula != null)
                        {
                            if (formula.Equals(field.zero))
                            {
                                return true;
                            }
                            if (!formula.Equals(field.one))
                                constants = field.multiply(constants, formula._val);
                        }
                        else
                            aggregator.Add(optimise);
                    }
                    aggregator.Add(constants);
                    a.Item2.Add(aggregator.Create(Multiply));
                    return true;
                }
            };
            //c0*c1=>(c0*c1)
            //1*f=>f
            //component grouper
            return productoptimizer;
        }
        public override Formula<T> optimisedInternals()
        {
            return _arg1.Optimise() * _arg2.Optimise();
        }
    }
    public class QuotientFormula<T> : ProductFormula<T>
    {
        private readonly Formula<T> _natarg2;
        public QuotientFormula(Formula<T> arg1, Formula<T> arg2) : base(arg1, arg2 ^ -1)
        {
            this._natarg2 = arg2;
        }
        public override string ToString()
        {
            return "(" + this._arg1 + ")/(" + this._natarg2 + ")";
        }
    }
    public class LogFormula<T> : Formula<T>
    {
        internal readonly Formula<T> _int;
        public LogFormula(Formula<T> i)
        {
            this._int = i;
            Field<T> field = getField<T>();
        }
        public override T this[params T[] x0]
        {
            get
            {
                Field<T> field = getField<T>();
                return field.log(this._int[x0]);
            }
        }
        public override Formula<T> derive(int deriveindex = 0)
        {
            return (this._int.derive(deriveindex) / this._int) ;
        }
        public override bool hasValue(params T[] x0)
        {
            Field<T> f = getField<T>();
            return this._int.hasValue(x0) && f.Compare(this._int[x0], f.zero) != 0;
        }
        public override bool Equals(Formula<T> other)
        {
            var sum = other as LogFormula<T>;
            return sum != null && sum._int.Equals(this._int);
        }
        public override string ToString()
        {
            Field<T> f = getField<T>();
            return "ln(" + this._int + ")";
        }
        public override Formula<T> optimisedInternals()
        {
            return _int.Optimise().log();
        }
        public override Funnel<Tuple<Formula<T>, FormulaAggregator<T>>> getOptimiserFunnel()
        {
            Funnel<Tuple<Formula<T>, FormulaAggregator<T>>> ret = new Funnel<Tuple<Formula<T>, FormulaAggregator<T>>>
            {
                a =>
                {
                    var formula = a.Item1 as LogFormula<T>;
                    var field = getField<T>();
                    if (formula._int is ConstantFormula<T>)
                    {
                        a.Item2.Add(a.Item1[field.zero]);
                        return true;
                    }
                    return false;
                },
                a =>
                {
                    var formula = a.Item1 as LogFormula<T>;
                    var field = getField<T>();
                    ExponentFormula<T> exponentFormula = formula._int as ExponentFormula<T>;
                    if (exponentFormula != null)
                    {
                        a.Item2.Add(exponentFormula._pow*exponentFormula._base.log());
                        return true;
                    }
                    return false;
                }
            };
            //log(c)=>(log(c))
            //log(f1^f2)=>f2*log(f1)
            return ret;
        }
    }
    public class ExponentFormula<T> : Formula<T>
    {
        internal readonly Formula<T> _pow, _base;
        public ExponentFormula(Formula<T> @base, Formula<T> i)
        {
            this._base = @base;
            this._pow = i;
        }
        public override T this[params T[] x0]
        {
            get
            {
                Field<T> f = getField<T>();
                return f.pow(this._base[x0], this._pow[x0]);
            }
        }
        public override Formula<T> derive(int deriveindex = 0)
        {
            return this._pow * (this._base ^ (this._pow - getField<T>().one)) * this._base.derive(deriveindex) + this._pow.derive(deriveindex) * this._base.log() * this;
        }
        public override bool hasValue(params T[] x0)
        {
            return this._pow.hasValue(x0) && this._base.hasValue(x0);
        }
        public override bool Equals(Formula<T> other)
        {
            var sum = other as ExponentFormula<T>;
            return sum != null && (this._pow * this._base.log()).Equals(sum._pow * sum._base.log());
        }
        public override string ToString()
        {
            return "(" + this._base + ")^(" + this._pow + ")";
        }
        public override Funnel<Tuple<Formula<T>, FormulaAggregator<T>>> getOptimiserFunnel()
        {
            Funnel<Tuple<Formula<T>, FormulaAggregator<T>>> powoptimizer = new Funnel<Tuple<Formula<T>, FormulaAggregator<T>>>
            {
                a =>
                {
                    var formula = a.Item1 as ExponentFormula<T>;
                    var field = getField<T>();
                    if (formula._base.Equals(field.zero))
                    {
                        return true;
                    }
                    if (formula._pow.Equals(field.zero) || formula._base.Equals(field.one))
                    {
                        a.Item2.Add(field.one);
                        return true;
                    }
                    if (formula._pow.Equals(field.one))
                    {
                        a.Item2.Add(formula._base);
                        return true;
                    }
                    ConstantFormula<T> constantFormula = formula._pow as ConstantFormula<T>;
                    if (constantFormula != null && field.isNegative(constantFormula._val))
                    {
                        a.Item2.Add(field.one/(formula._base ^ field.Negate(constantFormula._val)));
                        return true;
                    }
                    return false;
                },
                a =>
                {
                    var formula = a.Item1 as ExponentFormula<T>;
                    var field = getField<T>();
                    if (formula._base is ConstantFormula<T> && formula._pow is ConstantFormula<T>)
                    {
                        a.Item2.Add(a.Item1[field.zero]);
                        return true;
                    }
                    return false;
                }
            };
            //constant pow handler
            //0^f => 0
            //f^0,1^f=>1
            //f^1=>f
            //f1^-f2=>1/(f1^f2)
            //c1^c2=>c3
            return powoptimizer;
        }
        public override Formula<T> optimisedInternals()
        {
            return _base.Optimise() ^ _pow.Optimise();
        }
    }
    public class SineFormula : Formula<double>
    {
        internal readonly Formula<double> _int;
        public SineFormula(Formula<double> i)
        {
            this._int = i;
        }
        public override double this[params double[] x0]
        {
            get
            {
                return Math.Sin(this._int[x0]);
            }
        }
        public override Formula<double> derive(int deriveindex = 0)
        {
            return new CosineFormula(this._int) * this._int.derive(deriveindex);
        }
        public override bool hasValue(params double[] x0)
        {
            return this._int.hasValue(x0);
        }
        public override bool Equals(Formula<double> other)
        {
            var sum = other as SineFormula;
            return sum != null && this._int.Equals(sum._int);
        }
        public override string ToString()
        {
            return "sin(" + this._int + ")";
        }
        public override Funnel<Tuple<Formula<double>, FormulaAggregator<double>>> getOptimiserFunnel()
        {
            Funnel<Tuple<Formula<double>, FormulaAggregator<double>>> ret = new Funnel<Tuple<Formula<double>, FormulaAggregator<double>>>
            {
                a =>
                {
                    var formula = a.Item1 as SineFormula;
                    var field = getField<double>();
                    if (formula._int is ConstantFormula<double>)
                    {
                        a.Item2.Add(a.Item1[field.zero]);
                        return true;
                    }
                    return false;
                }
            };
            return ret;
        }
        public override Formula<double> optimisedInternals()
        {
            return new SineFormula(_int.Optimise());
        }
    }
    public class CosineFormula : Formula<double>
    {
        internal readonly Formula<double> _int;
        public CosineFormula(Formula<double> i)
        {
            this._int = i;
        }
        public override double this[params double[] x0]
        {
            get
            {
                return Math.Cos(this._int[x0]);
            }
        }
        public override Formula<double> derive(int deriveindex = 0)
        {
            return new SineFormula(this._int) * this._int.derive(deriveindex);
        }
        public override bool hasValue(params double[] x0)
        {
            return this._int.hasValue(x0);
        }
        public override bool Equals(Formula<double> other)
        {
            var sum = other as CosineFormula;
            return sum != null && this._int.Equals(sum._int);
        }
        public override string ToString()
        {
            return "cos(" + this._int + ")";
        }
        public override Funnel<Tuple<Formula<double>, FormulaAggregator<double>>> getOptimiserFunnel()
        {
            Funnel<Tuple<Formula<double>, FormulaAggregator<double>>> ret = new Funnel<Tuple<Formula<double>, FormulaAggregator<double>>>
            {
                a =>
                {
                    var formula = a.Item1 as CosineFormula;
                    var field = getField<double>();
                    if (formula._int is ConstantFormula<double>)
                    {
                        a.Item2.Add(a.Item1[field.zero]);
                        return true;
                    }
                    return false;
                }
            };
            return ret;
        }
        public override Formula<double> optimisedInternals()
        {
            return new CosineFormula(_int.Optimise());
        }
    }
    public class TangentFormula : QuotientFormula<double>
    {
        public TangentFormula(Formula<double> i) : base(new SineFormula(i), new CosineFormula(i)) { }
    }
    public class ComplexSineFormula : Formula<ComplexNumber>
    {
        internal readonly Formula<ComplexNumber> _int;
        public ComplexSineFormula(Formula<ComplexNumber> i)
        {
            this._int = i;
        }
        public override ComplexNumber this[params ComplexNumber[] x0]
        {
            get
            {
                return this._int[x0].Sin();
            }
        }
        public override Formula<ComplexNumber> derive(int deriveindex = 0)
        {
            return new ComplexCosineFormula(this._int) * this._int.derive(deriveindex);
        }
        public override bool hasValue(params ComplexNumber[] x0)
        {
            return this._int.hasValue(x0);
        }
        public override bool Equals(Formula<ComplexNumber> other)
        {
            var sum = other as ComplexSineFormula;
            return sum != null && this._int.Equals(sum._int);
        }
        public override string ToString()
        {
            return "sin(" + this._int + ")";
        }
        public override Funnel<Tuple<Formula<ComplexNumber>, FormulaAggregator<ComplexNumber>>> getOptimiserFunnel()
        {
            Funnel<Tuple<Formula<ComplexNumber>, FormulaAggregator<ComplexNumber>>> ret = new Funnel
                <Tuple<Formula<ComplexNumber>, FormulaAggregator<ComplexNumber>>>
            {
                a =>
                {
                    var formula = a.Item1 as ComplexSineFormula;
                    var field = getField<ComplexNumber>();
                    if (formula._int is ConstantFormula<ComplexNumber>)
                    {
                        a.Item2.Add(a.Item1[field.zero]);
                        return true;
                    }
                    return false;
                }
            };
            return ret;
        }
        public override Formula<ComplexNumber> optimisedInternals()
        {
            return new ComplexSineFormula(_int.Optimise());
        }
    }
    public class ComplexCosineFormula : Formula<ComplexNumber>
    {
        internal readonly Formula<ComplexNumber> _int;
        public ComplexCosineFormula(Formula<ComplexNumber> i)
        {
            this._int = i;
        }
        public override ComplexNumber this[params ComplexNumber[] x0]
        {
            get
            {
                return this._int[x0].Cos();
            }
        }
        public override Formula<ComplexNumber> derive(int deriveindex = 0)
        {
            return new ComplexSineFormula(this._int) * this._int.derive(deriveindex);
        }
        public override bool hasValue(params ComplexNumber[] x0)
        {
            return this._int.hasValue(x0);
        }
        public override bool Equals(Formula<ComplexNumber> other)
        {
            var sum = other as ComplexCosineFormula;
            return sum != null && this._int.Equals(sum._int);
        }
        public override string ToString()
        {
            return "cos(" + this._int + ")";
        }
        public override Funnel<Tuple<Formula<ComplexNumber>, FormulaAggregator<ComplexNumber>>> getOptimiserFunnel()
        {
            Funnel<Tuple<Formula<ComplexNumber>, FormulaAggregator<ComplexNumber>>> ret = new Funnel
                <Tuple<Formula<ComplexNumber>, FormulaAggregator<ComplexNumber>>>
            {
                a =>
                {
                    var formula = a.Item1 as ComplexCosineFormula;
                    var field = getField<ComplexNumber>();
                    if (formula._int is ConstantFormula<ComplexNumber>)
                    {
                        a.Item2.Add(a.Item1[field.zero]);
                        return true;
                    }
                    return false;
                }
            };
            return ret;
        }
        public override Formula<ComplexNumber> optimisedInternals()
        {
            return new ComplexCosineFormula(_int.Optimise());
        }
    }
    public class ComplexTangentFormula : QuotientFormula<ComplexNumber>
    {
        internal ComplexTangentFormula(Formula<ComplexNumber> i) : base(new ComplexSineFormula(i), new ComplexCosineFormula(i)) { }
    }
    public class ArcSineFormula : Formula<double>
    {
        internal readonly Formula<double> _int;
        public ArcSineFormula(Formula<double> i)
        {
            this._int = i;
        }
        public override double this[params double[] x0]
        {
            get
            {
                return Math.Asin(this._int[x0]);
            }
        }
        public override Formula<double> derive(int deriveindex = 0)
        {
            return this._int.derive(deriveindex) / ((1 - this._int ^ 2) ^ 0.5);
        }
        public override bool hasValue(params double[] x0)
        {
            return this._int.hasValue(x0) && this._int[x0].iswithin(-1, 1);
        }
        public override bool Equals(Formula<double> other)
        {
            var sum = other as ArcSineFormula;
            return sum != null && this._int.Equals(sum._int);
        }
        public override string ToString()
        {
            return "asin(" + this._int + ")";
        }
        public override Funnel<Tuple<Formula<double>, FormulaAggregator<double>>> getOptimiserFunnel()
        {
            Funnel<Tuple<Formula<double>, FormulaAggregator<double>>> ret = new Funnel<Tuple<Formula<double>, FormulaAggregator<double>>>
            {
                a =>
                {
                    var formula = a.Item1 as ArcSineFormula;
                    var field = getField<double>();
                    if (formula._int is ConstantFormula<double>)
                    {
                        a.Item2.Add(a.Item1[field.zero]);
                        return true;
                    }
                    return false;
                }
            };
            return ret;
        }
        public override Formula<double> optimisedInternals()
        {
            return new ArcSineFormula(_int.Optimise());
        }
    }
    public class ArcCosineFormula : Formula<double>
    {
        internal readonly Formula<double> _int;
        public ArcCosineFormula(Formula<double> i)
        {
            this._int = i;
        }
        public override double this[params double[] x0]
        {
            get
            {
                return Math.Acos(this._int[x0]);
            }
        }
        public override Formula<double> derive(int deriveindex = 0)
        {
            return -(this._int.derive(deriveindex) / ((1 - this._int ^ 2) ^ 0.5));
        }
        public override bool hasValue(params double[] x0)
        {
            return this._int.hasValue(x0) && this._int[x0].iswithin(-1, 1);
        }
        public override bool Equals(Formula<double> other)
        {
            var sum = other as ArcCosineFormula;
            return sum != null && this._int.Equals(sum._int);
        }
        public override string ToString()
        {
            return "acos(" + this._int + ")";
        }
        public override Funnel<Tuple<Formula<double>, FormulaAggregator<double>>> getOptimiserFunnel()
        {
            Funnel<Tuple<Formula<double>, FormulaAggregator<double>>> ret = new Funnel<Tuple<Formula<double>, FormulaAggregator<double>>>
            {
                a =>
                {
                    var formula = a.Item1 as ArcCosineFormula;
                    var field = getField<double>();
                    if (formula._int is ConstantFormula<double>)
                    {
                        a.Item2.Add(a.Item1[field.zero]);
                        return true;
                    }
                    return false;
                }
            };
            return ret;
        }
        public override Formula<double> optimisedInternals()
        {
            return new ArcCosineFormula(_int.Optimise());
        }
    }
    public class ArcTangentFormula : Formula<double>
    {
        internal readonly Formula<double> _int;
        public ArcTangentFormula(Formula<double> i)
        {
            this._int = i;
        }
        public override double this[params double[] x0]
        {
            get
            {
                return Math.Atan(this._int[x0]);
            }
        }
        public override Formula<double> derive(int deriveindex = 0)
        {
            return this._int.derive(deriveindex) / (1 + this._int ^ 2);
        }
        public override bool hasValue(params double[] x0)
        {
            return this._int.hasValue(x0);
        }
        public override bool Equals(Formula<double> other)
        {
            var sum = other as ArcTangentFormula;
            return sum != null && this._int.Equals(sum._int);
        }
        public override string ToString()
        {
            return "atan(" + this._int + ")";
        }
        public override Funnel<Tuple<Formula<double>, FormulaAggregator<double>>> getOptimiserFunnel()
        {
            Funnel<Tuple<Formula<double>, FormulaAggregator<double>>> ret = new Funnel<Tuple<Formula<double>, FormulaAggregator<double>>>
            {
                a =>
                {
                    var formula = a.Item1 as ArcTangentFormula;
                    var field = getField<double>();
                    if (formula._int is ConstantFormula<double>)
                    {
                        a.Item2.Add(a.Item1[field.zero]);
                        return true;
                    }
                    return false;
                }
            };
            return ret;
        }
        public override Formula<double> optimisedInternals()
        {
            return new ArcTangentFormula(_int.Optimise());
        }
    }
    public class ComplexArcSineFormula : Formula<ComplexNumber>
    {
        internal readonly Formula<ComplexNumber> _int;
        public ComplexArcSineFormula(Formula<ComplexNumber> i)
        {
            this._int = i;
        }
        public override ComplexNumber this[params ComplexNumber[] x0]
        {
            get
            {
                return this._int[x0].Asin();
            }
        }
        public override Formula<ComplexNumber> derive(int deriveindex = 0)
        {
            return this._int.derive(deriveindex) / (((ComplexNumber)1 - this._int ^ 2) ^ 0.5);
        }
        public override bool hasValue(params ComplexNumber[] x0)
        {
            return this._int.hasValue(x0) && this._int[x0].RealPart.iswithin(-1, 1);
        }
        public override bool Equals(Formula<ComplexNumber> other)
        {
            var sum = other as ComplexArcSineFormula;
            return sum != null && this._int.Equals(sum._int);
        }
        public override string ToString()
        {
            return "asin(" + this._int + ")";
        }
        public override Funnel<Tuple<Formula<ComplexNumber>, FormulaAggregator<ComplexNumber>>> getOptimiserFunnel()
        {
            Funnel<Tuple<Formula<ComplexNumber>, FormulaAggregator<ComplexNumber>>> ret = new Funnel
                <Tuple<Formula<ComplexNumber>, FormulaAggregator<ComplexNumber>>>
            {
                a =>
                {
                    var formula = a.Item1 as ComplexArcSineFormula;
                    var field = getField<ComplexNumber>();
                    if (formula._int is ConstantFormula<ComplexNumber>)
                    {
                        a.Item2.Add(a.Item1[field.zero]);
                        return true;
                    }
                    return false;
                }
            };
            return ret;
        }
        public override Formula<ComplexNumber> optimisedInternals()
        {
            return new ComplexArcSineFormula(_int.Optimise());
        }
    }
    public class ComplexArcCosineFormula : Formula<ComplexNumber>
    {
        internal readonly Formula<ComplexNumber> _int;
        public ComplexArcCosineFormula(Formula<ComplexNumber> i)
        {
            this._int = i;
        }
        public override ComplexNumber this[params ComplexNumber[] x0]
        {
            get
            {
                return this._int[x0].Acos();
            }
        }
        public override Formula<ComplexNumber> derive(int deriveindex = 0)
        {
            return -(this._int.derive(deriveindex) / (((ComplexNumber)1 - this._int ^ 2) ^ 0.5));
        }
        public override bool hasValue(params ComplexNumber[] x0)
        {
            return this._int.hasValue(x0) && this._int[x0].RealPart.iswithin(-1, 1);
        }
        public override bool Equals(Formula<ComplexNumber> other)
        {
            var sum = other as ComplexArcCosineFormula;
            return sum != null && this._int.Equals(sum._int);
        }
        public override string ToString()
        {
            return "acos(" + this._int + ")";
        }
        public override Funnel<Tuple<Formula<ComplexNumber>, FormulaAggregator<ComplexNumber>>> getOptimiserFunnel()
        {
            Funnel<Tuple<Formula<ComplexNumber>, FormulaAggregator<ComplexNumber>>> ret = new Funnel
                <Tuple<Formula<ComplexNumber>, FormulaAggregator<ComplexNumber>>>
            {
                a =>
                {
                    var formula = a.Item1 as ComplexArcCosineFormula;
                    var field = getField<ComplexNumber>();
                    if (formula._int is ConstantFormula<ComplexNumber>)
                    {
                        a.Item2.Add(a.Item1[field.zero]);
                        return true;
                    }
                    return false;
                }
            };
            return ret;
        }
        public override Formula<ComplexNumber> optimisedInternals()
        {
            return new ComplexArcCosineFormula(_int.Optimise());
        }
    }
    public class ComplexArcTangentFormula : Formula<ComplexNumber>
    {
        internal readonly Formula<ComplexNumber> _int;
        public ComplexArcTangentFormula(Formula<ComplexNumber> i)
        {
            this._int = i;
        }
        public override ComplexNumber this[params ComplexNumber[] x0]
        {
            get
            {
                return this._int[x0].Atan();
            }
        }
        public override Formula<ComplexNumber> derive(int deriveindex = 0)
        {
            return this._int.derive(deriveindex) / ((ComplexNumber)1 + this._int ^ (ComplexNumber)2);
        }
        public override bool hasValue(params ComplexNumber[] x0)
        {
            return this._int.hasValue(x0);
        }
        public override bool Equals(Formula<ComplexNumber> other)
        {
            var sum = other as ComplexArcTangentFormula;
            return sum != null && this._int.Equals(sum._int);
        }
        public override string ToString()
        {
            return "atan(" + this._int + ")";
        }
        public override Funnel<Tuple<Formula<ComplexNumber>, FormulaAggregator<ComplexNumber>>> getOptimiserFunnel()
        {
            Funnel<Tuple<Formula<ComplexNumber>, FormulaAggregator<ComplexNumber>>> ret = new Funnel
                <Tuple<Formula<ComplexNumber>, FormulaAggregator<ComplexNumber>>>
            {
                a =>
                {
                    var formula = a.Item1 as ComplexArcTangentFormula;
                    var field = getField<ComplexNumber>();
                    if (formula._int is ConstantFormula<ComplexNumber>)
                    {
                        a.Item2.Add(a.Item1[field.zero]);
                        return true;
                    }
                    return false;
                }
            };
            return ret;
        }
        public override Formula<ComplexNumber> optimisedInternals()
        {
            return new ComplexArcTangentFormula(_int.Optimise());
        }
    }
    public class AbsFormula<T> : Formula<T>
    {
        internal readonly Formula<T> _int;
        public AbsFormula(Formula<T> i)
        {
            this._int = i;
        }
        public override T this[params T[] x0]
        {
            get
            {
                return getField<T>().abs(this._int[x0]);
            }
        }
        public override Formula<T> derive(int deriveindex = 0)
        {
            return (this._int * this._int.derive(deriveindex)) / this;
        }
        public override bool hasValue(params T[] x0)
        {
            return this._int.hasValue(x0);
        }
        public override bool Equals(Formula<T> other)
        {
            var sum = other as AbsFormula<T>;
            return sum != null && this._int.Equals(sum._int);
        }
        public override string ToString()
        {
            return "|" + this._int + "|";
        }
        public override Formula<T> optimisedInternals()
        {
            return new AbsFormula<T>(_int.Optimise());
        }
        public override Funnel<Tuple<Formula<T>, FormulaAggregator<T>>> getOptimiserFunnel()
        {
            Funnel<Tuple<Formula<T>, FormulaAggregator<T>>> ret = new Funnel<Tuple<Formula<T>, FormulaAggregator<T>>>
            {
                a =>
                {
                    var formula = a.Item1 as AbsFormula<T>;
                    var field = getField<T>();
                    if (formula._int is ConstantFormula<T>)
                    {
                        a.Item2.Add(a.Item1[field.zero]);
                        return true;
                    }
                    return false;
                }
            };
            //log(c)=>(log(c))
            return ret;
        }
    }
    public class MemoryFormula<T> : Formula<T>
    {
        private readonly Formula<T> _int;
        private readonly IDictionary<T[], T> _memory;
        private readonly IDictionary<T[], bool> _hasvalue;
        public MemoryFormula(Formula<T> i)
        {
            this._int = i;
            _memory = new Dictionary<T[], T>(new EnumerableEqualityCompararer<T>(getField<T>().ToEqualityComparer()));
            _hasvalue = new Dictionary<T[], bool>(new EnumerableEqualityCompararer<T>(getField<T>().ToEqualityComparer()));
        }
        public override T this[params T[] x0]
        {
            get
            {
                if (_memory.ContainsKey(x0))
                    return _memory[x0];
                return (_memory[x0] = _int[x0]);
            }
        }
        public override Formula<T> derive(int deriveindex = 0)
        {
            return new MemoryFormula<T>(_int.derive(deriveindex));
        }
        public override bool hasValue(params T[] x0)
        {
            if (_hasvalue.ContainsKey(x0))
                return _hasvalue[x0];
            return (_hasvalue[x0] = _int.hasValue(x0));
        }
        public override bool Equals(Formula<T> other)
        {
            var m = other as MemoryFormula<T>;
            return (m?._int.Equals(this._int) ?? false) || _int.Equals(other);
        }
        public override string ToString()
        {
            return _int.ToString();
        }
    }
    
}
