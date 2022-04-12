using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace XNodeEditor
{
    /// <summary> Deals with modified assets </summary>
    class NodeGraphImporter : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string path in importedAssets)
            {
                // Skip processing anything without the .asset extension
                if (Path.GetExtension(path) != ".asset") continue;

                // Get the object that is requested for deletion
                NodeGraphProxy graph = AssetDatabase.LoadAssetAtPath<NodeGraphProxy>(path);
                if (graph == null) continue;

                // Get attributes
                Type graphType = graph.GetGraphType();
                XNode.NodeGraph.RequireNodeAttribute[] attribs = Array.ConvertAll(
                    graphType.GetCustomAttributes(typeof(XNode.NodeGraph.RequireNodeAttribute), true),
                    x => x as XNode.NodeGraph.RequireNodeAttribute);

                Vector2 position = Vector2.zero;
                foreach (XNode.NodeGraph.RequireNodeAttribute attrib in attribs)
                {
                    if (attrib.type0 != null) AddRequired(graph, attrib.type0, ref position);
                    if (attrib.type1 != null) AddRequired(graph, attrib.type1, ref position);
                    if (attrib.type2 != null) AddRequired(graph, attrib.type2, ref position);
                }
            }
        }

        private static void AddRequired(NodeGraphProxy graphProxy, Type type, ref Vector2 position)
        {
            if (!graphProxy.NodeProxies.Any(x => x.GetNodeType() == type))
            {
                NodeProxy nodeProxy = graphProxy.AddNode(type);
                nodeProxy.position = position;
                position.x += 200;
                if (nodeProxy.name == null || nodeProxy.name.Trim() == "") nodeProxy.name = NodeEditorUtilities.NodeDefaultName(type);
                if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(graphProxy)))
                    AssetDatabase.AddObjectToAsset(nodeProxy, graphProxy);
            }
        }
    }
}