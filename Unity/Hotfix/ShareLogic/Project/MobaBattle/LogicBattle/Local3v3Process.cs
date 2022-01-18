namespace ET
{
    public class Local3v3Process : AMobaBattleProcess
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