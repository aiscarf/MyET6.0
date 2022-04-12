namespace ET
{
    public class AttrData
    {
        public SVector3 BornPos;
        public SVector3 BornForward;
        
        public int Level; // 等级
        public int Hp; // 生命
        public int Attack; // 攻击
        public int DefensePhysical; // 物理防御
        public int DefenseMagic; // 魔法防御
        public int MoveSpeed; // 移动速度
        public int AttackSpeed; // 攻击速度
        public int CollisionRadius; // 碰撞半径

        public int SkillId1; // 技能1 普攻
        public int SkillId2; // 技能2 角色
        public int SkillId3; // 技能3 宠物
        public int SkillId4; // 技能4 人造炮技能
        public int SkillId5; // 技能5
        public int SkillId6; // 技能6
        public int SkillId7; // 技能7

        public int KilledExp; // 被击杀经验
        public int RebornTime = -1; // 复活时间

        public int BattleHpUp; // 生命成长系数
        public int BattleAttackUp; // 攻击成长系数
        public int BattleDefPhysicalUp; // 物理防御成长
        public int BattleDefMagicUp; // 魔法防御成长
        public int BattleAttackSpeedUp; // 攻速成长
        public int BattleKilledExUp; // 死亡给予敌方经验成长系数

        public int TemplateId; // 模板id 可在策划表里查到的id
        public long ServerId; // 服务器id 可在服务器上查到的id
        public int SkinId; // 皮肤id 可在策划表里查到的id
        public string NickName; // 昵称

        public int ResId1; // 唯一资源id1
    }
}