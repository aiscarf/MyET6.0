using Scarf.ANode.Flow.Runtime;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using XNode;

namespace XNodeEditor
{
    public class NodeValueDrawer : OdinValueDrawer<Node>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var value = this.ValueEntry.SmartValue;
            EditorGUILayout.LabelField("NodeValueDrawer", "OdinValueDrawer<Node>");
            this.ValueEntry.SmartValue = value;
        }
    }
    
    // public class FlowCheckEventNodeDrawer : OdinValueDrawer<FlowCheckEventNode>
    // {
    //     protected override void DrawPropertyLayout(GUIContent label)
    //     {
    //         var value = this.ValueEntry.SmartValue;
    //         
    //         this.ValueEntry.SmartValue = value;
    //     }
    // }
}