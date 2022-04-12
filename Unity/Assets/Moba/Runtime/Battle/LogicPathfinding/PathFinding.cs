using System;
using System.Collections.Generic;

namespace Scarf.Moba
{
    public class PathFinding
    {
        private Cell[] m_arrPassCell = new Cell[4];

        // public const int OBLIQUE = 14;
        // public const int STEP = 10;
        private static int m_nSession;
        private Map m_cMap;
        private BinaryHeap m_cBinaryHeap;
        private List<Cell> m_lstPath;

        public PathFinding(Map cMap)
        {
            this.m_cMap = cMap;
            this.m_cBinaryHeap = new BinaryHeap(this.m_cMap.rowCount * this.m_cMap.columnCount / 2); // 创建 地图{行x列}一半的大小
            this.m_lstPath = new List<Cell>(Math.Max(this.m_cMap.rowCount, this.m_cMap.columnCount) << 1);
        }

        /// <summary>
        /// 寻路
        /// </summary>
        public bool CalculatePath(
            SVector3 sCurPos,
            SVector3 sTargetPos,
            List<SVector3> lstPath,
            int nDis,
            byte mask = 255)
        {
            return this.CalculatePathImpl(sCurPos, sTargetPos, lstPath, nDis, mask);
        }

        private bool CalculatePathImpl(
            SVector3 sCurPos,
            SVector3 sTargetPos,
            List<SVector3> lstPath,
            int nDis,
            byte mask)
        {
            if (lstPath == null)
                return false;
            lstPath.Clear();
            SVector3 curPosition = sCurPos;
            Cell cellByWorldPos1 = this.m_cMap.GetCellByWorldPos(curPosition);
            Cell cellByWorldPos2 = this.m_cMap.GetCellByWorldPos(sTargetPos);
            if (cellByWorldPos1 == null || cellByWorldPos2 == null ||
                cellByWorldPos2 != cellByWorldPos1 && !cellByWorldPos2.Walkable(mask) && nDis <= 0)
                return false;
            if (this.GetBarrier(curPosition, sTargetPos, mask) == null) // 之间没有障碍物, 直线移动过去
            {
                lstPath.Add(curPosition);
                lstPath.Add(sTargetPos);
                return true;
            }

            Cell cell = this.FindPath(curPosition, sTargetPos, nDis, mask);
            if (cell == null)
                return false;
            this.m_lstPath.Clear();
            for (; cell != null; cell = cell.parent)
                this.m_lstPath.Insert(0, cell);
            this.SmoothPath(mask);
            for (int index = 0; index != this.m_lstPath.Count; ++index)
                lstPath.Add(this.m_lstPath[index].centerPos);
            if (lstPath.Count > 0)
            {
                if (lstPath[0] != curPosition)
                {
                    if (lstPath.Count > 1 && this.GetBarrier(curPosition, lstPath[1], mask) != null && nDis <= 0)
                        lstPath.Insert(0, curPosition);
                    else
                        lstPath[0] = curPosition;
                }

                if (lstPath[lstPath.Count - 1] != sTargetPos && nDis <= 0)
                {
                    if (lstPath.Count > 1 && this.GetBarrier(lstPath[lstPath.Count - 2], sTargetPos, mask) != null)
                        lstPath.Add(sTargetPos);
                    else
                        lstPath[lstPath.Count - 1] = sTargetPos;
                }
            }

            if (lstPath.Count <= 1)
                lstPath.Insert(0, curPosition);
            return true;
        }

        /// <summary>
        /// 获得可到达的位置 
        /// </summary>
        public bool GetReachablePosition(
            SVector3 sStartPos,
            ref SVector3 posTarget,
            bool bAlongObstacle,
            byte mask = 255)
        {
            return this.GetReachablePositionImpl(sStartPos, ref posTarget, bAlongObstacle, mask);
        }

