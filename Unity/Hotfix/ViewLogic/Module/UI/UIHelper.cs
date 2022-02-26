using System;

namespace ET
{
    public static class UIHelper
    {
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
                if (!UIMediatorManager.Instance.m_allUiEvents.TryGetValue(uiType, out var uiEvent))
                {
                    Log.Error("UIHelper.OpenUI Error: 没有注册UIEvent");
                    return;
                }
                
                var ui = await UIHelper.GetOrCreateUI(uiType);
                uiEvent.Bind(ui);
                await UIMediatorManager.Instance.m_allUiEvents[uiType].PreOpen();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}