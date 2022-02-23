using System;

namespace ET
{
    [MediatorTag]
    public abstract class UIMediator<T> : IMediator where T : Entity
    {
        public UI ViewUI { get; set; }
        public T self { get; set; }
        public ReferenceCollector referenceCollector { get; set; }

        public Type GetGenericType()
        {
            return typeof(T);
        }

        public void Bind(object component)
        {
            self = (T)component;
            this.OnAutoBind();
        }

        public abstract void OnAutoBind();
        public abstract void OnInit();
        public abstract void OnDestroy();
        public abstract void OnOpen();
        public abstract void OnClose();
        public abstract void OnBeCover();
        public abstract void OnUnCover();
    }

    public interface IMediator
    {
        UI ViewUI { get; set; }
        ReferenceCollector referenceCollector { get; set; }
        Type GetGenericType();
        void Bind(object component);
        void OnInit();
        void OnDestroy();
        void OnOpen();
        void OnClose();
        void OnBeCover();
        void OnUnCover();
    }
}