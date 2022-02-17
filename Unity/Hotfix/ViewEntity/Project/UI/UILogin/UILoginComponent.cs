using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	[UITag(UIType.UILogin)]
	public class UILoginComponent : Entity
	{
		public Button EUI_Button_Register;
		public InputField EUI_InputField_Account;
		public InputField EUI_InputField_Password;
		public Button EUI_Button_Login;
	}
}
