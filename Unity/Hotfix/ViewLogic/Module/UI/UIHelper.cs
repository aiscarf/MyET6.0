using System;

namespace ET
{
    public static class UIHelper
    {
        public static readonly ViewDataDomain ViewDataDomain = new ViewDataDomain();

        public static async ETTask<UI> GetOrCreateUI(string uiType)
        {
            UI result = null;
            result = UIManager.Instance.GetUI(uiType) ?? await UIManager.Instance.CreateUI(uiType);
            return result;
        }

        public static async ETTask CloseUI(IMediator mediator)
        {
            string uiType = mediator.ViewUI.Name;
            await CloseUI(uiType);
        }

        public static async ETTask CloseUI(string uiType)
        {
            try
            {
                await UIMediatorManager.Instance.m_allUiEvents[uiType].PreClose();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static async ETTask OpenUI(string uiType)
        {
            try
            {
                await UIMediatorManager.Instance.m_allUiEvents[uiType].PreOpen();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}