using System;

namespace ET
{
    public class G2C_OnGameStart_Handler : AMHandler<G2C_OnGameStart>
    {
        protected override async ETTask Run(Session session, G2C_OnGameStart message)
        {
            try
            {
                Log.Debug("匹配成功, 准备战斗界面!");
                
                // DONE: 去连接战斗服务器.
                string address = $"{message.Host}:{message.Port}";
                await MainHelper.LoginBattle(address, message.RoomId, message.Token);
                
                // DONE: 将数据存储在战斗场景里.
                var battleDataComponent = DataHelper.GetDataComponentFromCurScene<BattleDataComponent>();
                battleDataComponent.RoomId = message.RoomId;
                battleDataComponent.MapId = message.MapId;
                battleDataComponent.Token = message.Token;
                battleDataComponent.RandomSeed = message.RandomSeed;
                battleDataComponent.BattleAddress = address;
                battleDataComponent.Players = message.Players;

                var battleViewDataComponent = BattleMgr.GetBattleViewDataComponent();
                battleViewDataComponent.Uid = battleDataComponent.Uid;
                
                // DONE: 通知登录了战斗服务器.
                await Game.EventSystem.PublishAsync(new EventType.LoginBattleFinish()
                    { MapId = message.MapId, RandomSeed = message.RandomSeed, Players = message.Players });
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}