        private bool GetReachablePositionImpl(
            SVector3 sStartPos,
            ref SVector3 posTarget,
            bool bAlongObstacle,
            byte mask)
        {
            SVector3 sOffset = posTarget - sStartPos;
            if (this.m_cMap.GetCellByWorldPos(sStartPos) == null)
            {
                posTarget = sStartPos;
                return false;
            }

            sOffset.y = 0;
            if (this.m_cMap.GetCellByWorldPos(posTarget) == null)
            {
                posTarget = sStartPos;
                return false;
            }

            SVector3 svector3_3 = sStartPos;
            SVector3 svector3_1 = sStartPos;
            Cell barrier = this.GetBarrier(sStartPos, posTarget, mask);
            bool reachablePositionImpl = false;
            if (barrier != null)
            {
                reachablePositionImpl = true;
                int num1 = barrier.CheckIntersect(svector3_1, sOffset, this.m_cMap.cellSize);
                if (num1 > 20)
                    svector3_1 += sOffset.normalized * (num1 - 20) / 1000;
                if (bAlongObstacle)
                {
                    sStartPos = svector3_1;
                    SVector3 svector3_2 = posTarget - sStartPos;
                    int num2 = 0;
                    int num3 = 0;
                    if (CMath.Abs(svector3_2.z) > CMath.Abs(svector3_2.x))
                    {
                        num2 = this.GetAxisOffsetDis(sStartPos, svector3_2.z, false, mask);
                        if (num2 == 0)
                            num3 = this.GetAxisOffsetDis(sStartPos, svector3_2.x, true, mask);
                    }
                    else
                    {
                        num3 = this.GetAxisOffsetDis(sStartPos, svector3_2.x, true, mask);
                        if (num3 == 0)
                            num2 = this.GetAxisOffsetDis(sStartPos, svector3_2.z, false, mask);
                    }

                    if (num2 == 0 && num3 == 0)
                    {
                        bool bDirZ = sOffset.z != 0; // 是否是z方向.
                        bool bDir = bDirZ ? sOffset.z > 0 : sOffset.x > 0; // 正符号

                        const int LimitCount = 10;
                        int offsetX = CMath.Abs(sOffset.x);
                        int offsetZ = CMath.Abs(sOffset.z);
                        // 朝上
                        if (bDirZ && bDir)
                        {
                            int leftCount = 0;
                            while (leftCount < LimitCount)
                            {
                                var cell1 = this.m_cMap[barrier.rowIndex + 1, barrier.columnIndex - leftCount - 1];
                                if (cell1 == null || !cell1.Walkable(mask))
                                {
                                    leftCount = LimitCount;
                                    break;
                                }

                                ++leftCount;
                                var cell2 = this.m_cMap[barrier.rowIndex, barrier.columnIndex - leftCount - 1];
                                if (cell2 != null && cell2.Walkable(mask))
                                {
                                    break;
                                }
                            }

                            int rightCount = 0;
                            while (rightCount < LimitCount)
                            {
                                var cell1 = this.m_cMap[barrier.rowIndex + 1, barrier.columnIndex + rightCount + 1];
                                if (cell1 == null || !cell1.Walkable(mask))
                                {
                                    rightCount = LimitCount;
                                    break;
                                }

                                ++rightCount;
                                var cell2 = this.m_cMap[barrier.rowIndex, barrier.columnIndex + rightCount + 1];
                                if (cell2 != null && cell2.Walkable(mask))
                                {
                                    break;
                                }
                            }

                            if (leftCount < 10 || rightCount < 10)
                            {
                                num3 = leftCount < rightCount ? -offsetZ : offsetZ;
                            }
                        }
                        // 朝下
                        else if (bDirZ && !bDir)
                        {
                            int leftCount = 0;
                            while (leftCount < LimitCount)
                            {
                                var cell1 = this.m_cMap[barrier.rowIndex - 1, barrier.columnIndex - leftCount - 1];
                                if (cell1 == null || !cell1.Walkable(mask))
                                {
                                    leftCount = LimitCount;
                                    break;
                                }

                                ++leftCount;
                                var cell2 = this.m_cMap[barrier.rowIndex, barrier.columnIndex - leftCount - 1];
                                if (cell2 != null && cell2.Walkable(mask))
                                {
                                    break;
                                }
                            }

                            int rightCount = 0;
                            while (rightCount < LimitCount)
                            {
                                var cell1 = this.m_cMap[barrier.rowIndex - 1, barrier.columnIndex + rightCount + 1];
                                if (cell1 == null || !cell1.Walkable(mask))
                                {
                                    rightCount = LimitCount;
                                    break;
                                }

                                ++rightCount;
                                var cell2 = this.m_cMap[barrier.rowIndex, barrier.columnIndex + rightCount + 1];
                                if (cell2 != null && cell2.Walkable(mask))
                                {
                                    break;
                                }
                            }

                            if (leftCount < 10 || rightCount < 10)
                            {
                                num3 = leftCount < rightCount ? -offsetZ : offsetZ;
                            }
                        }
                        // 朝右
                        else if (!bDirZ && bDir)
                        {
                            int leftCount = 0;
                            while (leftCount < LimitCount)
                            {
                                var cell1 = this.m_cMap[barrier.rowIndex - leftCount - 1, barrier.columnIndex - 1];
                                if (cell1 == null || !cell1.Walkable(mask))
                                {
                                    leftCount = LimitCount;
                                    break;
                                }

                                ++leftCount;
                                var cell2 = this.m_cMap[barrier.rowIndex - leftCount - 1, barrier.columnIndex];
                                if (cell2 != null && cell2.Walkable(mask))
                                {
                                    break;
                                }
                            }

                            int rightCount = 0;
                            while (rightCount < LimitCount)
                            {
                                var cell1 = this.m_cMap[barrier.rowIndex + rightCount + 1, barrier.columnIndex - 1];
                                if (cell1 == null || !cell1.Walkable(mask))
                                {
                                    rightCount = LimitCount;
                                    break;
                                }

                                ++rightCount;
                                var cell2 = this.m_cMap[barrier.rowIndex + rightCount + 1, barrier.columnIndex];
                                if (cell2 != null && cell2.Walkable(mask))
                                {
                                    break;
                                }
                            }

                            if (leftCount < 10 || rightCount < 10)
                            {
                                num2 = leftCount < rightCount ? offsetX : -offsetX;
                            }
                        }
                        // 朝左
                        else if (!bDirZ && !bDir)
                        {
                            int leftCount = 0;
                            while (leftCount < LimitCount)
                            {
                                var cell1 = this.m_cMap[barrier.rowIndex - leftCount - 1, barrier.columnIndex + 1];
                                if (cell1 == null || !cell1.Walkable(mask))
                                {
                                    leftCount = LimitCount;
                                    break;
                                }

                                ++leftCount;
                                var cell2 = this.m_cMap[barrier.rowIndex - leftCount - 1, barrier.columnIndex];
                                if (cell2 != null && cell2.Walkable(mask))
                                {
                                    break;
                                }
                            }

                            int rightCount = 0;
                            while (rightCount < LimitCount)
                            {
                                var cell1 = this.m_cMap[barrier.rowIndex + rightCount + 1, barrier.columnIndex + 1];
                                if (cell1 == null || !cell1.Walkable(mask))
                                {
                                    rightCount = LimitCount;
                                    break;
                                }

                                ++rightCount;
                                var cell2 = this.m_cMap[barrier.rowIndex + rightCount + 1, barrier.columnIndex];
                                if (cell2 != null && cell2.Walkable(mask))
                                {
                                    break;
                                }
                            }

                            if (leftCount < 10 || rightCount < 10)
                            {
                                num2 = leftCount < rightCount ? offsetX : -offsetX;
                            }
                        }

                        #region 暂时屏蔽

                        // bool bRegionX = sStartPos.x > barrier.centerPos.x;
                        // bool bRegionZ = sStartPos.z > barrier.centerPos.z;
                        // bool bDirZ = sOffset.z != 0; // 是否是z方向.
                        //
                        // UnityEngine.Debug.Log($"[{bDirZ},{bRegionX},{bRegionZ}]");
                        // if (bDirZ && bRegionX && bRegionZ)
                        // {
                        //     var cell1 = this.m_cMap[barrier.rowIndex - 1, barrier.columnIndex + 1];
                        //     if (cell1 != null && cell1.Walkable(mask))
                        //     {
                        //         var cell2 = this.m_cMap[barrier.rowIndex, barrier.columnIndex + 1];
                        //         if (cell2 != null && cell2.Walkable(mask))
                        //         {
                        //             num3 = CMath.Abs(sOffset.z) / 2;
                        //         }
                        //     }
                        // }
                        // else if (bDirZ && bRegionX && !bRegionZ)
                        // {
                        //     var cell1 = this.m_cMap[barrier.rowIndex + 1, barrier.columnIndex + 1];
                        //     if (cell1 != null && cell1.Walkable(mask))
                        //     {
                        //         var cell2 = this.m_cMap[barrier.rowIndex, barrier.columnIndex + 1];
                        //         if (cell2 != null && cell2.Walkable(mask))
                        //         {
                        //             num3 = CMath.Abs(sOffset.z) / 2;
                        //         }
                        //     }
                        // }
                        // else if (bDirZ && !bRegionX && bRegionZ)
                        // {
                        //     var cell1 = this.m_cMap[barrier.rowIndex - 1, barrier.columnIndex - 1];
                        //     if (cell1 != null && cell1.Walkable(mask))
                        //     {
                        //         var cell2 = this.m_cMap[barrier.rowIndex, barrier.columnIndex - 1];
                        //         if (cell2 != null && cell2.Walkable(mask))
                        //         {
                        //             num3 = -CMath.Abs(sOffset.z) / 2;
                        //         }
                        //     }
                        // }
                        // else if (bDirZ && !bRegionX && !bRegionZ)
                        // {
                        //     var cell1 = this.m_cMap[barrier.rowIndex + 1, barrier.columnIndex - 1];
                        //     if (cell1 != null && cell1.Walkable(mask))
                        //     {
                        //         var cell2 = this.m_cMap[barrier.rowIndex, barrier.columnIndex - 1];
                        //         if (cell2 != null && cell2.Walkable(mask))
                        //         {
                        //             num3 = -CMath.Abs(sOffset.z) / 2;
                        //         }
                        //     }
                        // }
                        // // 移动方向
                        // else if (!bDirZ && bRegionX && bRegionZ)
                        // {
                        //     var cell1 = this.m_cMap[barrier.rowIndex - 1, barrier.columnIndex + 1];
                        //     if (cell1 != null && cell1.Walkable(mask))
                        //     {
                        //         var cell2 = this.m_cMap[barrier.rowIndex - 1, barrier.columnIndex];
                        //         if (cell2 != null && cell2.Walkable(mask))
                        //         {
                        //             num2 = CMath.Abs(sOffset.x) / 2;
                        //         }
                        //     }
                        // }
                        // else if (!bDirZ && bRegionX && !bRegionZ)
                        // {
                        //     var cell1 = this.m_cMap[barrier.rowIndex + 1, barrier.columnIndex + 1];
                        //     if (cell1 != null && cell1.Walkable(mask))
                        //     {
                        //         var cell2 = this.m_cMap[barrier.rowIndex + 1, barrier.columnIndex];
                        //         if (cell2 != null && cell2.Walkable(mask))
                        //         {
                        //             num2 = -CMath.Abs(sOffset.x) / 2;
                        //         }
                        //     }
                        // }
                        // else if (!bDirZ && !bRegionX && bRegionZ)
                        // {
                        //     var cell1 = this.m_cMap[barrier.rowIndex - 1, barrier.columnIndex - 1];
                        //     if (cell1 != null && cell1.Walkable(mask))
                        //     {
                        //         var cell2 = this.m_cMap[barrier.rowIndex - 1, barrier.columnIndex];
                        //         if (cell2 != null && cell2.Walkable(mask))
                        //         {
                        //             num2 = CMath.Abs(sOffset.x) / 2;
                        //         }
                        //     }
                        // }
                        // else if (!bDirZ && !bRegionX && !bRegionZ)
                        // {
                        //     var cell1 = this.m_cMap[barrier.rowIndex + 1, barrier.columnIndex - 1];
                        //     if (cell1 != null && cell1.Walkable(mask))
                        //     {
                        //         var cell2 = this.m_cMap[barrier.rowIndex + 1, barrier.columnIndex];
                        //         if (cell2 != null && cell2.Walkable(mask))
                        //         {
                        //             num2 = -CMath.Abs(sOffset.x) / 2;
                        //         }
                        //     }
                        // }

                        #endregion
                    }

                    svector3_1.x += num3;
                    svector3_1.z += num2;
                }
            }
            else
                svector3_1 += sOffset;

            Cell cellByWorldPos = this.m_cMap.GetCellByWorldPos(svector3_1);
            svector3_1.y = cellByWorldPos.centerPos.y;
            posTarget = svector3_1;
            return reachablePositionImpl;
        }

