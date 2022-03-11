namespace ET
{
    [UIEventTag(UIType.UIBattleLoading)]
    public class UIBattleLoadingEvent : UIEvent<UIBattleLoadingComponent>
    {
        public override async ETTask PreOpen(object args)
        {
            BattleMgr.GetBattleViewDataComponent().LoadingProgressProxy.BindProxy(self.ProgressProxy);
            await UIManager.Instance.OpenUI(ViewUI.Name);
        }

        public override async ETTask PreClose()
        {
            BattleMgr.GetBattleViewDataComponent().LoadingProgressProxy.UnBindProxy(self.ProgressProxy);
            await UIManager.Instance.DestroyUI(ViewUI.Name);
        }
    }
}