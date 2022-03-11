namespace ET
{
    public static class MainMgr
    {
        public static async ETTask StartReady()
        {
            await MainHelper.StartReady();
        }

        public static async ETTask CancelReady()
        {
            await MainHelper.CancelReady();
        }
        
        public static MainViewDataComponent GetMainViewDataComponent()
        {
            return DataHelper.GetDataComponentFromCurScene<MainViewDataComponent>();
        }
    }
}