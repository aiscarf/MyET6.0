namespace ET
{
    [UIEventTag(UIType.UILoginScene)]
    public class UILoginSceneEvent : UIEvent
    {
        public override async ETTask PreOpen()
        {
            await UIManager.Instance.OpenAndCoverAll(UIType.UILoginScene);
        }
    }
}