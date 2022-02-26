using System;
using System.Collections.Generic;

namespace ET
{
    public class MatchComponentAwakeSystem: AwakeSystem<MatchComponent>
    {
        public override void Awake(MatchComponent self)
        {
            // DONE: 创建计时器.
            self.Timer = TimerComponent.Instance.NewRepeatedTimer(5000, TimerType.MatchTimer, self);

            var gateSceneConfig = StartSceneConfigCategory.Instance.GetBySceneType(self.DomainZone(), SceneType.Gate);
            self.GateActorId = gateSceneConfig.InstanceId;
        }
    }

    public class MatchComponentDestroySystem: DestroySystem<MatchComponent>
    {
        public override void Destroy(MatchComponent self)
        {
            TimerComponent.Instance.Remove(ref self.Timer);
        }
    }

    [Timer(TimerType.MatchTimer)]
    public class MatchComponentUpdate: ATimer<MatchComponent>
    {
        public override void Run(MatchComponent self)
        {
            try
            {
                // DONE: 每秒处理一次匹配队列.
                self.Update();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }

    public static class MatchComponentSystem
    {
        public static void Update(this MatchComponent self)
        {
            long actorId = self.GateActorId;

            // DONE: 同一个地图的进行匹配.
            foreach (int mapId in self.GetAllMapIds())
            {
                int matchNum = self.GetMatchNum(mapId);
                if (matchNum <= 0)
                    continue;
                var dungeonConfig = DungeonConfigCategory.Instance.Get(mapId);
                if (dungeonConfig == null)
                    continue;
                int needNum = 1; //dungeonConfig.NeedPlayerNum;
                if (matchNum < needNum)
                    continue;

                // DONE: 从该地图的队列里找寻needNum个玩家.
                var queue = self.GetMatchsByMapId(mapId);
                if (queue == null)
                    continue;

                var list = ListComponent<long>.Create();
                foreach (long uid in queue)
                {
                    if (list.Count >= needNum)
                        break;
                    self.Remove(uid);
                    list.Add(uid);
                }

                // DONE: 处理排队成功, 通知网关服务器.
                var m2GOnSuccessMatch = new M2G_OnSuccessMatch();
                m2GOnSuccessMatch.MapId = mapId;
                m2GOnSuccessMatch.Uids.AddRange(list);
                MessageHelper.SendActor(actorId, m2GOnSuccessMatch);

                list.Dispose();
            }
        }
    }
}