namespace ET
{
    public class Event_MobaBattleCommitOperate : AEvent<EventType.MobaBattleCommitOperate>
    {
        protected override async ETTask Run(EventType.MobaBattleCommitOperate args)
        {
            var mobaBattleEntity = args.inputComponent.GetParent<MobaBattleEntity>();
            if (mobaBattleEntity.m_bIsNet)
            {
                var netMobaServerComponent = mobaBattleEntity.GetComponent<NetMobaServerComponent>();
                if (netMobaServerComponent == null)
                    return;
                netMobaServerComponent.SendFrameMsg(args.frameMsg);
            }
            else
            {
                var localMobaServerComponent = mobaBattleEntity.GetComponent<LocalMobaServerComponent>();
                if (localMobaServerComponent == null)
                    return;
                var frameSyncComponent = mobaBattleEntity.GetComponent<FrameSyncComponent>();
                if (frameSyncComponent == null)
                    return;
                localMobaServerComponent.ReceiveOperation(args.frameMsg.Uid,
                    new C2B_FrameMsg() { FrameId = frameSyncComponent.m_nCurFrame, Msg = args.frameMsg });
            }

            await ETTask.CompletedTask;
        }
    }
}