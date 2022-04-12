using System;

namespace Scarf.Moba
{
    [Flags]
    public enum EUnitType
    {
        ENone = 0,

        EHero = 1 << 0, // 玩家
        EWild = 1 << 1, // 野怪
        EGuard = 1 << 2, // 机关守卫
        ETower = 1 << 3, // 防御塔
        EBase = 1 << 4, // 基地
        EArtificialGun = 1 << 5, // 人造炮
        EAtkTower = 1 << 6, // 进攻塔
        EScarecrow = 1 << 7, // 稻草人
        ESummon1 = 1 << 8, // 召唤物1 : 图腾
        ENatual = 1 << 9, // 中立地形 [复活点, 陷阱]
    }
}