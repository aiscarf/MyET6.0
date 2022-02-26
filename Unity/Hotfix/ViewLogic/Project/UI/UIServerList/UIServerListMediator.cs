using SuperScrollView;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public partial class UIServerListMediator : UIMediator<UIServerListComponent>
    {
        public override void OnInit()
        {
            self.EUI_LoopListView2_ServerList.InitListView(10, OnGetItemByIndex);
            self.EUI_LoopGridView_RegionGrid.InitGridView(6, OnGetItemByRowColumn);
            self.EUI_Button_Close.onClick.AddListener(OnBtnCloseClick);
        }

        public override void OnDestroy()
        {
        }

        public override void OnOpen()
        {
            self.EUI_LoopListView2_ServerList.SetListItemCount(self.ServiceVos.Count);
            // DONE: 默认选择之前选择的服务器.
            OnServiceItemClick(0);
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

        LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
        {
            var data = self.GetServiceByIndex(index);
            if (data == null)
                return null;
            LoopListViewItem2 item = listView.NewListViewItem("ItemPrefab1");
            var btn = item.transform.Find("btn_bg").GetComponent<Button>();
            var txtName = item.transform.Find("txt_name").GetComponent<Text>();
            var txtTime = item.transform.Find("txt_time").GetComponent<Text>();
            txtName.text = data.ServiceName;
            txtTime.text = data.ServiceStartTime.ToString();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => { OnServiceItemClick(index); });

            return item;
        }

        void OnServiceItemClick(int index)
        {
            // DONE: 获取该服务器的大区列表, 进行刷新.
            self.m_nSelectServiceIndex = index;
            var serviceData = self.GetServiceByIndex(index);
            var list = self.GetRegionByServiceId(serviceData.ServiceId);
            self.EUI_LoopGridView_RegionGrid.SetListItemCount(list.Count);
            self.EUI_LoopGridView_RegionGrid.RefreshAllShownItem();
        }

        LoopGridViewItem OnGetItemByRowColumn(LoopGridView gridView, int itemIndex, int row, int column)
        {
            var serviceData = self.GetServiceByIndex(self.m_nSelectServiceIndex);
            var list = self.GetRegionByServiceId(serviceData.ServiceId);
            if (itemIndex < 0 || itemIndex >= list.Count)
                return null;
            var data = list[itemIndex];
            LoopGridViewItem item = gridView.NewListViewItem("ItemPrefab1");
            var btn = item.transform.Find("Button").GetComponent<Button>();
            var img = item.transform.Find("Image").GetComponent<Image>();
            var txt = item.transform.Find("Text").GetComponent<Text>();

            switch (data.State)
            {
                case 0:
                    img.color = Color.gray;
                    break;
                case 1:
                    img.color = Color.green;
                    break;
                case 2:
                    img.color = Color.red;
                    break;
            }

            txt.text = data.RegionName;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => { OnRegionItemClick(itemIndex, row, column); });
            return item;
        }

        void OnRegionItemClick(int index, int row, int column)
        {
            var serviceData = self.GetServiceByIndex(self.m_nSelectServiceIndex);
            var list = self.GetRegionByServiceId(serviceData.ServiceId);
            if (index < 0 || index >= list.Count)
                return;
            var data = list[index];
            // DONE: 记录选择的大区.
            self.CurSelectRegion = data;
            OnBtnCloseClick();
        }

        async void OnBtnCloseClick()
        {
            await UIHelper.CloseUI(UIType.UIServerList);
        }
    }
}