        /// <summary>
        /// 位置采样 
        /// </summary>
        public bool SamplePosition(ref SVector3 sPos, int nDis, byte mask = 255)
        {
            return this.SamplePositionImpl(ref sPos, nDis, mask);
        }

        private bool SamplePositionImpl(ref SVector3 sPos, int nDis, byte mask)
        {
            Cell cellByWorldPos = this.m_cMap.GetCellByWorldPos(sPos);
            if (cellByWorldPos == null)
                return false;
            if (cellByWorldPos.Walkable(mask))
                return true;
            SVector3 sCircleCenter = sPos;
            int num1 = 2 * nDis / this.m_cMap.cellSize;
            SVector3 mapPos = this.m_cMap.GetMapPos(sPos);
            int[] numArray1 = new int[4] { 0, 1, 0, -1 };
            int[] numArray2 = new int[4] { 1, 0, -1, 0 };
            int num2 = this.m_cMap.cellSize / 2;
            mapPos.x /= 1000;
            mapPos.y /= 1000;
            for (int index1 = 1; index1 <= num1; index1 += 2)
            {
                for (int index2 = 0; index2 != 4; ++index2)
                {
                    for (int index3 = 0; index3 <= index1; ++index3)
                    {
                        Cell c = this.m_cMap[mapPos.x, mapPos.y];
                        if (c != null && c.Walkable(mask) &&
                            CPhysics.CheckRectangleAndCircle(c.centerPos, SVector3.forward, num2, num2, sCircleCenter,
                                nDis))
                        {
                            SVector3 sOffset = sCircleCenter - c.centerPos;
                            int num3 = c.CheckIntersect(c.centerPos, sOffset, this.m_cMap.cellSize);
                            if (num3 >= 0)
                            {
                                sPos = c.centerPos + sOffset.normalized * (num3 - 20) / 1000;
                                return true;
                            }
                        }

                        mapPos.x += numArray1[index2];
                        mapPos.y += numArray2[index2];
                    }

                    mapPos.x -= numArray1[index2];
                    mapPos.y -= numArray2[index2];
                }

                --mapPos.x;
                --mapPos.y;
            }

            return false;
        }

