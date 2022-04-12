using System.Collections.Generic;

namespace Scarf.Moba
{
    public class Map
    {
        private int m_nHeight;
        private int m_nWidth;
        private int m_nCellSize;
        private int m_nColumnCount;
        private int m_nRowCount;
        private SVector3 m_mapCenterPos;
        private int m_nHalfHeight;
        private int m_nHalfWidth;
        private List<Cell> mCellList;

        public int width
        {
            get { return this.m_nWidth; }
        }

        public int height
        {
            get { return this.m_nHeight; }
        }

        public int cellSize
        {
            get { return this.m_nCellSize; }
            set { this.m_nCellSize = value; }
        }

        public int columnCount
        {
            get { return this.m_nColumnCount; }
            set { this.m_nColumnCount = value; }
        }

        public int rowCount
        {
            get { return this.m_nRowCount; }
            set { this.m_nRowCount = value; }
        }

        public SVector3 centerPos
        {
            get { return this.m_mapCenterPos; }
            set { this.m_mapCenterPos = value; }
        }

        public Cell this[int nRowIdx, int nColumnIdx]
        {
            get
            {
                return nRowIdx < 0 || nRowIdx >= this.rowCount || (nColumnIdx < 0 || nColumnIdx >= this.columnCount)
                    ? (Cell)null
                    : this.mCellList[nRowIdx * this.columnCount + nColumnIdx];
            }
        }

        public void Clear()
        {
            if (this.mCellList == null)
                return;

            foreach (Cell cell in this.mCellList)
            {
                cell.Clear();
            }

            this.mCellList.Clear();
            this.mCellList = null;
        }

        public Cell GetCellByWorldPos(SVector3 sLogicPos)
        {
            int index1 = (this.m_mapCenterPos.y + this.m_nHalfHeight - sLogicPos.z) / this.m_nCellSize;
            int index2 = (sLogicPos.x - (this.m_mapCenterPos.x - this.m_nHalfWidth)) / this.m_nCellSize;
            return this[index1, index2];
        }

        public SVector3 GetWorldPosByIdx(int row, int column)
        {
            SVector3 svector3;
            svector3.z = this.m_mapCenterPos.y + this.m_nHalfHeight - row * this.m_nCellSize;
            svector3.x = this.m_mapCenterPos.x - this.m_nHalfWidth + column * this.m_nCellSize;
            svector3.y = 0;
            return svector3;
        }

        public int GetWorldPosHeight(SVector3 sLogicPos)
        {
            Cell cellByWorldPos = this.GetCellByWorldPos(sLogicPos);
            if (cellByWorldPos == null)
                return sLogicPos.y;
            int rowIndex = (int)cellByWorldPos.rowIndex;
            int columnIndex = (int)cellByWorldPos.columnIndex;
            int num1 = this.m_nCellSize / 2;
            SVector3 svector3_1;
            svector3_1.x = cellByWorldPos.centerPos.x - num1;
            svector3_1.y = cellByWorldPos.centerPos.y;
            svector3_1.z = cellByWorldPos.centerPos.z + num1;
            SVector3 svector3_2;
            svector3_2.x = cellByWorldPos.centerPos.x + num1;
            if (this[rowIndex + 1, columnIndex + 1] == null)
                return sLogicPos.y;
            svector3_2.y = this[rowIndex + 1, columnIndex + 1].centerPos.y;
            svector3_2.z = cellByWorldPos.centerPos.z - num1;
            SVector3 svector3_3;
            long num2;
            long num3;
            if (sLogicPos.x - svector3_1.x > svector3_1.z - sLogicPos.z)
            {
                svector3_3.x = cellByWorldPos.centerPos.x + num1;
                if (this[rowIndex, columnIndex + 1] == null)
                    return sLogicPos.y;
                svector3_3.y = this[rowIndex, columnIndex + 1].centerPos.y;
                svector3_3.z = cellByWorldPos.centerPos.z + num1;
                num2 = 1000L * (long)(svector3_3.z - sLogicPos.z) / (long)this.m_nCellSize;
                num3 = 1000L * (long)(svector3_3.x - sLogicPos.x) / (long)this.m_nCellSize;
            }
            else
            {
                svector3_3.x = cellByWorldPos.centerPos.x - num1;
                if (this[rowIndex + 1, columnIndex] == null)
                    return sLogicPos.y;
                svector3_3.y = this[rowIndex + 1, columnIndex].centerPos.y;
                svector3_3.z = cellByWorldPos.centerPos.z - num1;
                num2 = 1000L * (long)(sLogicPos.x - svector3_3.x) / (long)this.m_nCellSize;
                num3 = 1000L * (long)(sLogicPos.z - svector3_3.z) / (long)this.m_nCellSize;
            }

            return (int)(((1000L - num3 - num2) * (long)svector3_3.y + num3 * (long)svector3_1.y +
                          num2 * (long)svector3_2.y) / 1000L);
        }

