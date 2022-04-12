using System;
using System.Reflection;
using Sirenix.Serialization;

namespace XNode
{
    public static class NodeSerialization
    {
        public static object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }

        public static T Instantiate<T>(T obj) where T : class
        {
            var bytes = SerializationUtility.SerializeValue(obj, DataFormat.Binary);
            return SerializationUtility.DeserializeValue<T>(bytes, DataFormat.Binary);
        }
    }
}