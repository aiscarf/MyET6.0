using System.Collections.Generic;

namespace ET
{
    public sealed class BattleComponent: Entity
    {
        private Dictionary<int, BattleRoom> battleRooms = new Dictionary<int, BattleRoom>();
        private int BattleId;

        public BattleRoom CreateBattleRoom(int mapId)
        {
            int battleId = ++this.BattleId;
            int randomSeed = RandomHelper.RandomNumber(1000, 10000);
            string token = IdGenerater.Instance.GenerateInstanceId().ToString();
            var battleRoom = this.AddChild<BattleRoom, int, int, string, int>(battleId, mapId, token, randomSeed);
            this.battleRooms.Add(battleId, battleRoom);
            return battleRoom;
        }

        public BattleRoom GetRoom(int roomId)
        {
            BattleRoom result = null;
            this.battleRooms.TryGetValue(roomId, out result);
            return result;
        }

        public void DestroyBattleRoom(int battleId)
        {
            // TODO 销毁房间.
            this.battleRooms.Remove(battleId);
        }
    }
}