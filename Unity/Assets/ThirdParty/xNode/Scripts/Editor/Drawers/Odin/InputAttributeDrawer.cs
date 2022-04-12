#if UNITY_EDITOR && ODIN_INSPECTOR
using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;
using XNode;

namespace XNodeEditor
{
    public class InputAttributeDrawer : OdinAttributeDrawer<XNode.Node.InputAttribute>
    {
        protected override bool CanDrawAttributeProperty(InspectorProperty property)
        {
            var nodeProxy = property.Tree.WeakTargets[0] as NodeProxy;
            return nodeProxy != null;
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var aaa = this.Property.Children.Get(Property.Name);

            var nodeProxy = Property.Tree.WeakTargets[0] as NodeProxy;
            NodePort port = nodeProxy.GetInputPort(Property.Name);

            if (!NodeEditor.inNodeEditor)
            {
                if (Attribute.backingValue == XNode.Node.ShowBackingValue.Always ||
                    Attribute.backingValue == XNode.Node.ShowBackingValue.Unconnected && !port.IsConnected)
                    CallNextDrawer(label);
                return;
            }

            if (Property.Tree.WeakTargets.Count > 1)
            {
                SirenixEditorGUI.WarningMessageBox("Cannot draw ports with multiple nodes selected");
                return;
            }

            if (port != null)
            {
                var portProperty = Property.Tree.GetUnityPropertyForPath(Property.UnityPropertyPath);
                if (portProperty == null)
                {
                    SirenixEditorGUI.ErrorMessageBox("Port property missing at: " + Property.UnityPropertyPath);
                    return;
                }

                ShowLiteral(port);
                var labelWidth = Property.GetAttribute<LabelWidthAttribute>();
                if (labelWidth != null)
                    GUIHelper.PushLabelWidth(labelWidth.Width);

                switch (Attribute.backingValue)
                {
                    case Node.ShowBackingValue.Never:
                        break;
                    case Node.ShowBackingValue.Unconnected:
                        if (!port.IsConnected)
                        {
                            CallNextDrawer(label);
                        }
                        break;
                    case Node.ShowBackingValue.Always:
                        CallNextDrawer(label);
                        break;
                }
                NodeEditorGUILayout.PropertyField(portProperty, label ?? GUIContent.none, nodeProxy.graphProxy, port,
                    true, GUILayout.MinWidth(30));

                CacheLiteral(port);
                if (labelWidth != null)
                    GUIHelper.PopLabelWidth();
            }
        }

        private void ShowLiteral(NodePort port)
        {
            var cacheValue = port.DefaultValue;
            if (string.IsNullOrEmpty(cacheValue)|| string.IsNullOrWhiteSpace(cacheValue))
            {
                return;
            }

            var valueType = Property.BaseValueEntry.TypeOfValue;
            if (valueType == typeof(Node.ControlPort))
            {
                return;
            }

            var targetValue = cacheValue.ForceConvert(valueType);
            if (targetValue == null)
            {
                return;
            }

            Property.BaseValueEntry.WeakSmartValue = targetValue;
        }

        private void CacheLiteral(NodePort port)
        {
            if (Property.BaseValueEntry.WeakSmartValue == null)
            {
                return;
            }

            if (port.IsConnected)
            {
                port.DefaultValue = "";
                return;
            }

            if (port.ValueType == typeof(Node.ControlPort))
            {
                return;
            }

            port.DefaultValue = Property.BaseValueEntry.WeakSmartValue.ToString();
        }
    }
}
#endif