using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	[UITag(UIType.UILogin)]
	public class UILoginComponent : Entity
	{
		public InputField account;
		public InputField password;
		public Button loginBtn;
	}
}
