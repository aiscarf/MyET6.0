using System;

namespace ET
{
    public class M2G_OnSuccessMatch_Handler: AMActorHandler<Scene, M2G_OnSuccessMatch>
    {
        protected override async ETTask Run(Scene scene, M2G_OnSuccessMatch message)
        {
            try
            {
                // DONE: 将玩家添加进战斗服的游戏房间.
                var allbattles = StartSceneConfigCategory.Instance.Battles;
                if (!allbattles.TryGetValue(scene.Zone, out var list))
                {
                    // TODO 配置错误, 通知客户端用户为什么取消匹配了.

                    return;
                }

                var battleSceneConfig = allbattles[scene.Zone][RandomHelper.RandomNumber(0, list.Count)];
                long actorId = battleSceneConfig.InstanceId;

                var b2GCreateBattle =
                        await MessageHelper.CallActor(actorId, new G2B_CreateBattle() { MapId = message.MapId, Uids = message.Uids }) as
                                B2G_CreateBattle;

                // TODO 如果战斗服不允许进入战斗, 需要善后.
                if (b2GCreateBattle.Error > 0)
                {
                    // TODO 通知客户端用户为什么匹配取消了.
                    Log.Error(b2GCreateBattle.Message);
                    return;
                }

                // DONE: 通知每一个玩家去连战斗服务器.
                var playerComponent = scene.GetComponent<PlayerComponent>();
                var g2COnGameStart = new G2C_OnGameStart();
                g2COnGameStart.MapId = message.MapId;
                g2COnGameStart.RoomId = b2GCreateBattle.RoomId;
                g2COnGameStart.Token = b2GCreateBattle.Token;
                g2COnGameStart.Host = battleSceneConfig.StartProcessConfig.OuterIP;
                g2COnGameStart.Port = battleSceneConfig.OuterPort;
                g2COnGameStart.RandomSeed = b2GCreateBattle.RandomSeed;
                g2COnGameStart.Players = b2GCreateBattle.Players;

                var uids = message.Uids;
                for (int i = 0; i < uids.Count; i++)
                {
                    var uid = message.Uids[i];
                    var player = playerComponent.Get(uid);
                    player.BattleActorId = actorId;
                    player.BattleToken = b2GCreateBattle.Token;
                    player.RoomId = b2GCreateBattle.RoomId;
                    player.ChangeState(EPlayerState.Game);
                    player.SendClient(g2COnGameStart);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}