        public Cell GetBarrier(SVector3 sStartPos, SVector3 sTargetPos, byte mask)
        {
            Cell cellByWorldPos1 = this.m_cMap.GetCellByWorldPos(sStartPos);
            Cell cellByWorldPos2 = this.m_cMap.GetCellByWorldPos(sTargetPos);
            if (cellByWorldPos2 == null)
                return (Cell)null;
            int rowIndex1 = (int)cellByWorldPos1.rowIndex;
            int columnIndex1 = (int)cellByWorldPos1.columnIndex;
            int rowIndex2 = (int)cellByWorldPos2.rowIndex;
            int columnIndex2 = (int)cellByWorldPos2.columnIndex;
            if (cellByWorldPos1 == cellByWorldPos2)
                return (Cell)null;
            SVector3 mapPos1 = this.m_cMap.GetMapPos(sStartPos);
            SVector3 mapPos2 = this.m_cMap.GetMapPos(sTargetPos);
            int nDir = -1;
            if ((rowIndex2 <= rowIndex1 ? rowIndex1 - rowIndex2 : rowIndex2 - rowIndex1) >
                (columnIndex2 <= columnIndex1 ? columnIndex1 - columnIndex2 : columnIndex2 - columnIndex1))
            {
                if (mapPos1.x == mapPos2.x)
                    return !cellByWorldPos2.Walkable(mask) ? cellByWorldPos2 : (Cell)null;
                int pos = rowIndex1;
                int num = rowIndex2;
                if (rowIndex2 >= rowIndex1)
                {
                    nDir = 1;
                    ++pos;
                    ++num;
                }

                for (; pos != num; pos += nDir)
                {
                    int yPos = this.LineFunc(mapPos1, mapPos2, 0, pos);
                    int nodesUnderPoint = this.GetNodesUnderPoint(1000 * pos, yPos, nDir, this.m_arrPassCell);
                    for (int index = 0; index < nodesUnderPoint; ++index)
                    {
                        Cell cell = this.m_arrPassCell[index];
                        if (!cell.Walkable(mask))
                            return cell;
                    }
                }
            }
            else
            {
                if (mapPos1.y == mapPos2.y)
                    return !cellByWorldPos2.Walkable(mask) ? cellByWorldPos2 : (Cell)null;
                int pos = columnIndex1;
                int num = columnIndex2;
                if (columnIndex2 >= columnIndex1)
                {
                    nDir = 1;
                    ++pos;
                    ++num;
                }

                for (; pos != num; pos += nDir)
                {
                    int nodesUnderPoint = this.GetNodesUnderPoint(this.LineFunc(mapPos1, mapPos2, 1, pos), 1000 * pos,
                        nDir,
                        this.m_arrPassCell);
                    for (int index = 0; index < nodesUnderPoint; ++index)
                    {
                        Cell cell = this.m_arrPassCell[index];
                        if (!cell.Walkable(mask))
                            return cell;
                    }
                }
            }

            return !cellByWorldPos2.Walkable(mask) ? cellByWorldPos2 : (Cell)null;
        }

