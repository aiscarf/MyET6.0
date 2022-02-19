using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public partial class UIRegisterMediator : UIMediator<UIRegisterComponent>
    {
        public override void OnInit()
        {
            self.EUI_Button_Register.onClick.AddListener(OnBtnRegisterClick);
            self.EUI_Button_Close.onClick.AddListener(OnBtnCloseClick);
        }

        public override void OnDestroy()
        {
            self.EUI_Button_Register.onClick.RemoveAllListeners();
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

        async void OnBtnRegisterClick()
        {
            await LoginHelper.RegisterRealm(self.EUI_InputField_Account.text, self.EUI_InputField_Password.text);
        }

        async void OnBtnCloseClick()
        {
            await UIManager.Instance.CloseUI(UIType.UIRegister);
        }
    }
}