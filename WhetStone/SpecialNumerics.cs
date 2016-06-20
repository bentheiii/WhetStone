using System;
using System.Collections.Generic;
using WhetStone.Fielding;

namespace WhetStone.SpecialNumerics
{
    public abstract class ISpecialVal<T>
    {
        public abstract ISpecialValTemplate<T> Gtemplate { get; }
        public abstract T value { get; }
        public virtual T max
        {
            get
            {
                return Gtemplate.max;
            }
        }
        public virtual T min
        {
            get
            {
                return Gtemplate.min;
            }
        }
        public override string ToString()
        {
            return this.min + "<=" + this.value + "<" + this.max;
        }
    }
    public abstract class ISpecialValTemplate<T>
    {
        protected bool Equals(ISpecialValTemplate<T> other)
        {
            return EqualityComparer<T>.Default.Equals(this.max, other.max) && EqualityComparer<T>.Default.Equals(this.min, other.min);
        }
        /// <summary>
        /// Serves as the default hash function. 
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<T>.Default.GetHashCode(this.max) * 397) ^ EqualityComparer<T>.Default.GetHashCode(this.min);
            }
        }
        protected ISpecialValTemplate(T max, T min)
        {
            this.max = max;
            this.min = min;
        }
        public T max { get; }
        public T min { get; }
        public abstract T quick(T val, out bool valchanged);
        public T quick(T val)
        {
            bool p;
            return quick(val, out p);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return obj.GetType() == this.GetType() && this.Equals((ISpecialValTemplate<T>)obj);
        }
    }
    public class RollerTemplate<T> : ISpecialValTemplate<T>
    {
        public RollerTemplate(T max, T min) : base(max, min) { }
        public override T quick(T val, out bool valchanged)
        {
            var o = val.ToFieldWrapper();
            valchanged = (o >= max || o < min);
            return (o - this.min).TrueMod(this.max.ToFieldWrapper() - this.min) + this.min;
        }
    }
    public class BoundTemplate<T> : ISpecialValTemplate<T>
    {
        public BoundTemplate(T max, T min) : base(max, min) { }
        public override T quick(T val, out bool valchanged)
        {
            var c = val.ToFieldWrapper();
            valchanged = true;
            if (c < min)
                return min;
            if (c > max)
                return max;
            valchanged = false;
            return val;
        }
    }
    public class InflaterTemplate<T> : ISpecialValTemplate<T>
    {
        public InflaterTemplate(T max, T min) : base(max, min) { }
        public override T quick(T val, out bool valchanged)
        {
            valchanged = false;
            return val;
        }
    }
    public class RollerNum<T> : ISpecialVal<T>
    {
        public RollerTemplate<T> template { get; }
        public override ISpecialValTemplate<T> Gtemplate
        {
            get
            {
                return template;
            }
        }
        public override T value { get; }
        /// <summary>
        /// <![CDATA[max is exclusive, min is inclusive, min <= value < max]]>
        /// </summary>
        public RollerNum(T value, T maxvalue, T minvalue)
        {
            if (maxvalue.ToFieldWrapper() <= minvalue)
                throw new ArgumentException("maxvalue is lower than min value");
            this.template = new RollerTemplate<T>(maxvalue, minvalue);
            this.value = this.template.quick(value);
        }
        public RollerNum(T value, RollerTemplate<T> template)
        {
            this.template = template;
            this.value = this.template.quick(value);
        }
        public static RollerNum<T> operator -(RollerNum<T> a)
        {
            return new RollerNum<T>(-a.value.ToFieldWrapper(), a.template);
        }
        public static RollerNum<T> operator +(RollerNum<T> a, RollerNum<T> b)
        {
            if (!a.template.Equals(b.template))
                throw new ArgumentException("the templates don't match");
            return new RollerNum<T>(a.value.ToFieldWrapper() + b.value, a.template);
        }
        public static RollerNum<T> operator -(RollerNum<T> a, RollerNum<T> b)
        {
            if (!a.template.Equals(b.template))
                throw new ArgumentException("the templates don't match");
            return new RollerNum<T>(a.value.ToFieldWrapper() - b.value, a.template);
        }
        public static RollerNum<T> operator *(RollerNum<T> a, RollerNum<T> b)
        {
            if (!a.template.Equals(b.template))
                throw new ArgumentException("the templates don't match");
            return new RollerNum<T>(a.value.ToFieldWrapper() * b.value, a.template);
        }
        public static RollerNum<T> operator /(RollerNum<T> a, RollerNum<T> b)
        {
            if (!a.template.Equals(b.template))
                throw new ArgumentException("the templates don't match");
            return new RollerNum<T>(a.value.ToFieldWrapper() / b.value, a.template);
        }
        public static RollerNum<T> operator ++(RollerNum<T> a)
        {
            return new RollerNum<T>(a.value.ToFieldWrapper() + 1, a.template);
        }
        public static RollerNum<T> operator --(RollerNum<T> a)
        {
            return new RollerNum<T>(a.value.ToFieldWrapper() - 1, a.template);
        }
        public static RollerNum<T> operator *(RollerNum<T> a, T b)
        {
            return new RollerNum<T>(a.value.ToFieldWrapper() * b, a.template);
        }
        public static RollerNum<T> operator +(RollerNum<T> a, T b)
        {
            return new RollerNum<T>(a.value.ToFieldWrapper() + b, a.template);
        }
        public static RollerNum<T> operator -(RollerNum<T> a, T b)
        {
            return new RollerNum<T>(a.value.ToFieldWrapper() - b, a.template);
        }
        public static RollerNum<T> operator /(RollerNum<T> a, T b)
        {
            return new RollerNum<T>(a.value.ToFieldWrapper() / b, a.template);
        }
        public static RollerNum<T> operator *(T b, RollerNum<T> a)
        {
            return a * b;
        }
        public static RollerNum<T> operator +(T b, RollerNum<T> a)
        {
            return a + b;
        }
        public static RollerNum<T> operator -(T b, RollerNum<T> a)
        {
            return a - b;
        }
        public static RollerNum<T> operator /(T b, RollerNum<T> a)
        {
            return a / b;
        }
        public RollerNum<T> pow(T power)
        {
            return new RollerNum<T>(value.ToFieldWrapper().pow(power), this.max, this.min);
        }
        public static implicit operator T(RollerNum<T> a)
        {
            return a.value;
        }
    }
    public class BoundNum<T> : ISpecialVal<T>
    {
        public BoundTemplate<T> template { get; }
        public override ISpecialValTemplate<T> Gtemplate
        {
            get
            {
                return template;
            }
        }
        public override T value { get; }
        /// <summary>
        /// <![CDATA[max is exclusive, min is inclusive, min <= value < max]]>
        /// </summary>
        public BoundNum(T value, T maxvalue, T minvalue)
        {
            if (maxvalue.ToFieldWrapper() <= minvalue)
                throw new ArgumentException("maxvalue is lower than min value");
            this.template = new BoundTemplate<T>(maxvalue, minvalue);
            this.value = template.quick(value);
        }
        public BoundNum(T value, BoundTemplate<T> template)
        {
            this.template = template;
            this.value = template.quick(value);
        }
        public static BoundNum<T> operator -(BoundNum<T> a)
        {
            return new BoundNum<T>(-a.value.ToFieldWrapper(), a.template);
        }
        public static BoundNum<T> operator +(BoundNum<T> a, BoundNum<T> b)
        {
            if (!a.template.Equals(b.template))
                throw new ArgumentException("the templates don't match");
            return new BoundNum<T>(a.value.ToFieldWrapper() + b.value, a.template);
        }
        public static BoundNum<T> operator -(BoundNum<T> a, BoundNum<T> b)
        {
            if (!a.template.Equals(b.template))
                throw new ArgumentException("the templates don't match");
            return new BoundNum<T>(a.value.ToFieldWrapper() - b.value, a.template);
        }
        public static BoundNum<T> operator *(BoundNum<T> a, BoundNum<T> b)
        {
            if (!a.template.Equals(b.template))
                throw new ArgumentException("the templates don't match");
            return new BoundNum<T>(a.value.ToFieldWrapper() * b.value, a.template);
        }
        public static BoundNum<T> operator /(BoundNum<T> a, BoundNum<T> b)
        {
            if (!a.template.Equals(b.template))
                throw new ArgumentException("the templates don't match");
            return new BoundNum<T>(a.value.ToFieldWrapper() / b.value, a.template);
        }
        public static BoundNum<T> operator ++(BoundNum<T> a)
        {
            return new BoundNum<T>(a.value.ToFieldWrapper() + 1, a.template);
        }
        public static BoundNum<T> operator --(BoundNum<T> a)
        {
            return new BoundNum<T>(a.value.ToFieldWrapper() - 1, a.template);
        }
        public static BoundNum<T> operator *(BoundNum<T> a, T b)
        {
            return new BoundNum<T>(a.value.ToFieldWrapper() * b, a.template);
        }
        public static BoundNum<T> operator +(BoundNum<T> a, T b)
        {
            return new BoundNum<T>(a.value.ToFieldWrapper() + b, a.template);
        }
        public static BoundNum<T> operator -(BoundNum<T> a, T b)
        {
            return new BoundNum<T>(a.value.ToFieldWrapper() - b, a.template);
        }
        public static BoundNum<T> operator /(BoundNum<T> a, T b)
        {
            return new BoundNum<T>(a.value.ToFieldWrapper() / b, a.template);
        }
        public static BoundNum<T> operator *(T b, BoundNum<T> a)
        {
            return a * b;
        }
        public static BoundNum<T> operator +(T b, BoundNum<T> a)
        {
            return a + b;
        }
        public static BoundNum<T> operator -(T b, BoundNum<T> a)
        {
            return a - b;
        }
        public static BoundNum<T> operator /(T b, BoundNum<T> a)
        {
            return a / b;
        }
        public BoundNum<T> pow(T power)
        {
            return new BoundNum<T>(value.ToFieldWrapper().pow(power), this.max, this.min);
        }
        public static implicit operator T(BoundNum<T> a)
        {
            return a.value;
        }
    }
    public class InflaterNum<T> : ISpecialVal<T>
    {
        public InflaterTemplate<T> template { get; }
        public override ISpecialValTemplate<T> Gtemplate
        {
            get
            {
                return template;
            }
        }
        public override T value { get; }
        public InflaterNum(T value) : this(value, value, value) { }
        public InflaterNum(T value, T maxvalue, T minvalue)
        {
            if (maxvalue.ToFieldWrapper() < minvalue)
                throw new ArgumentException("maxvalue is lower than min value");
            if (maxvalue.ToFieldWrapper() < value)
                maxvalue = value;
            if (minvalue.ToFieldWrapper() > value)
                minvalue = value;
            this.template = new InflaterTemplate<T>(maxvalue, minvalue);
            this.value = template.quick(value);
        }
        public InflaterNum(T value, ISpecialValTemplate<T> template) : this(value, template.min, template.max) { }
        public static InflaterNum<T> operator -(InflaterNum<T> a)
        {
            return new InflaterNum<T>(-a.value.ToFieldWrapper(), a.template);
        }
        public static InflaterNum<T> operator +(InflaterNum<T> a, InflaterNum<T> b)
        {
            if (!a.template.Equals(b.template))
                throw new ArgumentException("the templates don't match");
            return new InflaterNum<T>(a.value.ToFieldWrapper() + b.value, a.template);
        }
        public static InflaterNum<T> operator -(InflaterNum<T> a, InflaterNum<T> b)
        {
            if (!a.template.Equals(b.template))
                throw new ArgumentException("the templates don't match");
            return new InflaterNum<T>(a.value.ToFieldWrapper() - b.value, a.template);
        }
        public static InflaterNum<T> operator *(InflaterNum<T> a, InflaterNum<T> b)
        {
            if (!a.template.Equals(b.template))
                throw new ArgumentException("the templates don't match");
            return new InflaterNum<T>(a.value.ToFieldWrapper() * b.value, a.template);
        }
        public static InflaterNum<T> operator /(InflaterNum<T> a, InflaterNum<T> b)
        {
            if (!a.template.Equals(b.template))
                throw new ArgumentException("the templates don't match");
            return new InflaterNum<T>(a.value.ToFieldWrapper() / b.value, a.template);
        }
        public static InflaterNum<T> operator ++(InflaterNum<T> a)
        {
            return new InflaterNum<T>(a.value.ToFieldWrapper() + 1, a.template);
        }
        public static InflaterNum<T> operator --(InflaterNum<T> a)
        {
            return new InflaterNum<T>(a.value.ToFieldWrapper() - 1, a.template);
        }
        public static InflaterNum<T> operator *(InflaterNum<T> a, T b)
        {
            return new InflaterNum<T>(a.value.ToFieldWrapper() * b, a.template);
        }
        public static InflaterNum<T> operator +(InflaterNum<T> a, T b)
        {
            return new InflaterNum<T>(a.value.ToFieldWrapper() + b, a.template);
        }
        public static InflaterNum<T> operator -(InflaterNum<T> a, T b)
        {
            return new InflaterNum<T>(a.value.ToFieldWrapper() - b, a.template);
        }
        public static InflaterNum<T> operator /(InflaterNum<T> a, T b)
        {
            return new InflaterNum<T>(a.value.ToFieldWrapper() / b, a.template);
        }
        public static InflaterNum<T> operator *(T b, InflaterNum<T> a)
        {
            return a * b;
        }
        public static InflaterNum<T> operator +(T b, InflaterNum<T> a)
        {
            return a + b;
        }
        public static InflaterNum<T> operator -(T b, InflaterNum<T> a)
        {
            return a - b;
        }
        public static InflaterNum<T> operator /(T b, InflaterNum<T> a)
        {
            return a / b;
        }
        public InflaterNum<T> pow(T power)
        {
            return new InflaterNum<T>(value.ToFieldWrapper().pow(power), this.max, this.min);
        }
        public static implicit operator T(InflaterNum<T> a)
        {
            return a.value;
        }
    }
}
