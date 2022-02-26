using SuperScrollView;
using UnityEngine.UI;

namespace ET
{
    public partial class UISelectMapMediator : UIMediator<UISelectMapComponent>
    {
        public override void OnInit()
        {
            self.EUI_LoopGridView.InitGridView(6, OnGetItemByRowColumn);
            self.EUI_Button_Close.onClick.AddListener(OnBtnCloseClick);
        }

        public override void OnDestroy()
        {
        }

        public override void OnOpen()
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

        LoopGridViewItem OnGetItemByRowColumn(LoopGridView gridView, int itemIndex, int row, int column)
        {
            var data = self.DungeonVos[itemIndex];
            LoopGridViewItem item = gridView.NewListViewItem("ItemPrefab1");
            var btn = item.transform.Find("Button").GetComponent<Button>();
            var img = btn.transform.GetComponent<Image>();
            img.sprite = AtlasHelper.LoadSprite(AtlasHelper.ATLAS_DUNGEON, data.Background);
            var txt = item.transform.Find("Name").GetComponent<Text>();
            txt.text = data.Name;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => { OnBtnMapClick(itemIndex); });
            return item;
        }

        void OnBtnMapClick(int index)
        {
            var data = self.DungeonVos[index];
            self.CurSelectDungeonVo = data;
            OnBtnCloseClick();
        }

        async void OnBtnCloseClick()
        {
            await UIHelper.CloseUI(this);
        }
    }
}