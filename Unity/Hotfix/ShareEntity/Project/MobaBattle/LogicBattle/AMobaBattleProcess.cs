namespace ET
{
    public abstract class AMobaBattleProcess
    {
        public void Init()
        {
            this.OnInit();
            Game.EventSystem.Publish(new EventType.MobaBattleProcessInit());
        }

        public void Destroy()
        {
            this.OnDestroy();
            Game.EventSystem.Publish(new EventType.MobaBattleProcessDestroy());
        }

        public async ETTask Start()
        {
            await OnStart();
            await Game.EventSystem.PublishAsync(new EventType.MobaBattleProcessStart());
        }

        protected abstract void OnInit();
        
        protected abstract void OnDestroy();
        protected abstract ETTask OnStart();
    }
}