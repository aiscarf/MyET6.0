namespace ET
{
    [StepFrame]
    public abstract class AStepFrame<T> : IStepFrame where T : Entity
    {
        protected T self;
        public abstract void Bind(FrameSyncComponent frameSyncComponent);

        public abstract void OnStepFrame(int delta);
    }
}