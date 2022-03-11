using System;

namespace ET
{
    public class G2C_OnPlayerKickOut_Handler : AMHandler<G2C_OnPlayerKickOut>
    {
        protected override async ETTask Run(Session session, G2C_OnPlayerKickOut message)
        {
            try
            {
                // DONE: 通知弹出单选确认弹窗.
                await Game.EventSystem.PublishAsync(new EventType.PlayerKickOut());

                // DONE: 当关闭弹窗时, 跳至登录场景.
                await ZoneSceneManagerComponent.Instance.ChangeScene(SceneType.Login);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}