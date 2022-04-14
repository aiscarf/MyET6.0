using System;
using UnityEngine;

namespace Scarf.Moba
{
    public class Event_CreateUnit_Graphic: AEvent<EventType.CreateUnit>
    {
        protected override void Run(EventType.CreateUnit args)
        {
            try
            {
                var battleGraphic = args.unit.Battle.BattleGraphic();

                // TODO 查配置表.
                GameObject go = null;
                go = GameObject.Instantiate(go);
                go.transform.SetParent(battleGraphic.UnitRoot.transform);

                var unitGraphic = args.unit.AddComponent<UnitGraphicComponent>();
                unitGraphic.BindUnity(go.transform);
                unitGraphic.Init();
                unitGraphic.Start();

                if (args.unit.ServerId != battleGraphic.Uid)
                {
                    return;
                }

                battleGraphic.ChaseCamera.SetForward(args.unit.LogicForward.ToUnity());
                battleGraphic.ChaseCamera.SetFollower(go.transform);
            }
            catch (Exception e)
            {
                BattleLog.Error(e);
            }
        }
    }
}