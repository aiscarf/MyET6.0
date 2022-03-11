namespace ET
{
    public class NoviceGuideProcess : AMobaBattleProcess
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