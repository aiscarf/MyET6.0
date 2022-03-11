using UnityEngine;

namespace ET
{
    public class Event_MobaCreateUnit_View : AEvent<EventType.MobaCreateUnit>
    {
        protected override async ETTask Run(EventType.MobaCreateUnit args)
        {
            if (args.unit == null)
                return;

            var heroConfig = HeroConfigCategory.Instance.Get(args.unit.TemplateId);
            if (heroConfig == null)
                return;
            var go = (GameObject)ResourcesComponent.Instance.GetAsset(heroConfig.ModelRes.StringToAB(),
                heroConfig.ModelRes);
            go = GameObject.Instantiate(go);

            var battleDataComponent = DataHelper.GetDataComponentFromCurScene<BattleDataComponent>();
            var mobaBattleEntity = ZoneSceneManagerComponent.Instance.CurScene.GetChild<MobaBattleEntity>(battleDataComponent.BattleId);
            var mobaBattleViewComponent = mobaBattleEntity.GetComponent<MobaBattleViewComponent>();
            go.transform.SetParent(mobaBattleViewComponent.UnitRoot.transform);
            args.unit.AddComponent<UnitViewComponent, Transform>(go.transform);
            
            if (args.unit.ServerId == BattleMgr.GetBattleViewDataComponent().Uid)
            {
                mobaBattleViewComponent.ChaseCamera.SetForward(args.unit.BornForward.ToUnity());
                mobaBattleViewComponent.ChaseCamera.SetFollower(go.transform);
            }
            await ETTask.CompletedTask;
        }
    }
}