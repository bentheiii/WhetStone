using System;
using System.Diagnostics;
using WhetStone.Looping;
using WhetStone.SystemExtensions;

namespace NumberStone
{
    /// <summary>
    /// Stores a binomial coefficient in a format that allows for easily incrementing and decrementing the super and sub of the coefficient.
    /// </summary>
    /// <remarks><para>For convenience, the first argument in the coefficient is called the super, and the second is called the sub.</para></remarks>
    public class BinomialCoefficient
    {
        /// <summary>
        /// Get or Set the super of the coefficient
        /// </summary>
        /// <remarks>It is recommended not to change the super by a large value. If you are considering this, instead consider using the <see cref="choose.Choose"/> function.</remarks>
        public int super
        {
            get
            {
                return _super;
            }
            set
            {
                var change = value - _super;
                if (change == 0)
                    return;
                if (change > 0)
                    this.IncreaseSuper(change);
                else
                    this.DecreaseSuper(-change);
            }
        }
        /// <summary>
        /// Get or Set the sub of the coefficient
        /// </summary>
        /// <remarks>It is recommended not to change the sub by a large value. If you are considering this, instead consider using the <see cref="choose.Choose"/> function.</remarks>
        /// <exception cref="InvalidOperationException">If trying to bring the coefficient to a state where sub is higher than super.</exception>
        public int sub
        {
            get
            {
                return _sub;
            }
            set
            {
                var change = value - _sub;
                if (change == 0)
                    return;
                if (change > 0)
                    this.IncreaseSub(change);
                else
                    this.DecreaseSub(-change);
            }
        }
        /// <summary>
        /// gets the coefficient's value.
        /// </summary>
        public int value => _val.toNum();
        private readonly BigProduct _val = new BigProduct();
        private int _super;
        private int _sub;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="super">The initial super of the coefficient</param>
        /// <param name="sub">The initial sub of the coefficient</param>
        /// <exception cref="ArgumentException">If super is not as high as sub, or either being lower than zero.</exception>
        public BinomialCoefficient(int super, int sub)
        {
            if (sub > super || sub < 0)
                throw new ArgumentException("Super must be at least as high as sub ,and sub must be higher than or equal to 0.");
            _super = super;
            _sub = sub;
            init();
        }
        private void init()
        {
            _val.MultiplyFactorial(super);
            _val.DivideFactorial(sub);
            _val.DivideFactorial(super-sub);
        }
        /// <summary>
        /// Decrements the super.
        /// </summary>
        /// <param name="div">The amount to decrease super by.</param>
        /// <exception cref="InvalidOperationException">If attempt to decrease super to below sub.</exception>
        public void DecreaseSuper(int div = 1)
        {
            div.ThrowIfAbsurd(nameof(div));
            if (super-div < sub)
                throw new InvalidOperationException("cannot bring BinomialCoefficient to desired state.");
            foreach (int i in range.Range(div))
            {
                _val.Multiply(super-sub-i);
                _val.Divide(super-i);
            }
            _super-=div;
        }
        /// <summary>
        /// Increase the super.
        /// </summary>
        /// <param name="div">The amount to increase super by.</param>
        public void IncreaseSuper(int div = 1)
        {
            div.ThrowIfAbsurd(nameof(div));
            foreach (int i in range.IRange(1,div))
            {
                _val.Multiply(super + i);
                _val.Divide(super - sub + i);
            }
            _super+=div;
        }
        /// <summary>
        /// Decrements the sub.
        /// </summary>
        /// <param name="div">The amount to decrease sub by.</param>
        /// <exception cref="InvalidOperationException">If attempt to decrease sub to below 0.</exception>
        public void DecreaseSub(int div = 1)
        {
            div.ThrowIfAbsurd(nameof(div));
            if (div > sub)
                throw new InvalidOperationException("cannot bring BinomialCoefficient to desired state.");
            foreach (int i in range.Range(div))
            {
                _val.Multiply(sub - i);
            }
            foreach (int i in range.IRange(div,1,-1))
            {
                _val.Divide(super - sub + i);
            }
            _sub -= div;
        }
        /// <summary>
        /// Increments the sub.
        /// </summary>
        /// <param name="div">The amount to increase sub by.</param>
        /// <exception cref="InvalidOperationException">If attempt to increase sub to above super.</exception>
        public void IncreaseSub(int div = 1)
        {
            div.ThrowIfAbsurd(nameof(div));
            if (sub + div > super)
                throw new InvalidOperationException("cannot bring BinomialCoefficient to desired state.");
            foreach (int i in range.IRange(1,div))
            {
                _val.Divide(sub + i);
            }
            foreach (int i in range.Range(div))
            {
                _val.Multiply(super - sub - i);
            }
            _sub += div;
        }
        /// <summary>
        /// Decreases both the super and the sub simultaneously.
        /// </summary>
        /// <param name="div">The amount to decrease both super and sub by.</param>
        /// <exception cref="InvalidOperationException">If attempt to lower sub to below zero.</exception>
        public void DecreaseBoth(int div = 1)
        {
            div.ThrowIfAbsurd(nameof(div));
            if (sub < div)
                throw new InvalidOperationException("cannot bring BinomialCoefficient to desired state.");
            foreach (int i in range.Range(div))
            {
                _val.Multiply(sub-div);
                _val.Divide(super-div);
            }
            _sub -= div;
            _super -= div;
        }
        /// <summary>
        /// Increases both the super and the sub simultaneously.
        /// </summary>
        /// <param name="div">The amount to increase both super and sub by.</param>
        public void IncreaseBoth(int div = 1)
        {
            div.ThrowIfAbsurd(nameof(div));
            foreach (int i in range.Range(div))
            {
                _val.Divide(sub - div);
                _val.Multiply(super - div);
            }
            _sub += div;
            _super += div;
        }
    }
}
