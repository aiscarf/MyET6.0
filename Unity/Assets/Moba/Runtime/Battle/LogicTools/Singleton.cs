public class Singleton<T> where T : new()
{
    private static readonly object m_cLock = new object();
    private static T m_cInstance;

    protected Singleton()
    {
    }

    [global::System.Diagnostics.DebuggerHidden]
    public static T instance
    {
        get
        {
            if ((object) Singleton<T>.m_cInstance == null)
            {
                lock (Singleton<T>.m_cLock)
                {
                    if ((object) Singleton<T>.m_cInstance == null)
                        Singleton<T>.m_cInstance = new T();
                }
            }
            return Singleton<T>.m_cInstance;
        }
    }
}