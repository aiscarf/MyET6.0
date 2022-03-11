using System;

namespace ET
{
    public class B2G_OnBattleOver_Handler: AMActorHandler<Scene, B2G_OnBattleOver>
    {
        protected override async ETTask Run(Scene scene, B2G_OnBattleOver message)
        {
            try
            {
                var playerComponent = scene.GetComponent<PlayerComponent>();
                for (int i = 0; i < message.Uids.Count; i++)
                {
                    var uid = message.Uids[i];
                    var player = playerComponent.Get(uid);
                    if (player == null || player.IsConnected == false)
                    {
                        // DONE: 开始倒计时释放玩家对象, 除非玩家重新连接上来.
                        playerComponent.StartCountdownRemovePlayer(uid);
                        continue;
                    }
                    
                    // TODO: 通知客户端战斗结束.
                    // player.ClientSession?.Send();
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