        private Cell FindPath(
            SVector3 sStartPos,
            SVector3 sTargetPos,
            int nDis,
            byte mask)
        {
            Cell cellByWorldPos1 = this.m_cMap.GetCellByWorldPos(sStartPos);
            Cell cellByWorldPos2 = this.m_cMap.GetCellByWorldPos(sTargetPos);
            if (PathFinding.m_nSession > 100000000)
                PathFinding.m_nSession = 0;
            ++PathFinding.m_nSession;
            this.m_cBinaryHeap.Clear();
            this.m_cBinaryHeap.Add(cellByWorldPos1);
            long num1 = 3L * (sTargetPos - sStartPos).sqrMagnitudeXz;
            long num2 = (long)(nDis * nDis);
            Cell cell1 = cellByWorldPos1;
            cell1.Session = PathFinding.m_nSession;
            while (cell1 != null)
            {
                cell1 = this.m_cBinaryHeap.Remove();
                if (cell1 == null || cell1 == cellByWorldPos2 || /*cell1.lstUnit.Count <= 1 &&*/
                    cell1.Walkable(byte.MaxValue) &&
                    (cell1.centerPos - sTargetPos).sqrMagnitudeXz < num2)
                    return cell1;
                if ((sStartPos - cell1.centerPos).sqrMagnitudeXz > num1)
                    return (Cell)null;
                cell1.close = true;
                int num3 = cell1.rowIndex <= (short)0 ? 0 : (int)cell1.rowIndex - 1;
                int num4 = (int)cell1.rowIndex >= this.m_cMap.rowCount - 1
                    ? this.m_cMap.rowCount - 1
                    : (int)cell1.rowIndex + 1;
                int num5 = cell1.columnIndex <= (short)0 ? 0 : (int)cell1.columnIndex - 1;
                int num6 = (int)cell1.columnIndex >= this.m_cMap.columnCount - 1
                    ? this.m_cMap.columnCount - 1
                    : (int)cell1.columnIndex + 1;
                for (int x = num3; x <= num4; ++x)
                {
                    for (int y = num5; y <= num6; ++y)
                    {
                        if (this.CanReach(cell1, x, y, mask))
                        {
                            Cell cell2 = this.m_cMap[x, y];
                            if (cell2.m_nSession != PathFinding.m_nSession)
                            {
                                cell2.Session = PathFinding.m_nSession;
                                cell2.parent = cell1;
                                cell2.G = this.CalcG(cell1, cell2);
                                cell2.H = this.CalcH(cell2, cellByWorldPos2);
                                cell2.F = cell2.G + cell2.H;
                                this.m_cBinaryHeap.Add(cell2);
                            }
                            else
                            {
                                int num7 = this.CalcG(cell1, cell2);
                                if (num7 < cell2.G)
                                {
                                    cell2.parent = cell1;
                                    cell2.G = num7;
                                    cell2.F = cell2.G + cell2.H;
                                    this.m_cBinaryHeap.Rebuild(cell2);
                                }
                            }
                        }
                    }
                }
            }

            return (Cell)null;
        }

