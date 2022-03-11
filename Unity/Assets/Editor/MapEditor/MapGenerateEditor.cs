using System;
using System.Collections.Generic;
using System.IO;
using ET;
using UnityEditor;
using UnityEngine;

namespace ETEditor
{
    [CustomEditor(typeof (MapGenerate))]
    public class MapGenerateEditor: Editor
    {
        private MapGenerate mapGenerate;

        private void OnEnable()
        {
            mapGenerate = (MapGenerate)target;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("重新计算地图"))
            {
                if (this.mapGenerate == null)
                {
                    return;
                }

                this.mapGenerate.CaluMap();
            }

            if (GUILayout.Button("导出地图数据"))
            {
                ExportNodeData();
            }

            GUILayout.EndVertical();

            base.OnInspectorGUI();
        }

        private void ExportNodeData()
        {
            if (mapGenerate == null)
                return;
            string path = EditorUtility.SaveFilePanel("Save Graphs", "", "mapconfig.bytes", "bytes");
            if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path))
                return;
            try
            {
                var mapData = new MapData();
                mapData.RowCount = this.mapGenerate.Row;
                mapData.ColumnCount = this.mapGenerate.Column;
                mapData.CellSize = this.mapGenerate.CellSize;
                mapData.CellList = new List<CellData>();
                var arrNodes = this.mapGenerate.GetNodeData();
                foreach (NodeData nodeData in arrNodes)
                {
                    CellData cellData = new CellData();
                    cellData.x = nodeData.X;
                    cellData.z = nodeData.Z;
                    cellData.Obstacle = nodeData.TileType;
                    mapData.CellList.Add(cellData);
                }

                var s = JsonHelper.ToJson(mapData);
                var bytes = s.ToByteArray();
                var fileStream = new FileStream(path, FileMode.Create);
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Dispose();
                AssetDatabase.Refresh();

                Debug.Log("导出地图数据成功");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    public class MapData
    {
        public int Id; //地图编号
        public int CellSize; // 地图格子大小
        public int RowCount; // 地图多少行
        public int ColumnCount; // 地图多少列

        public List<CellData> CellList; // 障碍物细胞数据
    }

    public class CellData
    {
        public int x;
        public int z;
        public int Obstacle;
    }
}