using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	/// *************************************************************************************************
	/// The following code is automatically generated by EUI framework. Please do not modify it manually.
	/// *************************************************************************************************
	public partial class UIBattleLoadingMediator
	{
		public override void OnAutoBind()
		{
			 self.EUI_Slider_Slider = this.referenceCollector.Get<GameObject>(nameof(self.EUI_Slider_Slider)).GetComponent<Slider>();
		}
	}
}
