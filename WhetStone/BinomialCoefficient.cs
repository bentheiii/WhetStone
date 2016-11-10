using NumberStone;

namespace WhetStone.NumbersMagic
{
    public class BinomialCoefficient
    {
        public int super { get; private set; }
        public int sub { get; }
        public int value => (int)_val.toNum();
        private BigProduct _val = new BigProduct();
        public BinomialCoefficient(int super, int sub)
        {
            this.super = super;
            this.sub = sub;
            init();
        }
        private void init()
        {
            _val = new BigProduct();
            _val.MultiplyFactorial(super);
            _val.DivideFactorial(sub);
            _val.DivideFactorial(super-sub);
        }
        public void ReduceSuper()
        {
            _val.Multiply(super-sub);
            _val.Divide(super);
            super--;
        }
    }
}
