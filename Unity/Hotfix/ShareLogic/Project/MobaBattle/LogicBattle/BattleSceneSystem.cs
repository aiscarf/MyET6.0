using System.Collections.Generic;

namespace ET
{
    public class BattleSceneComponentAwakeSystem: AwakeSystem<BattleSceneComponent, MapData>
    {
        public override void Awake(BattleSceneComponent self, MapData mapData)
        {
            self.m_cMap = new Map();
            self.m_cMap.LoadMapFile(mapData);
            self.m_cPathfinding = new PathFinding(self.m_cMap);
            self.m_lstAllUnits = new List<Unit>();
            self.m_lstRemoveUnits = new List<Unit>();
        }
    }

    public class BattleSceneComponentDestroySystem: DestroySystem<BattleSceneComponent>
    {
        public override void Destroy(BattleSceneComponent self)
        {
        }
    }

    public static class BattleSceneSystem
    {
        #region 添加Unit

        public static void AddUnit(this BattleSceneComponent self, Unit unit)
        {
            if (unit == null)
                return;
            self.m_lstAllUnits.Add(unit);
        }

        public static void RemoveUnit(this BattleSceneComponent self, Unit unit)
        {
            if (unit == null)
                return;
            self.m_lstRemoveUnits.Add(unit);
        }

        #endregion

        #region Unit筛选

        public static List<Unit> GetUnitsByUnitType(this BattleSceneComponent self, params EUnitType[] eUnitTypes)
        {
            self.LstFilter.Clear();
            for (int i = 0; i < self.m_lstAllUnits.Count; i++)
            {
                var unit = self.m_lstAllUnits[i];
                for (int j = 0; j < eUnitTypes.Length; j++)
                {
                    if (unit.UnitType == eUnitTypes[j])
                    {
                        self.LstFilter.Add(unit);
                        break;
                    }
                }
            }

            return self.LstFilter;
        }

        public static Unit GetUnitById(this BattleSceneComponent self, int entityId)
        {
            for (int i = 0; i < self.m_lstAllUnits.Count; i++)
            {
                var unit = self.m_lstAllUnits[i];
                if (unit.EntityId == entityId)
                {
                    return unit;
                }
            }

            return null;
        }

        public static List<Unit> GetUnitsByTemplateId(this BattleSceneComponent self, int templateId)
        {
            self.LstFilter.Clear();
            for (int i = 0; i < self.m_lstAllUnits.Count; i++)
            {
                var unit = self.m_lstAllUnits[i];
                if (unit.TemplateId == templateId)
                {
                    self.LstFilter.Add(unit);
                }
            }

            return self.LstFilter;
        }

        public static Unit GetUnitByServerId(this BattleSceneComponent self, long serverId)
        {
            for (int i = 0; i < self.m_lstAllUnits.Count; i++)
            {
                var unit = self.m_lstAllUnits[i];
                if (unit.ServerId == serverId)
                {
                    return unit;
                }
            }

            return null;
        }

        #endregion

        #region 帧同步驱动

        public static void OnFrameSyncUpdate(this BattleSceneComponent self, int delta)
        {
            // DONE: 遍历所有角色.
            for (int i = 0; i < self.m_lstAllUnits.Count; i++)
            {
                var unit = self.m_lstAllUnits[i];
                unit.OnFrameSyncUpdate(delta);
            }
        }

        #endregion
    }

    public class BattleSceneStepFrame: AStepFrame<BattleSceneComponent>
    {
        public override void Bind(FrameSyncComponent frameSyncComponent)
        {
            self = frameSyncComponent.GetParent<MobaBattleEntity>().GetComponent<BattleSceneComponent>();
        }

        public override void OnStepFrame(int delta)
        {
            self.OnFrameSyncUpdate(delta);
        }
    }
}