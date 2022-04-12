// using Sirenix.OdinInspector;
// using Sirenix.OdinInspector.Editor;
// using Sirenix.Utilities.Editor;
// using UnityEngine;
// using XNode;
//
// namespace XNodeEditor
// {
//     public class ControlPortAttributeDrawer : OdinAttributeDrawer<XNode.Node.ControlPortAttribute>
//     {
//         protected override bool CanDrawAttributeProperty(InspectorProperty property)
//         {
//             return true;
//             // var nodeProxy = property.Tree.WeakTargets[0] as NodeProxy;
//             // return nodeProxy != null;
//         }
//
//         protected override void DrawPropertyLayout(GUIContent label)
//         {
//             var graphProxy = Property.Tree.WeakTargets[0] as NodeGraphProxy;
//             if (graphProxy == null)
//             {
//                 return;
//             }
//
//             if (Property.Index >= graphProxy.NodeProxies.Count)
//             {
//                 return;
//             }
//
//             var nodeProxy = graphProxy.NodeProxies[Property.Index];
//             NodePort port = nodeProxy.GetInputPort(Property.Name);
//
//             // if (!NodeEditor.inNodeEditor)
//             // {
//             //     return;
//             // }
//
//             if (Property.Tree.WeakTargets.Count > 1)
//             {
//                 SirenixEditorGUI.WarningMessageBox("Cannot draw ports with multiple nodes selected");
//                 return;
//             }
//
//             if (port != null)
//             {
//                 var portProperty = Property.Tree.GetUnityPropertyForPath(Property.UnityPropertyPath);
//                 if (portProperty == null)
//                 {
//                     SirenixEditorGUI.ErrorMessageBox("Port property missing at: " + Property.UnityPropertyPath);
//                     return;
//                 }
//
//                 var labelWidth = Property.GetAttribute<LabelWidthAttribute>();
//                 if (labelWidth != null)
//                     GUIHelper.PushLabelWidth(labelWidth.Width);
//                 NodeEditorGUILayout.PropertyField(portProperty, label == null ? GUIContent.none : label, graphProxy,
//                     port, true, GUILayout.MinWidth(30));
//                 if (labelWidth != null)
//                     GUIHelper.PopLabelWidth();
//             }
//         }
//     }
// }