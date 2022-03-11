using System;

namespace ET
{
    public class C2G_CancelReady_Handler: AMRpcHandler<C2G_CancelReady, G2C_CancelReady>
    {
        protected override async ETTask Run(Session session, C2G_CancelReady request, G2C_CancelReady response, Action reply)
        {
            try
            {
                var sessionPlayerComponent = session.GetComponent<SessionPlayerComponent>();
                if (sessionPlayerComponent == null)
                {
                    response.Error = ErrorCode.ERR_DISCONNECTED;
                    response.Message = "请重新连接";
                    reply();
                    return;
                }

                var player = sessionPlayerComponent.Player;

                // DONE: 去匹配服取消匹配.
                var startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneType(session.DomainZone(), SceneType.Match);
                long actorId1 = startSceneConfig.InstanceId;
                var m2GCancelMatch = await MessageHelper.CallActor(actorId1, new G2M_CancelMatch() { Uid = player.Id }) as M2G_CancelMatch;
                if (m2GCancelMatch.Error > 0)
                {
                    response.Error = m2GCancelMatch.Error;
                    response.Message = m2GCancelMatch.Message;
                    reply();
                    return;
                }

                // DONE: 将该玩家状态置为大厅待机状态.
                if (player.PlayerState != EPlayerState.Match)
                {
                    player.ChangeState(EPlayerState.Hall);
                }

                reply();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}