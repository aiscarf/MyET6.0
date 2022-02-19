namespace ET
{
    public class Local1v1Process : AMobaBattleProcess
    {
        protected override void OnInit()
        {
            
        }

        protected override void OnDestroy()
        {
            
        }

        protected override async ETTask OnStart()
        {
            await ETTask.CompletedTask;
        }
    }
}