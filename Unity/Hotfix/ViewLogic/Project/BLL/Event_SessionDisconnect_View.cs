using System;

namespace ET
{
    public class Event_SessionDisconnect_View : AEvent<EventType.SessionDisconnect>
    {
        protected override async ETTask Run(EventType.SessionDisconnect args)
        {
            try
            {
                // DONE: 主场景的自动断线重连.
                if (args.SceneType == SceneType.Main)
                {
                    bool b = await UIHelper.ShowDoubleSelectAsync("提示", "已断线", "尝试重新连接", "重新登录");
                    if (b)
                    {
                        var mainScene = ZoneSceneManagerComponent.Instance.CurScene;
                        if (mainScene.SceneType != SceneType.Main)
                            return;

                        await MainHelper.ReconnectGate();
                    }
                    else
                    {
                        await ZoneSceneManagerComponent.Instance.ChangeScene(SceneType.Login);
                    }
                }

                await ETTask.CompletedTask;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}