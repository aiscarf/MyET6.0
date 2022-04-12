namespace Scarf.Moba
{
    public class DamageInfo: IPoolable
    {
        public Unit Attacker { get; set; } // 攻击者
        public Unit Defenser { get; set; } // 防守者
        public Skill Skill { get; set; } // 技能 哪个技能造成的伤害
        public EDamageType DamageType { get; set; }
        public int DamageValue { get; set; } // 原始伤害
        public int DamageReductionValue { get; set; } // 承伤 {防御, 护盾等}
        public int DamageRealValue { get; set; } // 实际造成了多少伤害

        public DamageInfo Clone()
        {
            var dmg = CObjectPool<DamageInfo>.instance.GetObject();
            dmg.Attacker = this.Attacker;
            dmg.Defenser = this.Defenser;
            dmg.Skill = this.Skill;
            dmg.DamageType = this.DamageType;
            dmg.DamageValue = this.DamageValue;
            dmg.DamageReductionValue = this.DamageReductionValue;
            dmg.DamageRealValue = this.DamageRealValue;
            return dmg;
        }

        public void Reset()
        {
            this.Attacker = null;
            this.Defenser = null;
            this.Skill = null;
            this.DamageType = EDamageType.ENone;
            this.DamageValue = 0;
            this.DamageRealValue = 0;
            this.DamageReductionValue = 0;
        }
    }
}