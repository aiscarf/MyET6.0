using System;

namespace UnityEditor
{
    public static class SerializedObjectExtern
    {
        public static object GetSmartValue(this SerializedProperty self)
        {
            object result = null;
            var propertyType = self.propertyType;
            switch (propertyType)
            {
                case SerializedPropertyType.Generic:
                    break;
                case SerializedPropertyType.Integer:
                    result = self.intValue;
                    break;
                case SerializedPropertyType.Boolean:
                    result = self.boolValue;
                    break;
                case SerializedPropertyType.Float:
                    result = self.floatValue;
                    break;
                case SerializedPropertyType.String:
                    result = self.stringValue;
                    break;
                case SerializedPropertyType.Color:
                    result = self.colorValue;
                    break;
                case SerializedPropertyType.ObjectReference:
                    result = self.objectReferenceValue;
                    break;
                case SerializedPropertyType.LayerMask:
                    break;
                case SerializedPropertyType.Enum:
                    result = self.enumValueIndex;
                    break;
                case SerializedPropertyType.Vector2:
                    result = self.vector2Value;
                    break;
                case SerializedPropertyType.Vector3:
                    result = self.vector3Value;
                    break;
                case SerializedPropertyType.Vector4:
                    result = self.vector4Value;
                    break;
                case SerializedPropertyType.Rect:
                    result = self.rectValue;
                    break;
                case SerializedPropertyType.ArraySize:
                    result = self.arraySize;
                    break;
                case SerializedPropertyType.Character:
                    result = null;
                    break;
                case SerializedPropertyType.AnimationCurve:
                    result = self.animationCurveValue;
                    break;
                case SerializedPropertyType.Bounds:
                    result = self.boundsValue;
                    break;
                case SerializedPropertyType.Gradient:
                    result = null;
                    break;
                case SerializedPropertyType.Quaternion:
                    result = self.quaternionValue;
                    break;
                case SerializedPropertyType.ExposedReference:
                    result = self.exposedReferenceValue;
                    break;
                case SerializedPropertyType.FixedBufferSize:
                    result = self.fixedBufferSize;
                    break;
                case SerializedPropertyType.Vector2Int:
                    result = self.vector2IntValue;
                    break;
                case SerializedPropertyType.Vector3Int:
                    result = self.vector3IntValue;
                    break;
                case SerializedPropertyType.RectInt:
                    result = self.rectIntValue;
                    break;
                case SerializedPropertyType.BoundsInt:
                    result = self.boundsIntValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }

        public static void SetSmartValue(this SerializedProperty self)
        {

        }
    }
}