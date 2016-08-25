using System.Collections.Generic;

namespace WhetStone.Funnels
{
    public interface IFunnel<in PT, RT> : IEnumerable<Proccesor<PT, RT>>
    {
        RT Process(PT val);
    }
    public interface IFunnel<in PT> : IEnumerable<Proccesor<PT>>
    {
        void Process(PT val);
    }
    public interface IFunnel : IEnumerable<Proccesor>
    {
        void Process();
    }
}