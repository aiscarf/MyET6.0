using System;

namespace ET
{
    public interface IStepFrame
    {
        Type GetGenericType();
        void Bind(Entity component);
        void OnStepFrame();
    }
}