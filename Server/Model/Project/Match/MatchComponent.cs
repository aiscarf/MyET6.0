using System.Collections.Generic;

namespace ET
{
    public sealed class MatchComponent: Entity
    {
        private readonly Dictionary<long, Matcher> dictionary = new Dictionary<long, Matcher>();
        private readonly MultiDictionary<int, long, Matcher> multiDictionary = new MultiDictionary<int, long, Matcher>();

        public long Timer;
        public long GateActorId;

        public void Add(Matcher matcher)
        {
            this.dictionary.Add(matcher.Uid, matcher);
            this.multiDictionary.Add(matcher.MapId, matcher.Uid, matcher);
        }

        public bool Remove(long uid)
        {
            var matcher = Get(uid);
            if (matcher == null)
                return false;
            this.dictionary.Remove(uid);
            return this.multiDictionary.Remove(matcher.MapId, uid);
        }

        public Matcher Get(long uid)
        {
            this.dictionary.TryGetValue(uid, out var matcher);
            return matcher;
        }

        public IEnumerable<long> GetMatchsByMapId(int mapId)
        {
            if (!this.multiDictionary.TryGetValue(mapId, out var queue))
            {
                return null;
            }

            return queue.Keys;
        }

        public int GetMatchNum(int mapId)
        {
            if (!this.multiDictionary.TryGetValue(mapId, out var queue))
            {
                return 0;
            }

            return queue.Count;
        }

        public IEnumerable<int> GetAllMapIds()
        {
            return this.multiDictionary.Keys;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            this.multiDictionary.Clear();
            base.Dispose();
        }
    }
}