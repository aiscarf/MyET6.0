using System;

namespace ET
{
    public class C2G_ReconnectGate_Handler: AMRpcHandler<C2G_ReconnectGate, G2C_ReconnectGate>
    {
        protected override async ETTask Run(Session session, C2G_ReconnectGate request, G2C_ReconnectGate response, Action reply)
        {
            try
            {
                var sessionPlayerComponent = session.GetComponent<SessionPlayerComponent>();
                if (sessionPlayerComponent != null && sessionPlayerComponent.Player.IsConnected)
                {
                    response.Error = ErrorCode.ERR_GATE_RECONNECT_FAILED;
                    response.Message = "玩家没有掉线, 所以不能重连";
                    reply();
                    return;
                }

                var playerComponent = session.DomainScene().GetComponent<PlayerComponent>();
                var player = playerComponent.Get(request.Uid);
                if (player == null)
                {
                    response.Error = ErrorCode.ERR_GATE_RECONNECT_PLAYER_NOT_EXIST;
                    response.Message = "玩家不存在, 请重新登录";
                    reply();
                    return;
                }

                if (request.GateToken != player.GateToken)
                {
                    response.Error = ErrorCode.ERR_GATE_RECONNECT_TOKEN_FAILED;
                    response.Message = "Token错误, 请重新登录";
                    reply();
                    return;
                }

                // TODO 将玩家的数据查询.
                reply();

                switch (player.PlayerState)
                {
                    case EPlayerState.None:
                        break;
                    case EPlayerState.Hall:
                        break;
                    case EPlayerState.Match:
                        break;
                    case EPlayerState.Game:
                        InBattle(player);
                        break;
                }

                session.AddComponent<SessionPlayerComponent>().Player = player;
                session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);
                player.ClientSession = session;

                await ETTask.CompletedTask;
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }

        async void InBattle(Player player)
        {
            var battleSceneConfig = StartSceneConfigCategory.Instance.GetByInstanceId(player.DomainZone(), player.BattleActorId);
            var b2GGetBattle = (B2G_GetBattle)await MessageHelper.CallActor(player.BattleActorId,
                new G2B_GetBattle() { RoomId = player.RoomId, Token = player.BattleToken });
            if (b2GGetBattle.Error > 0)
            {
                return;
            }

            var g2COnGameStart = new G2C_OnGameStart();
            g2COnGameStart.MapId = b2GGetBattle.MapId;
            g2COnGameStart.RoomId = b2GGetBattle.RoomId;
            g2COnGameStart.Token = b2GGetBattle.Token;
            g2COnGameStart.Host = battleSceneConfig.StartProcessConfig.OuterIP;
            g2COnGameStart.Port = battleSceneConfig.OuterPort;
            g2COnGameStart.RandomSeed = b2GGetBattle.RandomSeed;
            g2COnGameStart.Players = b2GGetBattle.Players;

            player.SendClient(g2COnGameStart);
        }
    }
}