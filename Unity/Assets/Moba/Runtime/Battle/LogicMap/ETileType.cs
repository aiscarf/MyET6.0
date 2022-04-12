namespace Scarf.Moba
{
    public enum ETileType
    {
        ENone = 0, //白色部分,可行走的部分
        EBase = 1, //基地
        EBorn = 2, //复活点
        EWild = 3, //野怪
        EProp = 4, //道具
        EGrass = 5, //草丛
        EAtkTower = 6, //進攻塔
        EDefTower = 7, //防禦塔
        ENeutralGuard = 8, //中立机关守卫
        EMachine = 9, //机关
        EScarecrow = 10, //稻草人
        EDrawingBorn = 11, //圖紙點
        EFireTrap = 12, //火焰陷阱
        EWallThornTrap = 13, //壁刺陷阱
        ESpikeTrap = 14, //地刺陷阱
        ERockfall = 15, //落石陷阱
        ERockfallSwitch = 16, //落石陷阱觸發器
        EPortal = 17, //傳送門
        EAirWall = 18, //空气墙

        EScuffleTreasure = 20, //宝箱生成点
        EScuffleFlowerQueue = 21, //女皇之花
        EScuffleStone = 22, //石碑
        EScuffleJump = 23, //跳台
        EScuffleStarWater = 24, //星之井
        EScuffleBox = 25, //魔盒
        EScuffleGuardOfQueen = 27, //女王之花守卫
        EScuffleDevilApple = 28, //恶魔果实
        EDefend_MonsterNestPoint = 29, //巢穴
        EDefend_SpecialItemPoint = 30, //材料点

        EDamageEObstacle = 254, //可破壞障碍物
        EObstacle = 255, //障碍物
    }
}