using System;

namespace ET
{
    public class C2B_GameMainEnterHandler : AMActorLocationRpcHandler<Player, C2B_GameMainEnter, B2C_GameMainEnter>
    {
        protected override async ETTask Run(Player unit, C2B_GameMainEnter request, B2C_GameMainEnter response, Action reply)
        {
            await ETTask.CompletedTask;

            reply();
        }
    }
}