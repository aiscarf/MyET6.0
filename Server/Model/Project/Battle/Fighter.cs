using System.Collections.Generic;

namespace ET
{
    public sealed class Fighter: Entity
    {
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
        public Session ClientSession { get; set; }
    }
}