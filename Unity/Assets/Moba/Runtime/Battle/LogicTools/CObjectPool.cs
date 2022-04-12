using System;
using System.Collections.Generic;

public class CObjectPool<T> : Singleton<CObjectPool<T>>, IObjectPool where T : IPoolable
{
    private Queue<T> _pool;
    private int _capicity;
    private bool _inited;

    public void Init(int capicity)
    {
        if (this._inited)
            return;
        this._capicity = capicity;
        this._pool = new Queue<T>(this._capicity);
        this._inited = true;
    }

    public T GetObject(object[] param = null)
    {
        return this._pool.Count <= 0
            ? (T) Activator.CreateInstance(typeof(T))
            : this._pool.Dequeue();
    }

    public void SaveObject(T obj)
    {
        obj.Reset();
        if (this._pool.Count >= this._capicity)
            return;
        this._pool.Enqueue(obj);
    }

    public IPoolable GetPoolable(object[] param = null)
    {
        return (IPoolable) this.GetObject(param);
    }

    public void SavePoolable(IPoolable obj)
    {
        this.SaveObject((T) obj);
    }

    public void Clear()
    {
        this._inited = false;
        this._pool.Clear();
    }
}