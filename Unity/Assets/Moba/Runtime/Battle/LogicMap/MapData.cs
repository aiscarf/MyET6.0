using System.Collections.Generic;

namespace Scarf.Moba
{
    public class MapData
    {
        public int Id; //地图编号
        public int CellSize; // 地图格子大小
        public int RowCount; // 地图多少行
        public int ColumnCount; // 地图多少列

        public List<CellData> CellList; // 障碍物细胞数据
        // public List<ElementItemData> ElementsDataList = new List<ElementItemData>(); //物件信息列表
    }

    /// <summary>
    /// 单个物件信息 (由格子组成)
    /// </summary>
    public class ElementItemData
    {
        public int UId; // 物件UID
        public int Id; // 物件ID
        public int LogicID; // 物件數據ID
        public int Angle; // 物件旋转角
        public int Camp; //阵营
        public ETileType ElementTileType; //地形类型
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
    
    public class NodeData
    {
        public int Index { get; set; }
        
        public int X { get; set; }
        public int Z { get; set; }
        public int TileType { get; set; }
    }
}