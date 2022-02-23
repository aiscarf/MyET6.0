namespace ET
{
    public abstract class UIEvent : IUIEvent
    {
        public string Name { get; set; }

        public virtual async ETTask PreOpen()
        {
            await UIManager.Instance.OpenUI(Name);
        }

        public virtual async ETTask PreClose()
        {
            await UIManager.Instance.CloseUI(Name);
        }
    }

    public interface IUIEvent
    {
        string Name { get; set; }
        ETTask PreOpen();

        ETTask PreClose();
    }
}