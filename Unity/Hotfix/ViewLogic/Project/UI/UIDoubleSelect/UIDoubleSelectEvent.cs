namespace ET
{
    public class DoubleSelectData
    {
        public string Title;
        public string Content;
        public string LeftButtonName;
        public string RightButtonName;
        public ETTask<bool> ResultTask;
    }

    [UIEventTag(UIType.UIDoubleSelect)]
    public class UIDoubleSelectEvent : UIEvent<UIDoubleSelectComponent>
    {
        private ETTask<bool> m_resultAction = null;

        public override async ETTask PreOpen(object args)
        {
            if (args is DoubleSelectData doubleSelectData)
            {
                self.Title = doubleSelectData.Title;
                self.Content = doubleSelectData.Content;
                self.LeftButtonName = doubleSelectData.LeftButtonName;
                self.RightButtonName = doubleSelectData.RightButtonName;
                m_resultAction = doubleSelectData.ResultTask;
            }

            await UIManager.Instance.OpenUI(ViewUI.Name);
        }

        public override async ETTask PreClose()
        {
            await UIManager.Instance.DestroyUI(ViewUI.Name);
            if (m_resultAction == null)
                return;
            m_resultAction.SetResult(self.IsConfirm);
        }
    }
}