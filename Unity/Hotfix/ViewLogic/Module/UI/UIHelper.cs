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

        public static async ETTask OpenUI(string uiType, object args = null)
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
                await UIMediatorManager.Instance.m_allUiEvents[uiType].PreOpen(args);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static void ShowTip(string content)
        {
            OpenUI(UIType.UITip, content).Coroutine();
        }

        public static async void ShowSingleSelect(string title, string content, string buttonName, Action action)
        {
            ETTask resultTask = ETTask.Create();
            await OpenUI(UIType.UISingleSelect,
                new SingleSelectData()
                    { Title = title, Content = content, ButtonName = buttonName, ResultTask = resultTask });
            await resultTask;
            if (action == null)
                return;
            action.Invoke();
        }

        public static async ETTask ShowSingleSelectAsync(string title, string content, string buttonName)
        {
            ETTask resultTask = ETTask.Create();
            await OpenUI(UIType.UISingleSelect,
                new SingleSelectData()
                    { Title = title, Content = content, ButtonName = buttonName, ResultTask = resultTask });
            await resultTask;
        }

        public static async void ShowDoubleSelect(string title, string content,
            string leftButtonName, string rightButtonName,
            Action confirmAction, Action cancelAction)
        {
            ETTask<bool> resultTask = ETTask<bool>.Create();
            await OpenUI(UIType.UIDoubleSelect,
                new DoubleSelectData()
                {
                    Title = title, Content = content, ResultTask = resultTask,
                    LeftButtonName = leftButtonName, RightButtonName = rightButtonName
                });
            var b = await resultTask;
            if (b)
            {
                confirmAction?.Invoke();
            }
            else
            {
                cancelAction?.Invoke();
            }
        }

        public static async ETTask<bool> ShowDoubleSelectAsync(string title, string content,
            string leftButtonName, string rightButtonName)
        {
            ETTask<bool> resultTask = ETTask<bool>.Create();
            await OpenUI(UIType.UIDoubleSelect,
                new DoubleSelectData()
                {
                    Title = title, Content = content, ResultTask = resultTask,
                    LeftButtonName = leftButtonName, RightButtonName = rightButtonName
                });
            return await resultTask;
        }
    }
}