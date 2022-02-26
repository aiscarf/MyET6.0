using System;

namespace ET
{
    public class C2B_FrameMsgHandler: AMActorLocationRpcHandler<Player, C2B_FrameMsg, B2C_FrameMsg>
    {
        protected override async ETTask Run(Player unit, C2B_FrameMsg request, B2C_FrameMsg response, Action reply)
        {
            await ETTask.CompletedTask;
            reply();
        }
    }
}