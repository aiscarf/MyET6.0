namespace ET
{
    public partial class UIBattleLoadingMediator : UIMediator<UIBattleLoadingComponent>
    {
        public override void OnInit()
        {
        }

        public override void OnDestroy()
        {
        }

        public override void OnOpen()
        {
            self.ProgressProxy.AddListener(UpdateProgress);
        }

        public override void OnClose()
        {
            self.ProgressProxy.RemoveListener(UpdateProgress);
        }

        public override void OnBeCover()
        {
        }

        public override void OnUnCover()
        {
        }

        void UpdateProgress(float rate)
        {
            self.EUI_Slider_Slider.value = rate;
        }
    }
}