using System.Collections.Generic;

namespace ET
{
    public class OnlineComponent: Entity
    {
        private Dictionary<long, long> dictionary = new Dictionary<long, long>();

        public void AddUid(long uid, long gateId)
        {
            this.dictionary.Add(uid, gateId);
        }

        public long GetGateId(long uid)
        {
            long result = 0;
            this.dictionary.TryGetValue(uid, out result);
            return result;
        }

        public void RemoveByUid(long uid)
        {
            this.dictionary.Remove(uid);
        }

        public bool HasOnline(long uid)
        {
            return this.dictionary.ContainsKey(uid);
        }
    }
}