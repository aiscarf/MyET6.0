using System;
using System.Collections.Generic;

namespace Scarf.Moba
{
    public class CObject: IDisposable
    {
        public Battle Battle { get; set; }
        private List<CComponent> m_lstComponents = new List<CComponent>();
        private Dictionary<Type, CComponent> m_components = new Dictionary<Type, CComponent>();
        private bool IsDisposed = false;

        public T AddComponent<T>() where T : CComponent
        {
            return (T)AddComponent(typeof (T));
        }

        public CComponent AddComponent(Type type)
        {
            if (this.m_components.ContainsKey(type))
            {
                throw new Exception($"EObject.AddComponent ERROR: {type.ToString()}");
            }

            var component = Activator.CreateInstance(type) as CComponent;
            component.Battle = this.Battle;
            component.Parent = this;
            this.m_components.Add(type, component);
            this.m_lstComponents.Add(component);
            return component;
        }

        public T GetComponent<T>() where T : CComponent
        {
            return (T)GetComponent(typeof (T));
        }

        public CComponent GetComponent(Type type)
        {
            if (!this.m_components.TryGetValue(type, out var result))
            {
                throw new Exception($"EObject.RemoveComponent ERROR: {type.ToString()}");
            }

            return result;
        }

        public void RemoveComponent<T>() where T : CComponent
        {
            var type = typeof (T);
            RemoveComponent(type);
        }

        public void RemoveComponent(Type type)
        {
            if (!this.m_components.TryGetValue(type, out var component))
            {
                throw new Exception($"EObject.RemoveComponent ERROR: {type.ToString()}");
            }

            component.Battle = null;
            component.Parent = null;
            component.Dispose();
            this.m_components.Remove(type);
            this.m_lstComponents.Remove(component);
        }

        public virtual void OnFrameSyncUpdate(int delta)
        {
            foreach (var component in this.m_lstComponents)
            {
                component.OnFrameSyncUpdate(delta);
            }
        }

        public void Init()
        {
            OnInit();
            foreach (var component in this.m_lstComponents)
            {
                component.Init();
            }
        }

        public void Start()
        {
            OnStart();
            foreach (var component in this.m_lstComponents)
            {
                component.Start();
            }
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;
            IsDisposed = true;
            foreach (var component in this.m_lstComponents)
            {
                component.Dispose();
            }

            m_components.Clear();
            OnDestroy();
        }

        protected virtual void OnInit()
        {
        }

        protected virtual void OnStart()
        {
        }

        protected virtual void OnDestroy()
        {
        }
    }
}