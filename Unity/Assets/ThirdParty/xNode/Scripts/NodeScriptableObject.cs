using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XNodeEditor.Internal
{
    public class NodeScriptableObject : ScriptableObject
    {
        public virtual Type GetDataType()
        {
            return base.GetType();
        }
    }
}