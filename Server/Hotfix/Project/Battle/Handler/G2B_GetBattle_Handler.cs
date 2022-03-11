using System;

namespace ET
{
    public class G2B_GetBattle_Handler: AMActorRpcHandler<Scene, G2B_GetBattle, B2G_GetBattle>
    {
        protected override async ETTask Run(Scene scene, G2B_GetBattle request, B2G_GetBattle response, Action reply)
        {
            try
            {
                var battleComponent = scene.GetComponent<BattleComponent>();
                var battleRoom = battleComponent.GetRoom(request.RoomId);
                if (battleRoom == null)
                {
                    response.Error = ErrorCode.ERR_BATTLE_ROOM_NOT_EXIST;
                    response.Message = "房间不存在";
                    reply();
                    return;
                }

                if (battleRoom.Token != request.Token)
                {
                    response.Error = ErrorCode.ERR_BATTLE_TOKEN_FAILED;
                    response.Message = "Token错误";
                    reply();
                    return;
                }

                response.Token = battleRoom.Token;
                response.MapId = battleRoom.MapId;
                response.RoomId = battleRoom.RoomId;
                response.Token = battleRoom.Token;
                response.RandomSeed = battleRoom.Seed;

                var allFighters = battleRoom.GetAllFighters();

                foreach (var kv in allFighters)
                {
                    var fighter = kv.Value;
                    var mobaPlayer = new MobaPlayerInfo();
                    mobaPlayer.Uid = fighter.Uid;
                    mobaPlayer.Nickname = fighter.Nickname;
                    mobaPlayer.HeroId = fighter.HeroId;
                    mobaPlayer.HeroSkinId = fighter.HeroId;
                    mobaPlayer.Score = fighter.Score;
                    mobaPlayer.ChairId = fighter.ChairId;
                    mobaPlayer.Camp = fighter.Camp;
                    mobaPlayer.UnlockedSkill = fighter.UnlockedSkill;
                    mobaPlayer.HeroLv = fighter.HeroLv;
                    response.Players.Add(mobaPlayer);
                }

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