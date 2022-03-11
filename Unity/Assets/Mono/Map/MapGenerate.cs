using System;
using UnityEngine;

namespace ET
{
    public class MapGenerate: MonoBehaviour
    {
        public int CellSize;
        public int Row;
        public int Column;

        public Vector3 CenterPos;

        private NodeData[,] arrNodes = null;

        private int lastRow;
        private int lastColumn;

        public NodeData[,] GetNodeData()
        {
            return this.arrNodes;
        }

        void Start()
        {
            this.lastRow = Row;
            this.lastColumn = Column;
            this.CaluMap();
        }

        void OnEnable()
        {
            this.CaluMap();
        }

        public void CaluMap()
        {
            int nRow = this.lastRow;
            int nColumn = this.lastColumn;
            int nCellSize = this.CellSize;

            arrNodes = new NodeData[nRow, nColumn];
            Vector3 centerPos = this.CenterPos;
            float halfWidth = nColumn / 2f * nCellSize;
            float halfHeight = nRow / 2f * nCellSize;
            float halfSize = nCellSize / 2f;
            Vector3 leftTopPos = centerPos - new Vector3(halfWidth, 0, -halfHeight);
            for (int i = 0; i < nRow; i++)
            {
                for (int j = 0; j < nColumn; j++)
                {
                    Vector3 nodePos = new Vector3(leftTopPos.x + (j + 0.5f) * nCellSize, 0f, leftTopPos.z - (i + 0.5f) * nCellSize);
                    var nodeData = new NodeData();
                    nodeData.X = i;
                    nodeData.Z = j;
                    nodeData.Index = i * nColumn + j;
                    Vector3 rayStartPos = new Vector3(nodePos.x, this.CenterPos.y, nodePos.z);
                    bool bObstacle = Physics.CheckCapsule(rayStartPos, rayStartPos - Vector3.up * 500, halfSize, LayerMask.GetMask("Obstacle"));
                    // Debug.DrawLine(rayStartPos, rayStartPos - Vector3.up * 500, Color.red, 10f);
                    nodeData.TileType = bObstacle? 1 : 0;
                    arrNodes[i, j] = nodeData;
                }
            }
        }

        void OnDrawGizmos()
        {
            if (this.lastRow != this.Row || this.lastColumn != this.Column || this.arrNodes == null)
            {
                this.lastRow = Row;
                this.lastColumn = Column;
                this.CaluMap();
            }

            int nRow = this.lastRow;
            int nColumn = this.lastColumn;
            int nCellSize = this.CellSize;

            int rowLineCount = nRow + 1;
            int columnLineCount = nColumn + 1;

            Vector3 centerPos = this.CenterPos;

            float halfWidth = nColumn / 2f * nCellSize;
            float halfHeight = nRow / 2f * nCellSize;
            Vector3 leftTopPos = centerPos + new Vector3(-halfWidth, 0, halfHeight);
            Vector3 rightBottomPos = centerPos + new Vector3(halfWidth, 0, -halfHeight);

            for (int i = 0; i < rowLineCount; i++)
            {
                float rowLinePosZ = leftTopPos.z - i * nCellSize;
                Vector3 leftStartPos = new Vector3(leftTopPos.x, centerPos.y, rowLinePosZ);
                Vector3 rightEndPos = new Vector3(rightBottomPos.x, centerPos.y, rowLinePosZ);
                // 从最左边连向最右边.
                Gizmos.DrawLine(leftStartPos, rightEndPos);
            }

            for (int j = 0; j < columnLineCount; j++)
            {
                float columnLinePosX = leftTopPos.x + j * nCellSize;
                Vector3 topStartPos = new Vector3(columnLinePosX, centerPos.y, leftTopPos.z);
                Vector3 bottomEndPos = new Vector3(columnLinePosX, centerPos.y, rightBottomPos.z);

                Gizmos.DrawLine(topStartPos, bottomEndPos);
            }

            for (int i = 0; i < nRow; i++)
            {
                for (int j = 0; j < nColumn; j++)
                {
                    var nodeData = this.arrNodes[i, j];
                    if (nodeData.TileType <= 0)
                    {
                        continue;
                    }

                    Vector3 nodePos = new Vector3(leftTopPos.x + (j + 0.5f) * nCellSize, 0f, leftTopPos.z - (i + 0.5f) * nCellSize);
                    Gizmos.DrawCube(new Vector3(nodePos.x, centerPos.y, nodePos.z),
                        new Vector3(nCellSize / 3f, 0.2f, nCellSize / 3f));
                }
            }
        }
    }

    [Serializable]
    public class NodeData
    {
        public int Index { get; set; }
        
        public int X { get; set; }
        public int Z { get; set; }
        public int TileType { get; set; }
    }
}