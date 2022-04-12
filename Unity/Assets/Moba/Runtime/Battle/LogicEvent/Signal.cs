using System;

namespace Scarf.Moba
{
    public class Signal
    {
        private Action Listener = delegate { };

        public void AddListener(Action callback)
        {
            this.Listener += callback;
        }

        public void RemoveListener(Action callback)
        {
            this.Listener -= callback;
        }

        public void Dispatch()
        {
            try
            {
                this.Listener.Invoke();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void Clear()
        {
            this.Listener = delegate { };
        }
    }

    public class Signal<T0>
    {
        private Action<T0> Listener = delegate(T0 t0) { };

        public void AddListener(Action<T0> callback)
        {
            this.Listener += callback;
        }

        public void RemoveListener(Action<T0> callback)
        {
            this.Listener -= callback;
        }

        public void Dispatch(T0 t0)
        {
            try
            {
                this.Listener.Invoke(t0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void Clear()
        {
            this.Listener = delegate(T0 t0) { };
        }
    }

    public class Signal<T0, T1>
    {
        private Action<T0, T1> Listener = delegate(T0 t0, T1 t1) { };

        public void AddListener(Action<T0, T1> callback)
        {
            this.Listener += callback;
        }

        public void RemoveListener(Action<T0, T1> callback)
        {
            this.Listener -= callback;
        }

        public void Dispatch(T0 t0, T1 t1)
        {
            try
            {
                this.Listener.Invoke(t0, t1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void Clear()
        {
            this.Listener = delegate(T0 t0, T1 t1) { };
        }
    }

    public class Signal<T0, T1, T2>
    {
        private Action<T0, T1, T2> Listener = delegate(T0 t0, T1 t1, T2 t2) { };

        public void AddListener(Action<T0, T1, T2> callback)
        {
            this.Listener += callback;
        }

        public void RemoveListener(Action<T0, T1, T2> callback)
        {
            this.Listener -= callback;
        }

        public void Dispatch(T0 t0, T1 t1, T2 t2)
        {
            try
            {
                this.Listener.Invoke(t0, t1, t2);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void Clear()
        {
            this.Listener = delegate(T0 t0, T1 t1, T2 t2) { };
        }
    }
}