using UnityEditor;
using UnityEngine;
#if ODIN_INSPECTOR
using System.IO;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
#endif

namespace XNodeEditor {
    /// <summary> Override graph inspector to show an 'Open Graph' button at the top </summary>
    [CustomEditor(typeof(NodeGraphProxy), true)]
#if ODIN_INSPECTOR
    public class GlobalGraphEditor : OdinEditor
    {
        private NodeGraphProxy _nodeGraphProxy;

        protected override void OnEnable()
        {
            base.OnEnable();

            _nodeGraphProxy = target as NodeGraphProxy;
        }
        public override void OnInspectorGUI() {
            if (GUILayout.Button("Edit graph", GUILayout.Height(40)))
            {
                NodeEditorWindow.Open(serializedObject.targetObject as NodeGraphProxy);
            }

            if (GUILayout.Button("Export Json", GUILayout.Height(30)))
            {
                if (_nodeGraphProxy == null)
                {
                    return;
                }

                if (_nodeGraphProxy.textAsset == null)
                {
                    return;
                }

                var bytes = SerializationUtility.SerializeValue(_nodeGraphProxy.graph, DataFormat.JSON);
                var assetPath = AssetDatabase.GetAssetPath(_nodeGraphProxy.textAsset);
                File.WriteAllBytes(assetPath, bytes);

                AssetDatabase.Refresh();
                Debug.Log("导出Graph.Json成功!");
            }
            base.OnInspectorGUI();
        }
    }
#else
    public class GlobalGraphEditor : Editor {
        public override void OnInspectorGUI() {
            serializedObject.Update();

            if (GUILayout.Button("Edit graph", GUILayout.Height(40)))
            {
                NodeGraphProxy nodeGraphProxy = serializedObject.targetObject as NodeGraphProxy;
                NodeEditorWindow.Open(nodeGraphProxy);
            }

            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            GUILayout.Label("Raw data", "BoldLabel");

            DrawDefaultInspector();

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif

    [CustomEditor(typeof(NodeProxy), true)]
#if ODIN_INSPECTOR
    public class GlobalNodeEditor : OdinEditor {
        public override void OnInspectorGUI() {
            // TODO 和节点面板上显示的一样, 节点扩展支持折叠功能.
            // if (GUILayout.Button("Edit graph", GUILayout.Height(40))) {
            //     SerializedProperty graphProp = serializedObject.FindProperty("graph");
            //     NodeEditorWindow w = NodeEditorWindow.Open(graphProp.objectReferenceValue as NodeGraphProxy);
            //     w.Home(); // Focus selected node
            // }
            // base.OnInspectorGUI();
        }
    }
#else
    public class GlobalNodeEditor : Editor {
        public override void OnInspectorGUI() {
            serializedObject.Update();

            if (GUILayout.Button("Edit graph", GUILayout.Height(40))) {
                // SerializedProperty graphProp = serializedObject.FindProperty("graph");
                // var nodeGraphBehavior = graphProp.objectReferenceValue as XNode.NodeGraphBehavior;
                // // 为其bind一个Proxy
                // NodeGraphProxy nodeGraphProxy = nodeGraphBehavior.NodeGraph.nodeGraphProxy();
                // // NodeEditorWindow w = NodeEditorWindow.Open(nodeGraphBehavior.NodeGraph);
                // NodeEditorWindow w = NodeEditorWindow.Open(nodeGraphProxy);
                // w.Home(); // Focus selected node
            }

            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            GUILayout.Label("Raw data", "BoldLabel");

            // Now draw the node itself.
            DrawDefaultInspector();

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}