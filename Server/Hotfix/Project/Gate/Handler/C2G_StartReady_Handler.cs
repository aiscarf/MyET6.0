using System;

namespace ET
{
    public class C2G_StartReady_Handler: AMRpcHandler<C2G_StartReady, G2C_StartReady>
    {
        protected override async ETTask Run(Session session, C2G_StartReady request, G2C_StartReady response, Action reply)
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
                // DONE: 判断该玩家是否处于匹配中, 如果处于匹配中, 则不能再次匹配.
                if (player.PlayerState == EPlayerState.Match)
                {
                    response.Error = ErrorCode.ERR_GATE_ALREADY_MATCHING;
                    response.Message = "玩家已经处于匹配队列!";
                    reply();
                    return;
                }

                // DONE: 去匹配服开始匹配.
                var startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneType(session.DomainZone(), SceneType.Match);
                long actorId1 = startSceneConfig.InstanceId;
                var m2GStartMatch = await MessageHelper.CallActor(actorId1, new G2M_StartMatch() { Uid = player.Id }) as M2G_StartMatch;
                if (m2GStartMatch.Error > 0)
                {
                    response.Error = m2GStartMatch.Error;
                    response.Message = m2GStartMatch.Message;
                    reply();
                }

                // DONE: 玩家进入匹配状态.
                player.ChangeState(EPlayerState.Match);
                reply();
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}