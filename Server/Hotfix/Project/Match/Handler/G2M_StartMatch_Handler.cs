using System;

namespace ET
{
    public class G2M_StartMatch_Handler: AMActorRpcHandler<Scene, G2M_StartMatch, M2G_StartMatch>
    {
        protected override async ETTask Run(Scene scene, G2M_StartMatch request, M2G_StartMatch response, Action reply)
        {
            try
            {
                // DONE: 查询玩家是否处于匹配ing.
                var matchComponent = scene.GetComponent<MatchComponent>();
                var matcher = matchComponent.Get(request.Uid);
                if (matcher != null)
                {
                    matchComponent.Remove(matcher.Uid);
                }

                // DONE: 创建匹配对象.
                matcher = matchComponent.AddChild<Matcher, long>(request.Uid);
                matcher.MapId = 1;
                matchComponent.Add(matcher);
                
                reply();
                
                await ETTask.CompletedTask;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}