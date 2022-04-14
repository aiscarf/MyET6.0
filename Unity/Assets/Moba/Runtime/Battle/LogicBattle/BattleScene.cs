using System.Collections.Generic;

namespace Scarf.Moba
{
    public class BattleScene: CObject
    {
        private Map m_cMap;
        private PathFinding m_cPathfinding;
        private List<Unit> m_lstAllUnits;
        private List<Unit> m_lstRemoveUnits;
        private List<BaseArea> m_lstAreas;
        private List<BaseBullet> m_lstBullets;

        public Map Map => this.m_cMap;
        public PathFinding Pathfinding => this.m_cPathfinding;
        
        public BattleScene()
        {
            this.m_cMap = new Map();
            this.m_cPathfinding = new PathFinding(this.m_cMap);
            this.m_lstAllUnits = new List<Unit>();
            this.m_lstRemoveUnits = new List<Unit>();
            this.m_lstAreas = new List<BaseArea>();
            this.m_lstBullets = new List<BaseBullet>();
        }

        #region 生命周期

        protected override void OnInit()
        {
        }

        protected override void OnDestroy()
        {
            for (int i = 0; i < this.m_lstBullets.Count; i++)
            {
                var bullet = this.m_lstBullets[i];
                bullet.Dispose();
            }

            for (int i = 0; i < this.m_lstAreas.Count; i++)
            {
                var area = this.m_lstAreas[i];
                area.Dispose();
            }

            for (int i = 0; i < this.m_lstAllUnits.Count; i++)
            {
                var unit = this.m_lstAllUnits[i];
                unit.Dispose();
            }

            this.m_lstAllUnits.Clear();
        }

        public override void OnFrameSyncUpdate(int delta)
        {
            base.OnFrameSyncUpdate(delta);

            // DONE: 遍历所有地形.
            for (int i = 0; i < this.m_lstAreas.Count; i++)
            {
                var area = this.m_lstAreas[i];
                area.OnFrameSyncUpdate(delta);
            }

            // DONE: 遍历所有子弹.
            for (int i = 0; i < this.m_lstBullets.Count; i++)
            {
                var bullet = this.m_lstBullets[i];
                bullet.OnFrameSyncUpdate(delta);
            }

            // DONE: 遍历所有角色.
            for (int i = 0; i < this.m_lstAllUnits.Count; i++)
            {
                var unit = this.m_lstAllUnits[i];
                unit.OnFrameSyncUpdate(delta);
            }
        }

        #endregion

        #region 添加Unit

        public List<Unit> AllUnits => this.m_lstAllUnits;
        public void AddUnit(Unit unit)
        {
            if (unit == null)
                return;
            this.m_lstAllUnits.Add(unit);
        }

        public void RemoveUnit(Unit unit)
        {
            if (unit == null)
                return;
            this.m_lstAllUnits.Remove(unit);
        }

        #endregion

        #region 筛选Unit

        private List<Unit> LstFilter = new List<Unit>();

        public List<Unit> GetUnitsByUnitType(params EUnitType[] eUnitTypes)
        {
            this.LstFilter.Clear();
            for (int i = 0; i < this.m_lstAllUnits.Count; i++)
            {
                var unit = this.m_lstAllUnits[i];
                for (int j = 0; j < eUnitTypes.Length; j++)
                {
                    if (unit.UnitType == eUnitTypes[j])
                    {
                        this.LstFilter.Add(unit);
                        break;
                    }
                }
            }

            return this.LstFilter;
        }

        public Unit GetUnitById(int entityId)
        {
            for (int i = 0; i < this.m_lstAllUnits.Count; i++)
            {
                var unit = this.m_lstAllUnits[i];
                if (unit.Uid == entityId)
                {
                    return unit;
                }
            }

            return null;
        }

        public List<Unit> GetUnitsByTemplateId(int templateId)
        {
            this.LstFilter.Clear();
            for (int i = 0; i < this.m_lstAllUnits.Count; i++)
            {
                var unit = this.m_lstAllUnits[i];
                if (unit.TemplateId == templateId)
                {
                    this.LstFilter.Add(unit);
                }
            }

            return this.LstFilter;
        }

        public Unit GetUnitByServerId(long serverId)
        {
            for (int i = 0; i < this.m_lstAllUnits.Count; i++)
            {
                var unit = this.m_lstAllUnits[i];
                if (unit.ServerId == serverId)
                {
                    return unit;
                }
            }

            return null;
        }

        #endregion

        #region 添加地形

        private List<BaseArea> AreaFilter = new List<BaseArea>();
        public List<BaseArea> AllAreas => this.m_lstAreas;
        public void AddArea(BaseArea area)
        {
            area.Init();
            area.Start();
            this.m_lstAreas.Add(area);
        }

        public BaseArea GetAreaById(int areaId)
        {
            for (int i = 0; i < this.m_lstAreas.Count; i++)
            {
                var area = this.m_lstAreas[i];
                if (area.Id == areaId)
                {
                    return area;
                }
            }

            return null;
        }

        public List<BaseArea> GetAreasByType(EAreaType eAreaType)
        {
            AreaFilter.Clear();
            for (int i = 0; i < this.m_lstAreas.Count; i++)
            {
                var area = this.m_lstAreas[i];
                if (area.AreaType == eAreaType)
                {
                    this.AreaFilter.Add(area);
                }
            }

            return AreaFilter;
        }

        public List<BaseArea> GetAreaByRange(Unit unit)
        {
            AreaFilter.Clear();
            for (int i = 0; i < this.m_lstAreas.Count; i++)
            {
                var area = this.m_lstAreas[i];
                if (area.IsIncludePoint(unit))
                {
                    AreaFilter.Add(area);
                }
            }

            return AreaFilter;
        }

        #endregion
    }
}