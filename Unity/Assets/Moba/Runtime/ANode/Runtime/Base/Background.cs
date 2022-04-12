using System;
using System.Collections.Generic;

namespace Scarf.ANode.Flow.Runtime
{
    [Serializable]
    public class Blackboard: Dictionary<string, int>, UnityEngine.ISerializationCallbackReceiver
    {
        [UnityEngine.SerializeField]
        private List<string> keys = new List<string>();

        [UnityEngine.SerializeField]
        private List<int> values = new List<int>();

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (var pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            this.Clear();

            if (keys.Count != values.Count)
                throw new System.Exception("there are " + keys.Count + " keys and " + values.Count +
                    " values after deserialization. Make sure that both key and value types are serializable.");

            for (int i = 0; i < keys.Count; i++)
                this.Add(keys[i], values[i]);
        }

        public void AddParameter(string key, int value)
        {
            if (this.ContainsKey(key))
            {
                this[key] = value;
                return;
            }

            this.Add(key, value);
        }

        public void RemoveParameter(string key)
        {
            this.Remove(key);
        }

        public void UpdateParameter(string key, int value)
        {
            if (!this.ContainsKey(key))
            {
                return;
            }

            this[key] = value;
        }

        public int GetParameter(string key)
        {
            int result;
            this.TryGetValue(key, out result);
            return result;
        }
    }
}