namespace Scarf.Moba
{
    public enum EMoveType
    {
        ENone,

        /// <summary>
        /// 正常移动
        /// </summary>
        ENormal,
        ESystem,
        ESystemIgnoreTerrain,

        /// <summary>
        /// 受击移动
        /// </summary>
        EHurtMove,
        EHurtMoveIgnoreAll,

        /// <summary>
        /// 技能移动
        /// </summary>
        ESkillMove,
        ESkillMoveIgnoreTerrain,
        EFear,
    }
}