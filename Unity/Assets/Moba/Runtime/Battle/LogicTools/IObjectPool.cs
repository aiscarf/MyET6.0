public interface IObjectPool
{
    IPoolable GetPoolable(object[] param = null);

    void SavePoolable(IPoolable obj);

    void Clear();
}