        private int CalcG(Cell cCurCell, Cell cNextCell)
        {
            return (((int)cCurCell.rowIndex <= (int)cNextCell.rowIndex
                    ? (int)cNextCell.rowIndex - (int)cCurCell.rowIndex
                    : (int)cCurCell.rowIndex - (int)cNextCell.rowIndex) +
                ((int)cCurCell.columnIndex <= (int)cNextCell.columnIndex
                    ? (int)cNextCell.columnIndex - (int)cCurCell.columnIndex
                    : (int)cCurCell.columnIndex - (int)cNextCell.columnIndex) != 2
                    ? 10
                    : 14) + (cCurCell == null ? 0 : cCurCell.G);
        }

        private int CalcH(Cell cNextCell, Cell cEndCell)
        {
            return (((int)cEndCell.rowIndex <= (int)cNextCell.rowIndex
                        ? (int)cNextCell.rowIndex - (int)cEndCell.rowIndex
                        : (int)cEndCell.rowIndex - (int)cNextCell.rowIndex) +
                    ((int)cEndCell.columnIndex <= (int)cNextCell.columnIndex
                        ? (int)cNextCell.columnIndex - (int)cEndCell.columnIndex
                        : (int)cEndCell.columnIndex - (int)cNextCell.columnIndex)) * 10;
        }

