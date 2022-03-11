// using System;
//
// namespace ET
// {
//     public class Event_EnterMobaFinish_View : AEvent<EventType.EnterMobaFinish>
//     {
//         protected override async ETTask Run(EventType.EnterMobaFinish args)
//         {
//             try
//             {
//                 // TODO 通知服务器准备完成.
//                 
//                 // TODO 直接开始游戏.
//                 
//                 await ETTask.CompletedTask;
//             }
//             catch (Exception e)
//             {
//                 Log.Error(e);
//             }
//         }
//     }
// }