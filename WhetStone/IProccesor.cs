namespace WhetStone.Funnels
{
    public interface IProccesor<in PT, RT>
    {
        Proccesor<PT, RT> toProcessor();
    }
    public interface IProccesor<in PT>
    {
        Proccesor<PT> toProcessor();
    }
    public interface IProccesor
    {
        Proccesor toProcessor();
    }
}