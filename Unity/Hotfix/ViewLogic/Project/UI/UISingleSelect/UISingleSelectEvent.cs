namespace ET
{
    public class SingleSelectData
    {
        public string Title;
        public string Content;
        public string ButtonName;
        public ETTask ResultTask;
    }

    [UIEventTag(UIType.UISingleSelect)]
    public class UISingleSelectEvent : UIEvent<UISingleSelectComponent>
    {
        private ETTask m_resultTask = null;

        public override async ETTask PreOpen(object args)
        {
            if (args is SingleSelectData singleSelectData)
            {
                self.Title = singleSelectData.Title;
                self.Content = singleSelectData.Content;
                self.ButtonName = singleSelectData.ButtonName;
                m_resultTask = singleSelectData.ResultTask;
            }

            await UIManager.Instance.OpenUI(ViewUI.Name);
        }

        public override async ETTask PreClose()
        {
            await UIManager.Instance.DestroyUI(ViewUI.Name);
            if (m_resultTask == null)
                return;
            m_resultTask.SetResult();
        }
    }
}