        private bool CanReach(Cell cCenter, int x, int y, byte mask)
        {
            Cell cell = this.m_cMap[x, y];
            if (cell != cCenter && !cell.Walkable(mask) || PathFinding.m_nSession == cell.m_nSession && cell.close)
                return false;
            if (x == (int)cCenter.rowIndex - 1)
            {
                if (y == (int)cCenter.columnIndex - 1)
                {
                    if (!this.m_cMap[(int)cCenter.rowIndex - 1, (int)cCenter.columnIndex].Walkable(mask) ||
                        !this.m_cMap[(int)cCenter.rowIndex, (int)cCenter.columnIndex - 1].Walkable(mask))
                        return false;
                }
                else if (y == (int)cCenter.columnIndex + 1 &&
                         (!this.m_cMap[(int)cCenter.rowIndex, (int)cCenter.columnIndex + 1].Walkable(mask) ||
                          !this.m_cMap[(int)cCenter.rowIndex - 1, (int)cCenter.columnIndex].Walkable(mask)))
                    return false;
            }
            else if (x == (int)cCenter.rowIndex + 1)
            {
                if (y == (int)cCenter.columnIndex - 1)
                {
                    if (!this.m_cMap[(int)cCenter.rowIndex, (int)cCenter.columnIndex - 1].Walkable(mask) ||
                        !this.m_cMap[(int)cCenter.rowIndex + 1, (int)cCenter.columnIndex].Walkable(mask))
                        return false;
                }
                else if (y == (int)cCenter.columnIndex + 1 &&
                         (!this.m_cMap[(int)cCenter.rowIndex + 1, (int)cCenter.columnIndex].Walkable(mask) ||
                          !this.m_cMap[(int)cCenter.rowIndex, (int)cCenter.columnIndex + 1].Walkable(mask)))
                    return false;
            }

            return true;
        }

