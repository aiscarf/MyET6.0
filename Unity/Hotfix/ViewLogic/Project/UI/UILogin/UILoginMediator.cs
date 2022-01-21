using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UILoginMediator : UIMediator<UILoginComponent>
    {
        public override void OnInit()
        {
            self.account = referenceCollector.Get<GameObject>("Account").GetComponent<InputField>();
            self.password = referenceCollector.Get<GameObject>("Password").GetComponent<InputField>();
            self.loginBtn = referenceCollector.Get<GameObject>("LoginBtn").GetComponent<Button>();
            
            self.loginBtn.onClick.AddListener(OnBtnLoginClick);
        }

        public override void OnDestroy()
        {
            self.loginBtn.onClick.RemoveListener(OnBtnLoginClick);
        }

        public override void OnOpen(object data)
        {
        }

        public override void OnClose()
        {
        }

        public override void OnBeCover()
        {
        }

        public override void OnUnCover()
        {
        }

        private async void OnBtnLoginClick()
        {
            // TODO 1.应对账号密码进行初步的格式校验.
            await LoginHelper.Login(self.DomainScene(), ConstValue.LoginAddress, self.account.text, self.password.text);
        }
    }
}