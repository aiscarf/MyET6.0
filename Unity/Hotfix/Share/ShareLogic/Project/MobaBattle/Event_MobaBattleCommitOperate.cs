namespace ET
{
    public class Event_MobaBattleCommitOperate : AEvent<EventType.MobaBattleCommitOperate>
    {
        protected override async ETTask Run(EventType.MobaBattleCommitOperate args)
        {
            await ETTask.CompletedTask;
            var mobaBattleComponent = args.inputComponent.GetMobaBattleComponent();
            if (mobaBattleComponent.m_bIsNet)
            {
                
            }
            else
            {
                var localMobaServerComponent = mobaBattleComponent.GetComponent<LocalMobaServerComponent>();
                if (localMobaServerComponent == null)
                    return;
                var frameSyncComponent = mobaBattleComponent.GetComponent<FrameSyncComponent>();
                if (frameSyncComponent == null)
                    return;
                localMobaServerComponent.ReceiveOperation(args.frameMsg.Uid, new C2B_FrameMsg() { FrameId = frameSyncComponent.m_nCurFrame, Msg = args.frameMsg });
            }
        }
    }
}