        public SVector3 GetMapPos(SVector3 sPos)
        {
            SVector3 svector3;
            svector3.y = (int)(1000L * (long)(sPos.x + this.m_nHalfWidth - this.m_mapCenterPos.x) /
                               (long)this.m_nCellSize);
            svector3.x = (int)(1000L * (long)(this.m_mapCenterPos.y + this.m_nHalfHeight - sPos.z) /
                               (long)this.m_nCellSize);
            svector3.z = 0;
            return svector3;
        }

        public void LoadMapFile(MapData mapData)
        {
            this.m_nCellSize = mapData.CellSize * 1000; // Cell大小
            this.m_nRowCount = mapData.RowCount; // 地图行数
            this.m_nColumnCount = mapData.ColumnCount; // 地图列数
            this.m_mapCenterPos.x = 0; // 地图中心点x
            this.m_mapCenterPos.y = 0; // 地图中心点y

            this.m_nHeight = this.m_nCellSize * this.m_nRowCount; // 地图长度
            this.m_nHalfHeight = this.m_nHeight / 2; // 地图半长度
            this.m_nWidth = this.m_nCellSize * this.m_nColumnCount; // 地图宽度
            this.m_nHalfWidth = this.m_nWidth / 2; // 地图半宽度
            int num3 = this.m_mapCenterPos.x + (-this.m_nHalfWidth + this.m_nCellSize / 2);
            int num4 = this.m_mapCenterPos.y + (this.m_nHalfHeight - this.m_nCellSize / 2);
            this.mCellList = new List<Cell>(this.rowCount * this.columnCount);

            for (int index1 = 0; index1 < this.rowCount; ++index1)
            {
                int z = num4 - index1 * this.cellSize;
                for (int index2 = 0; index2 < this.columnCount; ++index2)
                {
                    SVector3 centerPos = new SVector3(num3 + index2 * this.cellSize, 0, z);
                    centerPos.y = 0; // 该Cell的高度
                    byte flag = 0; // 碰撞物体类型 {0,1}
                    Cell cell = new Cell(centerPos, flag);
                    cell.rowIndex = (short)index1; // 第几行索引
                    cell.columnIndex = (short)index2; // 第几列索引
                    this.mCellList.Add(cell);
                }
            }

            if (mapData.CellList != null && mapData.CellList.Count > 0)
            {
                int count = mapData.CellList.Count;
                for (int i = 0; i < count; i++)
                {
                    var cellData = mapData.CellList[i];
                    var mapCell = this[cellData.x, cellData.z];
                    mapCell.mFlagValue = (byte)cellData.Obstacle;
                }
            }
        }

        public bool IsValidateCell(int nRow, int nColumn)
        {
            return nRow >= 0 && nRow < this.rowCount && nColumn >= 0 && nColumn < this.columnCount;
        }

        public bool IsReachable(SVector3 sPos, byte mask = 255)
        {
            Cell cellByWorldPos = this.GetCellByWorldPos(sPos);
            if (cellByWorldPos == null)
                return false;
            return cellByWorldPos.Walkable(mask);
        }
    }
}