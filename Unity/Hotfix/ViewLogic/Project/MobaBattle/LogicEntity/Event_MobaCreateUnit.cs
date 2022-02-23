using UnityEngine;

namespace ET
{
    public class Event_MobaCreateUnit : AEvent<EventType.MobaCreateUnit>
    {
        protected override async ETTask Run(EventType.MobaCreateUnit args)
        {
            await ETTask.CompletedTask;
            if (args.unit == null)
                return;
            var go = (GameObject)ResourcesComponent.Instance.GetAsset("hero01_model01_l".StringToAB(), "hero01_model01_l");
            go = GameObject.Instantiate(go);
            var mobaBattleViewComponent = args.unit.DomainScene().GetComponent<MobaBattleViewComponent>();
            go.transform.SetParent(mobaBattleViewComponent.UnitRoot.transform);
            args.unit.AddComponent<UnitViewComponent, Transform>(go.transform);

            if (args.unit.ServerId == 1001)
            {
                mobaBattleViewComponent.ChaseCamera.SetFollower(go.transform);
            }
        }
    }
}