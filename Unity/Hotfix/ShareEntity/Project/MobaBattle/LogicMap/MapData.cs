using System.Collections.Generic;

namespace ET
{
    public class MapData
    {
        public int Id;//地图编号
        public int CellSize;  // 地图格子大小
        public int RowCount;    // 地图多少行
        public int ColumnCount; // 地图多少列
        public List<CellData> CellList; // 障碍物细胞数据
        public List<ElementItemData> ElementsDataList = new List<ElementItemData>(); //物件信息列表
    }

    /// <summary>
    /// 单个物件信息 (由格子组成)
    /// </summary>
    public class ElementItemData
    {
        public int UId;// 物件UID
        public int Id;// 物件ID
        public int LogicID;// 物件數據ID
        public int Angle;// 物件旋转角
        public int Camp;//阵营
        public EnumElementType ElementType;  // 物件类型
        public ETileType ElementTileType;//地形类型
        public int[][] MapInfo;
        public List<CellData> CellList; // 障碍物细胞数据
        public List<TerrainSpositionInfo> SpositionInfo = new List<TerrainSpositionInfo>();
    }

    /// <summary>
    /// 单个格子信息
    /// </summary>
    public class CellData
    {
        public int x;
        public int z;
        public int Obstacle;
    }
    
    public class TerrainSpositionInfo
    {
        public int ScenterPosX;
        public int ScenterPosY;
        public int ScenterPosZ;
        public int Sheight;
        public int Swidth;
    }
    

    /// <summary>
    /// 物件类型
    /// </summary>
    public enum EnumElementType
    {
        None,                                //     None
        BlueBase,                          //     蓝方基地
        BlueBorn,                          //     蓝方出生点
        BlueTower,                         //     蓝方防御塔
        RedBase,                           //     红方基地
        RedBorn,                           //     红方出生点
        RedTower,                          //     红方防御塔
        IndestructibleObstacle01,          //     不可破坏障碍物01
        IndestructibleObstacle02,          //     不可破坏障碍物02
        IndestructibleObstacle02_180,      //     不可破坏障碍物02_旋转180
        IndestructibleObstacle01_180,      //     不可破坏障碍物01_旋转180
        IndestructibleObstacle03,          //     不可破坏障碍物03
        IndestructibleObstacle04,          //     不可破坏障碍物04
        IndestructibleObstacle03_180,      //     不可破坏障碍物03_旋转180
        IndestructibleObstacle04_180,      //     不可破坏障碍物04_旋转180
        IndestructibleObstacle05,          //     不可破坏障碍物05
        IndestructibleObstacle06,          //     不可破坏障碍物06
        IndestructibleObstacle07,          //     不可破坏障碍物07
        Grass01,                           //     草丛01
        Grass02,                           //     草丛02
        Grass03,                           //     草丛03
        Grass04,                           //     草丛04
        GroundSpikeTrap01,                 //     地刺陷阱01
        GroundSpikeTrap02,                 //     地刺陷阱02
        WallSpikeTrap,                     //     壁刺陷阱
        StoneSwitchArea,                   //     落石陷阱触发区域
        StoneDamageArea,                   //     落石陷阱伤害区域
        FireTrap,                          //     火焰陷阱
        Portal,                            //     传送门
        CenterGear,                        //     中央机关
        CenterGuard,                       //     中央机关守卫
        CenterGearTower01,                   //     中央机关进攻塔01
        CenterGearTower02,                   //     中央机关进攻塔02 
        Scarecrow,                          //     稻草人
        BluePrintsPoint,                    //     图纸点 
        PropsPoint,                         //     道具点
        MonstersPoint,                      //     野怪生成点
        CornerSquare,                       //     图纸点
        Xwall,                              //     道具点
        Zwall,                              //     野怪生成点
        Obstacle_Part01,                    //     01部分地图障碍物
        Obstacle_Part02,                    //     02部分地图障碍物
        Obstacle_Part03,                    //     03部分地图障碍物
        Obstacle_Part04,                    //     04部分地图障碍物
        CenterGrass01,                      //     中央区域草丛
        CenterGrass02,                      //     中央区域草丛
        CenterGrass03,                      //     中央区域草丛
        CenterGrass04,                      //     中央区域草丛
        NewGrass01,                         //     中央区域草丛
        NewGrass02,                         //     中央区域草丛
        NewGrass03,                         //     中央区域草丛
        NewGrass04,                         //     中央区域草丛
        NewGrass05,                         //     中央区域草丛
        NewGrass06,                         //     中央区域草丛
        NewGrass07,                         //     中央区域草丛
        NewGrass08,                         //     中央区域草丛
        NewGrass09,                         //     中央区域草丛
        NewGrass10,                         //     中央区域草丛
        NewGrass11,                         //     中央区域草丛
        NewGrass12,                         //     中央区域草丛
        LIttleGrass03,                      //     1x1草丛
        LIttleGrass01,                      //     1x2草丛
        LIttleGrass02,                      //     2x1草丛
        on1_Obstacle,                       //     1V1地图障碍物
        on1_Grass01,                        //       1V1草丛01
        on1_Grass02,                        //       1V1草丛02
        on1_Grass03,                        //       1V1草丛03
        on1_Grass04,                        //       1V1草丛04
        on1_Grass05,                        //       1V1草丛05
        on1_Grass06,                        //       1V1草丛06
        v1_Center,                          //      1v1中央机关
        Ready_Wall01,                       //      准备时间的空气墙
        on1_RedBornPoint,                   //      1v1红方出生点
        on1_BlueBornPoint,                  //      1v1蓝方出生点
        Boss_Obstacle,                      //      BOSS地图边缘不可破坏障碍物
        Boss_InsideObstacle,                //      BOSS地图场景内可破坏障碍物
        Boss_InsideObstacle01,              //       BOSS地图场景内可破坏障碍物01
        Boss_InsideObstacle02,              //      BOSS地图场景内可破坏障碍物02
        Boss_InsideObstacle03,              //        BOSS地图场景内可破坏障碍物03
        Boss_InsideObstacle04,              //      Boss_InsideObstacle04
        TreasurePoint,                      //      宝箱生成点，新的地形类型
        Luandou_Obstacle01,                 //      乱斗地图障碍物01
        Luandou_Obstacle02,                 //      乱斗地图障碍物02
        Luandou_Obstacle03,                 //      乱斗地图障碍物03
        Luandou_Obstacle04,                 //      乱斗地图障碍物04
        Luandou_Stone,
        Luandou_Grass01,
        Luandou_Grass02,
        Luandou_Grass03,
        Luandou_Grass04,
        Luandou_Grass05,
        Luandou_Grass06,
        Luandou_Grass07,
        Luandou_Grass08,
        Luandou_Grass09,
        Luandou_Grass10,
        Luandou_Grass11,
        Luandou_Grass12,
        Luandou_Grass13,
        Luandou_Grass14,
        Luandou_Grass15,
        Luandou_Grass16,
        Luandou_Jump,
        Luandou_StarWater,
        Luandou_Box,
        FlowerofQueen,
        Luandou_GuardOfQueen,
        Luandou_DevilApple,
        //守城战模式
        Defend_MonsterNestPoint,
        Defend_SpecialItemPoint,
        Defend_Obstacle01,
        Defend_Obstacle02,
        Defend_Obstacle03,
        Defend_Obstacle04,
    }

}