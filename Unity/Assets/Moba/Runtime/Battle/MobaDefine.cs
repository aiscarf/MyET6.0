using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Scarf.Moba
{
    public enum ESkillState
    {
        ENone = 0,

        /// <summary>
        /// 技能待命状态
        /// </summary>
        EReady,

        /// <summary>
        /// 前摇
        /// </summary>
        EForwardSwing,

        /// <summary>
        /// 技能施法中
        /// </summary>
        ECasting,

        /// <summary>
        /// 后摇
        /// </summary>
        EBackSwing,

        /// <summary>
        /// 技能结束
        /// </summary>
        ECastEnd,
    }

    public enum ESkillType
    {
        ENone,

        [LabelText("普攻技能")]
        EAttack,

        [LabelText("被动技能")]
        EPassive,

        [LabelText("主动技能")]
        EActive,
    }

    public enum ESkillActiveType
    {
        ENone,

        [LabelText("蓄力技能")]
        EXuLi,

        [LabelText("读条技能")]
        EDuTiao,

        [LabelText("充能技能")]
        EChongNeng,

        [LabelText("开关技能")]
        EOnOff,
    }

    public enum ESkillTargetType
    {
        ENone,

        [LabelText("对自己")]
        ESelf,

        [LabelText("对单体目标")]
        ESingle,

        [LabelText("对面向")]
        EFace,

        [LabelText("选范围")]
        EArea,
    }

    public enum ESkillTargetFilterType
    {
        ENone,

        [LabelText("敌方单位")]
        EEnemy,

        [LabelText("友方单位")]
        EFriendly,

        [LabelText("不包括自己的友方单位")]
        EFriendlyNotSelf,

        [LabelText("所有单位")]
        EAll,

        [LabelText("不包括自己的所有单位")]
        EAllNotSelf,
    }

    public enum EIndicatorShapeType
    {
        ENone,

        [LabelText("扇形")]
        ESector,

        [LabelText("圆形")]
        ECircle,

        [LabelText("矩形")]
        ERectangle,
    }

    public enum ESkillConsumeType
    {
        ENone,

        [LabelText("魔法值")]
        EMagic,

        [LabelText("生命值")]
        EHp,

        [LabelText("能量值")]
        EEnergy,
    }

    public enum EDamageType
    {
        ENone,

        [LabelText("物理伤害")]
        EPhysic,

        [LabelText("法术伤害")]
        EMagic,

        [LabelText("真实伤害")]
        EReal,

        [LabelText("恢复生命值")]
        ERecoverHp,

        [LabelText("恢复法力值")]
        ERecoverMp,

        [LabelText("损失法力值")]
        ELooseMp,
    }

    public enum EAoeShapeType
    {
        ENone,

        [LabelText("扇形")]
        ESector,

        [LabelText("圆形")]
        ECircle,

        [LabelText("矩形")]
        ERectangle,
    }

    public enum EAoeFollowType
    {
        ENone,

        [LabelText("不随单位移动")]
        EDontMove,

        [LabelText("随单位的绝对坐标移动")]
        EAbsoluteCoordinateMove,

        [LabelText("随单位的相对坐标移动")]
        ERelativeCoordinateMove,
    }

    public enum EAttrType
    {
        ENone = 0, // 应对策划配属性0的时候返回0
        Hp = 1001, // 血量

        HpLimit = 1002, // 血量上限    HpLimit = HpLimitBase * (1 + HpLimitRatio) + HpLimitAdd
        AtkPhysic = 1003, // 物理攻击    AtkPhysic = AtkPhysicBase * (1 + AtkPhysicRatio) + AtkPhysicAdd

        DefPhysic = 1005, // 物理防御    DefPhysic = DefPhysicBase * (1 + DefPhysicRatio) + DefPhysicAdd    百分比值
        DefMagic = 1006, // 魔法防御    DefMagic = DefMagicBase *  (1 + DefMagicRatio) + DefMagicAdd       百分比值
        MoveSpeed = 1007, // 移动速度    MoveSpeed = MoveSpeedBase * (1 + MoveSpeedRatio) + MoveSpeedAdd    百分比值
        AtkSpeed = 1008, // 攻击速度    攻击间隔 单位毫秒
        KilledExp = 1009, // 死亡敌方获取经验
        RebornTime = 1010, // 重生时间

        Level = 1104, // 角色等级
        Exp = 1106, // 经验
        Shield = 1110, // 护盾
        SkillCD = 1111, // 全技能CD系数, 千分比 默认为1000, [-1000, 1000]

        SkillDamage = 1201, // 技能伤害
        Damage = 1202, // 总伤害
        NormalAtkDamage = 1203, // 普攻伤害

        HpLimitBase = 10020, // (基础值) 血量上限        Base 基础值 = 初始值Initial + 等级成长值Up * (等级 - 1)
        AtkPhysicBase = 10030, // (基础值) 物理攻击力
        DefPhysicBase = 10050, // (基础值) 物理防御
        DefMagicBase = 10060, // (基础值) 魔法防御
        MoveSpeedBase = 10070, // (基础值) 移动速度  这个是没有计算
        AtkSpeedBase = 10080, // (基础值) 攻击速度

        HpLimitAdd = 10021, // [固定加成值] 血量上限 
        AtkPhysicAdd = 10031, // [固定加成值] 物理攻击力

        DefPhysicAdd = 10051, // [固定加成值] 物理防御
        DefMagicAdd = 10061, // [固定加成值] 魔法防御
        MoveSpeedAdd = 10071, // [固定加成值] 移动速度
        AtkSpeedAdd = 10081, // [固定加成值] 攻击速度

        HpLimitRatio = 10022, // <比例加成值> 血量系数
        AtkPhysicRatio = 10032, // <比例加成值> 物理攻击系数
        DefPhysicRatio = 10052, // <比例加成值> 物理防御系数
        DefMagicRatio = 10062, // <比例加成值> 魔法防御系数
        MoveSpeedRatio = 10072, // <比例加成值> 移动速度系数
        AtkSpeedRatio = 10082, // <比例加成值> 攻击速度加成系数

        HpLimitUp = 10023, // {等级成长值} 血量战场等级加成
        AtkPhysicUp = 10033, // {等级成长值} 物理攻击战场等级加成
        DefPhysicUp = 10053, // {等级成长值} 物理防御战场等级加成
        DefMagicUp = 10063, // {等级成长值} 魔法防御战场等级加成
        AtkSpeedUp = 10083, // {等级成长值} 攻击速度战场等级加成
        KilledExpUp = 10093, // {等级成长值} 死亡战场等级加成

        HpLimitInitial = 10024, // ^初始值^ 血量上限初始值
        AtkPhysicInitial = 10034, // ^初始值^ 攻击初始值
        DefPhysicInitial = 10054, // ^初始值^ 物理防御初始值
        DefMagicInitial = 10064, // ^初始值^ 魔法防御初始值
        AtkSpeedInitial = 10084, // ^初始值^ 攻击速度初始值
        KilledExpInitial = 10094, // ^初始值^ 死亡敌方获取经验值
    }

    public enum EUnitState
    {
        ENone,

        [LabelText("是否可移动")]
        ECanMove,

        [LabelText("是否可普攻")]
        ECanAttack,

        [LabelText("是否可技能")]
        ECanSkill,

        [LabelText("是否可道具")]
        ECanProp,

        [LabelText("是否可选中")]
        ECanBeSelect,

        [LabelText("是否可命中")]
        ECanBeHit,

        [LabelText("是否可打断")]
        ECanBeBreak,
    }

    public enum EBuffGainType
    {
        [LabelText("减益")]
        EReduce,

        [LabelText("增益")]
        EGain,
    }

    public enum EBuffStackType
    {
        [LabelText("替换")]
        EReplace,

        [LabelText("最大时间留存")]
        EMaxTime,

        [LabelText("叠层")]
        EStack,
    }

    public static class SkillTip
    {
        public static string Damage = "伤害";
        public static string Shield = "护盾";
        public static string RecoverHp = "回血";
        public static string RecoverMp = "回蓝";
        public static string Shift = "位移";
        public static string SpeedUp = "加速";
        public static string SpeedDown = "减速";
        public static string Control = "控制";
        public static string Fly = "击飞";
        public static string Strengthen = "强化";
        public static string InjuryFree = "免伤";
        public static string Special = "特殊";
    }

    public class SkillData
    {
        // 技能id
        public int Id { get; set; }

        // 技能名字
        public string Name { get; set; }

        // 技能图标资源名
        public string Icon { get; set; }

        // 技能图标提示
        public string Tip { get; set; }

        // 技能描述
        public string Des { get; set; }

        // 技能总等级数量
        public int MaxLevel { get; set; }

        // 技能类型
        public ESkillType SkillType { get; set; }

        // 技能目标类型
        public ESkillTargetType SkillTargetType { get; set; }

        // 技能目标过滤类型
        public ESkillTargetFilterType SkillTargetFilterType { get; set; }

        // 技能目标过滤黑名单
        public List<int> SkillTargetFilterBlacklist { get; set; }

        // 技能目标过滤白名单
        public List<int> SkillTargetFilterWhitelist { get; set; }

        // 技能指示器形状
        public EIndicatorShapeType IndicatorShapeType { get; set; }

        // 技能指示器形状参数
        public int IndicatorShapeArg1 { get; set; }
        public int IndicatorShapeArg2 { get; set; }

        // 技能施法距离
        public int SkillDistance { get; set; }

        // 智能施法距离
        public int SkillIntellectDistance { get; set; }

        // 技能冷却时间
        public int SkillCD { get; set; }

        // 技能资源消耗类型
        public ESkillConsumeType SkillConsumeType { get; set; }

        // 技能消耗数量
        public int SkillConsumeNum { get; set; }

        // 是否切换技能自动取消时间
        public bool ChangeSkillAutoCancelTime { get; set; }

        // 是否强制面向施法方向
        public bool CastForceFace { get; set; }

        // 施放时可转向
        public bool CastCanTurn { get; set; }

        // 施放时可移动
        public bool CastCanMove { get; set; }

        // 蓄力时间
        public int XuLiTime { get; set; }

        // 蓄力动画
        public string XuLiAnimation { get; set; }

        // 蓄力时是否可转向
        public bool XuLiCanTurn { get; set; }

        // 蓄力时是否可移动
        public bool XuLiCanMove { get; set; }

        // 蓄力超时是否自动释放
        public bool XuLiOtAutoCast { get; set; }

        // 蓄力取消返还冷却比例
        public int XuLiOtRCdPct { get; set; }

        // 蓄力取消返还资源比例
        public int XuLiOtRResumePct { get; set; }
    }

    public class DamageData
    {
        // 伤害类型
        public EDamageType DamageType { get; set; }

        // TODO 伤害公式怎么配置?

        // 伤害基础值
        public int DamageNum { get; set; }

        // 伤害是否可暴击
        public bool IsCrit { get; set; }
    }

    public class AoeData
    {
        // Aoe形状类型
        public EAoeShapeType AoeShapeType { get; set; }

        // Aoe形状参数1
        public int AoeShapeArg1 { get; set; }

        // Aoe形状参数2
        public int AoeShapeArg2 { get; set; }

        // Aoe持续时间
        public int AoeTime { get; set; }

        // Aoe持续伤害段数 int (持续时间内总共造成几段伤害, 均匀分布)
        public int AoeDamageNum { get; set; }

        // Aoe跟随模式 (持续型伤害范围使用)
        public EAoeFollowType AoeFollowType { get; set; }
    }

    public class ModifierData
    {
        // 修改的属性类型 TargetAttrType = RefAttrType * Ratio + Fixed
        public EAttrType TgtAttrType { get; set; }

        // 参照的属性类型
        public EAttrType RefAttrType { get; set; }

        // 0: 是参照受击者的属性 1: 是参照施法者的属性
        public int RefTarget { get; set; }

        // 修改系数
        public int RatioNum { get; set; }

        // 修改固定
        public int FixedNum { get; set; }
    }

    public class BuffData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public EBuffStackType BuffStackType { get; set; }

        // Buff持续时间
        public int Time { get; set; }

        // Buff最大叠层
        public int MaxLayer { get; set; }

        // Buff利弊类型
        public EBuffGainType BuffGainType { get; set; }

        // Buff修改的属性
        public ModifierData ChangeAttrData { get; set; }

        // Buff造成的伤害
        public DamageData ChangeDamageData { get; set; }

        // Buff修改的基础状态 EUnitState
        public List<int> ChangeStates { get; set; }

        // Buff是否死亡移除
        public bool IsDieRemove { get; set; }
    }

    public class AnimationData
    {
        public string AnimationName;
        public List<AnimationEventData> AnimationEvents;
    }

    public class AnimationEventData
    {
        public string Name; // event key
        public int Time; // ms
    }
}