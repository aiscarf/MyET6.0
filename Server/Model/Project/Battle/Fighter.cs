using System.Collections.Generic;

namespace ET
{
    public sealed class Fighter: Entity
    {
        /// <summary>
        /// 代理客户端同步到的帧数.
        /// </summary>
        public int ClientSyncId { get; set; }
        public bool IsConnected { get; set; }
        public Session ClientSession { get; set; }

        public bool IsReady { get; set; }
        public int RoomeId { get; set; }
        public long Uid { get; set; }
        public string Nickname { get; set; }
        public int HeroId { get; set; }
        public int HeroSkinId { get; set; }
        public int TowerSkinId { get; set; }
        public int PetId { get; set; }
        public int PetSkinId { get; set; }
        public int Score { get; set; }
        public int ChairId { get; set; }
        public int Camp { get; set; }
        public int HeadId { get; set; }
        public int FrameId { get; set; }
        public int ShowId { get; set; }
        public int HeroLv { get; set; }
        public int PetLv { get; set; }
        public List<int> UnlockedSkill { get; set; } = new List<int>();
    }
}