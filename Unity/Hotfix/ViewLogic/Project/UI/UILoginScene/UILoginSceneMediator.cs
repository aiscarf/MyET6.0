using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UILoginSceneMediator : UIMediator<UILoginSceneComponent>
    {
        public override void OnInit()
        {
            self.EUI_Button_Enter = this.referenceCollector.Get<GameObject>(nameof(self.EUI_Button_Enter)).GetComponent<Button>();
            self.EUI_Button_SelectServer = this.referenceCollector.Get<GameObject>(nameof(self.EUI_Button_SelectServer)).GetComponent<Button>();
            self.EUI_Image_ServerState = this.referenceCollector.Get<GameObject>(nameof(self.EUI_Image_ServerState)).GetComponent<Image>();
            self.EUI_Text_ServerName = this.referenceCollector.Get<GameObject>(nameof(self.EUI_Text_ServerName)).GetComponent<Text>();

            self.EUI_Button_Enter.onClick.AddListener(OnBtnEnterClick);
            self.EUI_Button_SelectServer.onClick.AddListener(OnBtnSelectServerClick);
        }

        public override void OnDestroy()
        {
        }

        public override void OnOpen(object data)
        {
            var loginDataComponent = DataHelper.GetDataComponentFromCurScene<LoginDataComponent>();
            bool bLoginRealm = loginDataComponent.IsLoginRealm;
            self.EUI_Button_SelectServer.gameObject.SetActive(bLoginRealm);

            var loginViewDataComponent = DataHelper.GetDataComponentFromCurScene<LoginViewDataComponent>();
            if (!bLoginRealm) return;
            self.EUI_Text_ServerName.text = loginViewDataComponent.CurSelectRegion.RegionName;
        }

        public override void OnClose()
        {
        }

        public override void OnBeCover()
        {
        }

        public override void OnUnCover()
        {
            var loginDataComponent = DataHelper.GetDataComponentFromCurScene<LoginDataComponent>();
            bool bLoginRealm = loginDataComponent.IsLoginRealm;
            self.EUI_Button_SelectServer.gameObject.SetActive(bLoginRealm);

            var loginViewDataComponent = DataHelper.GetDataComponentFromCurScene<LoginViewDataComponent>();
            if (!bLoginRealm) return;
            self.EUI_Text_ServerName.text = loginViewDataComponent.CurSelectRegion.RegionName;
        }

        void OnBtnEnterClick()
        {
            var loginDataComponent = DataHelper.GetDataComponentFromCurScene<LoginDataComponent>();
            bool bLoginRealm = loginDataComponent.IsLoginRealm;
            if (!bLoginRealm)
            {
                UIManager.Instance.OpenUI(UIType.UILogin, null).Coroutine();
            }
            else
            {
                // DONE: 真正进入游戏
                var loginViewDataComponent = DataHelper.GetDataComponentFromCurScene<LoginViewDataComponent>();
                LoginHelper.LoginGate(loginViewDataComponent.CurSelectRegion.Address).Coroutine();
            }
        }

        async void OnBtnSelectServerClick()
        {
            await UIManager.Instance.OpenUI(UIType.UIServerList, null);
        }
    }
}