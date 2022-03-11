using System;

namespace ET
{
    public class B2C_OnFrame_Handler : AMHandler<B2C_OnFrame>
    {
        protected override async ETTask Run(Session session, B2C_OnFrame message)
        {
            try
            {
                Log.Debug("收到了服务器消息 " + message.FrameId);

                var battleDataComponent = DataHelper.GetDataComponentFromCurScene<BattleDataComponent>();
                var mobaBattleEntity = ZoneSceneManagerComponent.Instance.CurScene.GetChild<MobaBattleEntity>(battleDataComponent.BattleId);
                if (mobaBattleEntity == null)
                {
                    return;
                }
                
                if (mobaBattleEntity.GetComponent<FrameSyncComponent>() == null)
                {
                    Log.Debug("添加了n次");
                    mobaBattleEntity.AddComponent<FrameSyncComponent>();
                    mobaBattleEntity.AddComponent<InputComponent>();
                    mobaBattleEntity.m_battleProcess.Start().Coroutine();
                }
                
                FrameSyncComponent frameSyncComponent = mobaBattleEntity.GetComponent<FrameSyncComponent>();
                frameSyncComponent?.AddLogicFrame(message);
                
                mobaBattleEntity.GetComponent<InputComponent>().CollectInput();
                await ETTask.CompletedTask;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}