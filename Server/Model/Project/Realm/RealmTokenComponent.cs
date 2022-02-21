namespace ET
{
    public class RealmTokenComponent: Entity
    {
        private DoubleMap<long, string> dictionary = new DoubleMap<long, string>();

        public void AddToken(long uid, string token)
        {
            this.dictionary.Add(uid, token);
        }

        public long GetUid(string token)
        {
            return this.dictionary.GetKeyByValue(token);
        }

        public string GetToken(long uid)
        {
            return this.dictionary.GetValueByKey(uid);
        }

        public void RemoveToken(long uid)
        {
            this.dictionary.RemoveByKey(uid);
        }
    }
}