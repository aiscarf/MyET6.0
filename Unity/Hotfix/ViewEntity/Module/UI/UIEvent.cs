using System;

namespace ET
{
    public abstract class UIEvent<T> : IUIEvent where T : Entity
    {
        public UI ViewUI { get; private set; }

        public T self { get; private set; }

        public void Bind(UI ui)
        {
            ViewUI = ui;
            self = (T)ui.GetComponent(GetGenericType());
        }

        public Type GetGenericType()
        {
            return typeof(T);
        }

        public virtual async ETTask PreOpen(object args)
        {
            await UIManager.Instance.OpenUI(ViewUI.Name);
        }

        public virtual async ETTask PreClose()
        {
            await UIManager.Instance.CloseUI(ViewUI.Name);
        }
    }

    public interface IUIEvent
    {
        UI ViewUI { get; }
        void Bind(UI ui);
        Type GetGenericType();
        ETTask PreOpen(object args);

        ETTask PreClose();
    }
}