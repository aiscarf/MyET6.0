using UnityEngine.UI;

namespace ET
{
    [UITag(UIType.UILoginScene)]
    public class UILoginSceneComponent : Entity
    {
        public Button EUI_Button_Enter;
        public Button EUI_Button_SelectServer;
        public Image EUI_Image_ServerState;
        public Text EUI_Text_ServerName;
    }
}