using System;

namespace ET
{
    public class G2C_OnGameStart_Handler : AMHandler<G2C_OnGameStart>
    {
        protected override async ETTask Run(Session session, G2C_OnGameStart message)
        {
            try
            {
                Log.Debug("匹配成功, 准备战斗界面!");
                
                await ETTask.CompletedTask;
                
                // TODO 切换场景.
                
                // TODO 加载游戏资源.
                
                // TODO 
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}