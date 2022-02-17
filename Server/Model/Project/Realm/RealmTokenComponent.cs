namespace ET
{
    public class RealmTokenComponent: Entity
    {
        private DoubleMap<string, string> dictionary = new DoubleMap<string, string>();

        public void AddToken(string account, string token)
        {
            this.dictionary.Add(account, token);
        }

        public string GetAccount(string token)
        {
            return this.dictionary.GetKeyByValue(token);
        }

        public string GetToken(string account)
        {
            return this.dictionary.GetValueByKey(account);
        }

        public void RemoveToken(string account)
        {
            this.dictionary.RemoveByKey(account);
        }
    }
}