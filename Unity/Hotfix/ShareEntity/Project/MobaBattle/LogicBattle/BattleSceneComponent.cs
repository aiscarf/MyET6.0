using System.Collections.Generic;

namespace ET
{
    public class BattleSceneComponent : Entity
    {
        public Map m_cMap;
        public PathFinding m_cPathfinding;
        public List<Unit> m_lstAllUnits;
        public List<Unit> m_lstRemoveUnits;

        public List<Unit> LstFilter = new List<Unit>();
    }
}