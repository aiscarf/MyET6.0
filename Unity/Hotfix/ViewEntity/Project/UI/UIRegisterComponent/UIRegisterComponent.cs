using UnityEngine.UI;

namespace ET
{
    [UITag(UIType.UIRegister)]
    public class UIRegisterComponent : Entity
    {
        public InputField EUI_InputField_Account;
        public InputField EUI_InputField_Password;
        public Button EUI_Button_Register;
        public Button EUI_Button_Close;
    }
}