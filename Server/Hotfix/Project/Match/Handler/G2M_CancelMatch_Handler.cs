using System;

namespace ET
{
    public class G2M_CancelMatch_Handler: AMActorRpcHandler<Scene, G2M_CancelMatch, M2G_CancelMatch>
    {
        protected override async ETTask Run(Scene scene, G2M_CancelMatch request, M2G_CancelMatch response, Action reply)
        {
            try
            {
                // DONE: 查询玩家是否处于匹配ing.
                var matchComponent = scene.GetComponent<MatchComponent>();
                var matcher = matchComponent.Get(request.Uid);
                if (matcher == null)
                {
                    response.Error = ErrorCode.ERR_MATCH_CANCEL_FAILED;
                    response.Message = "玩家没有处于匹配队列中";
                    reply();
                    return;
                }

                // DONE: 将正在匹配的玩家移除队列.
                matchComponent.Remove(matcher.Uid);
                reply();

                await ETTask.CompletedTask;
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}