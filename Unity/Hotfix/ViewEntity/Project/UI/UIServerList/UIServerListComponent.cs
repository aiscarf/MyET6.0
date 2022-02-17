using SuperScrollView;
using UnityEngine.UI;

namespace ET
{
    [UITag(UIType.UIServerList)]
    public class UIServerListComponent : Entity
    {
        public LoopListView2 EUI_LoopListView2_ServerList;
        public LoopGridView EUI_LoopGridView_RegionGrid;
        public Button EUI_Button_Close;
    }
}