using System;

namespace ET
{
    public class G2B_CreateBattle_Handler: AMActorRpcHandler<Scene, G2B_CreateBattle, B2G_CreateBattle>
    {
        protected override async ETTask Run(Scene scene, G2B_CreateBattle request, B2G_CreateBattle response, Action reply)
        {
            try
            {
                // DONE: 创建一个MapId玩法的战斗房间.
                var battleComponent = scene.GetComponent<BattleComponent>();
                var battleRoom = battleComponent.CreateBattleRoom(request.MapId);

                var dbComponent = DBManagerComponent.Instance.GetZoneDB(scene.Zone);
                if (dbComponent == null)
                {
                    battleComponent.DestroyBattleRoom(battleRoom.RoomId);
                    response.Error = ErrorCore.ERR_RpcFail;
                    response.Message = "服务器没有正常连接数据库";
                    reply();
                    return;
                }

                // DONE: 创建玩家战斗数据.
                var list = request.Uids;
                for (int i = 0; i < list.Count; i++)
                {
                    var uid = list[i];
                    var dbPlayerInfos = await dbComponent.Query<DBPlayerInfo>((info) => info.Id == uid);
                    if (dbPlayerInfos.Count <= 0)
                    {
                        battleComponent.DestroyBattleRoom(battleRoom.RoomId);
                        response.Players.Clear();
                        response.Error = ErrorCore.ERR_RpcFail;
                        response.Message = $"该[{uid}]玩家数据查询异常";
                        reply();
                        return;
                    }

                    var dbPlayerInfo = dbPlayerInfos[i];
                    var fighter = battleRoom.AddChild<Fighter>();
                    fighter.Uid = uid;
                    fighter.Nickname = dbPlayerInfo.Nickname;
                    fighter.HeroId = dbPlayerInfo.HeroId;
                    fighter.HeroSkinId = dbPlayerInfo.HeroId;
                    fighter.Score = dbPlayerInfo.Score;
                    fighter.ChairId = i;
                    fighter.Camp = i;
                    fighter.UnlockedSkill = dbPlayerInfo.UnlockedSkills;
                    fighter.HeroLv = 1;
                    battleRoom.Add(fighter);

                    var mobaPlayer = new MobaPlayerInfo();
                    mobaPlayer.Uid = uid;
                    mobaPlayer.Nickname = dbPlayerInfo.Nickname;
                    mobaPlayer.HeroId = dbPlayerInfo.HeroId;
                    mobaPlayer.HeroSkinId = dbPlayerInfo.HeroId;
                    mobaPlayer.Score = dbPlayerInfo.Score;
                    mobaPlayer.ChairId = i;
                    mobaPlayer.Camp = i;
                    mobaPlayer.UnlockedSkill = dbPlayerInfo.UnlockedSkills;
                    mobaPlayer.HeroLv = 1;
                    response.Players.Add(mobaPlayer);
                }

                // DONE: 回应创建战斗房间成功.
                response.MapId = request.MapId;
                response.RoomId = battleRoom.RoomId;
                response.Token = battleRoom.Token;
                response.RandomSeed = battleRoom.Seed;
                reply();
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}