using NumberStone;
using Numerics;
using WhetStone.Looping;

namespace WhetStone.NumbersMagic
{
    public class BinomialCoefficient
    {
        public int super { get; private set; }
        public int sub { get; private set; }
        public int value => (int)_val.toNum();
        public BigRational fraction => _val.toFraction();
        private readonly BigProduct _val = new BigProduct();
        public BinomialCoefficient(int super, int sub)
        {
            this.super = super;
            this.sub = sub;
            init();
        }
        private void init()
        {
            _val.MultiplyFactorial(super);
            _val.DivideFactorial(sub);
            _val.DivideFactorial(super-sub);
        }
        public void DecreaseSuper(int div = 1)
        {
            foreach (int i in range.Range(div))
            {
                _val.Multiply(super-sub-i);
                _val.Divide(super-i);
            }
            super-=div;
        }
        public void IncreaseSuper(int div = 1)
        {
            foreach (int i in range.IRange(1,div))
            {
                _val.Multiply(super + i);
                _val.Divide(super - sub + i);
            }
            super+=div;
        }
        public void DecreaseSub(int div = 1)
        {
            foreach (int i in range.Range(div))
            {
                _val.Multiply(sub - i);
            }
            foreach (int i in range.IRange(div,1,-1))
            {
                _val.Divide(super - sub + i);
            }
            sub -= div;
        }
        public void IncreaseSub(int div = 1)
        {
            foreach (int i in range.IRange(1,div))
            {
                _val.Divide(sub + i);
            }
            foreach (int i in range.Range(div))
            {
                _val.Multiply(super - sub - i);
            }
            sub += div;
        }
    }
}
