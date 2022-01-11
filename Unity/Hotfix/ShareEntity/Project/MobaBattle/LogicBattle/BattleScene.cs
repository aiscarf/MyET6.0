using System.Collections.Generic;

namespace ET
{
    public class BattleScene : Entity
    {
        public Map m_cMap;
        public PathFinding m_cPathfinding;
        public List<BaseUnit> m_lstAllEntitys;
        public List<BaseUnit> m_lstRemoveEntitys;
    }
}