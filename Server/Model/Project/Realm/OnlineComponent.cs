namespace ET
{
    public class OnlineComponent: Entity
    {
        private DoubleMap<string, string> dictionary = new DoubleMap<string, string>();

        public void AddAccount(string account, string token)
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

        public void RemoveByAccount(string account)
        {
            this.dictionary.RemoveByKey(account);
        }

        public void RemoveByToken(string token)
        {
            this.dictionary.RemoveByValue(token);
        }
    }
}