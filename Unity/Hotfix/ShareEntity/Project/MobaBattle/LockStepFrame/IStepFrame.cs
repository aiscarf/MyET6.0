namespace ET
{
    public interface IStepFrame
    {
        void Bind(FrameSyncComponent frameSyncComponent);
        void OnStepFrame(int delta);
    }
}