using System;

namespace Scarf.Moba
{
    [Flags]
    public enum EUnitType
    {
        ENatual = 0, // 中立单位类型, 世界.

        EHero = 1 << 0, // 英雄
        EWild = 1 << 1, // 野怪
        ESummon = 1 << 2, // 召唤物
        EBuilding = 1 << 3, // 建筑
        ESoldier = 1 << 4, // 小兵
        EBoss = 1 << 5, // Boss
        EOther = 1 << 6, // 其他

        EAll = EHero | EWild | ESummon | EBuilding | ESoldier | EBoss | EOther, // 全部的
    }
}