        private void SmoothPath(byte mask)
        {
            int count1 = this.m_lstPath.Count;
            if (count1 > 2)
            {
                Cell cell1 = this.m_lstPath[count1 - 1];
                Cell cell2 = this.m_lstPath[count1 - 2];
                int num1 = (int)cell1.rowIndex - (int)cell2.rowIndex;
                int num2 = (int)cell1.columnIndex - (int)cell2.columnIndex;
                for (int index = this.m_lstPath.Count - 3; index >= 0; --index)
                {
                    Cell cell3 = this.m_lstPath[index + 1];
                    Cell cell4 = this.m_lstPath[index];
                    int num3 = (int)cell3.rowIndex - (int)cell4.rowIndex;
                    int num4 = (int)cell3.columnIndex - (int)cell4.columnIndex;
                    if (num1 == num3 && num2 == num4)
                    {
                        this.m_lstPath.RemoveAt(index + 1);
                    }
                    else
                    {
                        num1 = num3;
                        num2 = num4;
                    }
                }
            }

            for (int index1 = this.m_lstPath.Count - 1; index1 >= 0; --index1)
            {
                for (int index2 = 0; index2 <= index1 - 2; ++index2)
                {
                    if (this.GetBarrier(this.m_lstPath[index2].centerPos, this.m_lstPath[index1].centerPos, mask) ==
                        null)
                    {
                        for (int index3 = index1 - 1; index3 > index2; --index3)
                            this.m_lstPath.RemoveAt(index3);
                        index1 = index2 + 1;
                        int count2 = this.m_lstPath.Count;
                        break;
                    }
                }
            }
        }

        private int GetNodesUnderPoint(int xPos, int yPos, int nDir, Cell[] arrResult)
        {
            int num = 0;
            int index1 = xPos / 1000;
            int index2 = yPos / 1000;
            bool flag1 = xPos % 1000 == 0;
            bool flag2 = yPos % 1000 == 0;
            if (nDir < 0)
            {
                Cell c = this.m_cMap[index1, index2];
                if (c != null)
                    arrResult[num++] = c;
            }

            if (flag1 && flag2)
            {
                Cell c1 = this.m_cMap[index1, index2 - 1];
                if (c1 != null)
                    arrResult[num++] = c1;
                Cell c2 = this.m_cMap[index1 - 1, index2];
                if (c2 != null)
                    arrResult[num++] = c2;
                Cell c3 = this.m_cMap[index1 - 1, index2 - 1];
                if (c3 != null)
                    arrResult[num++] = c3;
            }
            else if (flag1 && !flag2)
            {
                Cell c = this.m_cMap[index1 - 1, index2];
                if (c != null)
                    arrResult[num++] = c;
            }
            else if (!flag1 && flag2)
            {
                Cell c = this.m_cMap[index1, index2 - 1];
                if (c != null)
                    arrResult[num++] = c;
            }

            if (nDir > 0)
            {
                Cell c = this.m_cMap[index1, index2];
                if (c != null)
                    arrResult[num++] = c;
            }

            return num;
        }

        private int LineFunc(SVector3 ponit1, SVector3 point2, int type, int pos)
        {
            if (ponit1.x == point2.x)
            {
                if (type == 0)
                {
                    // Debug.Log((object) "两点所确定直线垂直于y轴，不能根据x值得到y值");
                    return 0;
                }

                if (type == 1)
                    return ponit1.x;
            }
            else if (ponit1.y == point2.y)
            {
                if (type == 0)
                    return ponit1.y;
                if (type == 1)
                {
                    // Debug.Log((object) "两点所确定直线垂直于y轴，不能根据x值得到y值");
                    return 0;
                }
            }

            long num1 = 1000L * (long)(ponit1.y - point2.y) / (long)(ponit1.x - point2.x);
            long num2 = (long)ponit1.y - num1 * (long)ponit1.x / 1000L;
            if (type == 0)
                return (int)(num1 * (long)pos + num2);
            return type == 1 ? (int)(1000L * (1000L * (long)pos - num2) / num1) : 0;
        }

        private int GetAxisOffsetDis(
            SVector3 sStartPos,
            int nOffset,
            bool bIsXAxis,
            byte mask = 255)
        {
            SVector3 zero = SVector3.zero;
            SVector3 sTargetPos = sStartPos;
            if (bIsXAxis)
            {
                sTargetPos.x += nOffset;
                zero.x = nOffset;
            }
            else
            {
                sTargetPos.z += nOffset;
                zero.z = nOffset;
            }

            Cell barrier = this.GetBarrier(sStartPos, sTargetPos, mask);
            if (barrier == null)
                return nOffset;
            int num = barrier.CheckIntersect(sStartPos, zero, this.m_cMap.cellSize);
            return num > 20 ? CMath.Sign(nOffset) * (num - 20) : 0;
        }
    }
}