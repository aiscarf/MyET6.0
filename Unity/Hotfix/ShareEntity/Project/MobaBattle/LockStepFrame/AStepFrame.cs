using System;

namespace ET
{
    [StepFrame]
    public abstract class AStepFrame<T> : IStepFrame where T : Entity
    {
        public T self;

        public Type GetGenericType()
        {
            return typeof(T);
        }

        public void Bind(Entity component)
        {
            self = (T)component;
        }

        public abstract void OnStepFrame();
    }
}