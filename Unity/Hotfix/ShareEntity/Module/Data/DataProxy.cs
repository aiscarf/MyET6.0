using System;
using System.Collections.Generic;

namespace ET
{
    public class DataProxy<T> : IDisposable
    {
        private T m_value;
        private List<Action<T>> m_listeners = new List<Action<T>>();
        private List<DataProxy<T>> m_syncProxy = new List<DataProxy<T>>();
        private bool m_bIterating;

        private List<Action<T>> m_lstAddListeners = new List<Action<T>>();
        private List<Action<T>> m_lstRemoveListners = new List<Action<T>>();

        public DataProxy(T value)
        {
            m_value = value;
        }

        public void Dispose()
        {
            m_value = default(T);
            m_listeners.Clear();
            m_syncProxy.Clear();
            m_bIterating = false;
        }

        public T GetValue()
        {
            return m_value;
        }

        public void SetValue(T value)
        {
            if (m_bIterating)
            {
                Log.Error("正被驱动的服务, 不应逆向操作源头数据!.");
                return;
            }
            
            m_value = value;

            int addCount = m_lstAddListeners.Count;
            for (int i = 0; i < addCount; i++)
            {
                m_listeners.Add(m_lstAddListeners[i]);
            }

            m_lstAddListeners.Clear();

            int rmvCount = m_lstRemoveListners.Count;
            for (int i = 0; i < rmvCount; i++)
            {
                m_listeners.Remove(m_lstRemoveListners[i]);
            }

            m_lstRemoveListners.Clear();

            m_bIterating = true;
            int count = m_listeners.Count;
            for (int i = 0; i < count; i++)
            {
                try
                {
                    m_listeners[i].Invoke(value);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }

            int proxyCount = m_syncProxy.Count;
            for (int i = 0; i < proxyCount; i++)
            {
                try
                {
                    m_syncProxy[i].SetValue(value);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }

            m_bIterating = false;
        }

        public void AddListener(Action<T> action)
        {
            if (m_bIterating)
            {
                m_lstAddListeners.Add(action);
                return;
            }

            m_listeners.Add(action);
        }

        public void RemoveListener(Action<T> action)
        {
            if (m_bIterating)
            {
                m_lstRemoveListners.Remove(action);
                return;
            }

            m_listeners.Remove(action);
        }

        public void BindProxy(DataProxy<T> proxy)
        {
            m_syncProxy.Add(proxy);

            int proxyCount = m_syncProxy.Count;
            for (int i = 0; i < proxyCount; i++)
            {
                try
                {
                    m_syncProxy[i].SetValue(m_value);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public void UnBindProxy(DataProxy<T> proxy)
        {
            m_syncProxy.Remove(proxy);
        }
    }
}