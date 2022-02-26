using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public partial class UIMainMediator : UIMediator<UIMainComponent>
    {
        public override void OnInit()
        {
            self.EUI_Image_Frame = self.EUI_Button_Frame.GetComponent<Image>();

            self.EUI_Button_Frame.onClick.AddListener(OnBtnFrameClick);
            self.EUI_Button_Mode.onClick.AddListener(OnBtnModeClick);
            self.EUI_Button_Match.onClick.AddListener(OnBtnMatchClick);
        }

        public override void OnDestroy()
        {
            self.EUI_Button_Frame.onClick.RemoveAllListeners();
            self.EUI_Button_Mode.onClick.RemoveAllListeners();
            self.EUI_Button_Match.onClick.RemoveAllListeners();
        }

        public override void OnOpen()
        {
            // TODO 设置头像.
            self.EUI_Text_PlayerName.text = self.PlayerName;
            self.EUI_Text_CupNum.text = self.CupNum.ToString();
            self.EUI_Image_Head.sprite = null;
            self.EUI_Image_Frame.sprite = null;

            // DONE: 设置模式.
            self.CurDungeonProxy.AddListener(SetMode);
            SetMode(self.CurDungeonProxy.GetValue());

            // DONE: 设置玩家选择的英雄.
            SetHero(self.CurSelectHeroId);
        }

        public override void OnClose()
        {
            self.CurDungeonProxy.RemoveListener(SetMode);
        }

        public override void OnBeCover()
        {
        }

        public override void OnUnCover()
        {
        }

        void SetMode(DungeonVO data)
        {
            self.EUI_Text_Mode.text = data.Name;
        }

        void OnBtnFrameClick()
        {
            // TODO 打开头像UI.
        }

        #region 设置英雄

        void SetHero(int heroId)
        {
            var heroConfig = HeroConfigCategory.Instance.Get(heroId);
            if (heroConfig == null)
                return;
            string resName = heroConfig.ModelRes;
            ResourcesComponent.Instance.LoadBundle(resName.StringToAB());
            var heroPrefab = (GameObject)ResourcesComponent.Instance.GetAsset(resName.StringToAB(), resName);
            if (heroPrefab == null)
                return;
            GameObject.DestroyImmediate(self.EUI_Transform_ShowCenter.GetChild(0).gameObject);
            var go = GameObject.Instantiate(heroPrefab);
            go.transform.SetParent(self.EUI_Transform_ShowCenter);
            go.transform.localScale = Vector3.one;
            go.transform.localEulerAngles = Vector3.zero;
            go.transform.localPosition = Vector3.zero;
            go.layer = LayerMask.NameToLayer("Default");
            var animation = go.GetComponent<Animation>();
            animation.Play("free", PlayMode.StopAll);
        }

        #endregion

        async void OnBtnModeClick()
        {
            await UIHelper.OpenUI(UIType.UISelectMap);
        }

        async void OnBtnMatchClick()
        {
            await MainMgr.StartReady();

            // DONE: 打开加载界面.
            await UIHelper.OpenUI(UIType.UIMatching);
        }
    }
}