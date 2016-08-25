namespace WhetStone.Factory
{
    public interface ICreator<in H, in G, in F, out T>
    {
        T Create(H arg0, G arg1, F arg2);
    }
    public interface ICreator<in H, in G, out T>
    {
        T Create(H arg0, G arg1);
    }
    public interface ICreator<in G, out T>
    {
        T Create(G arg);
    }
    public interface ICreator<out T>
    {
        T Create();
    }
}
