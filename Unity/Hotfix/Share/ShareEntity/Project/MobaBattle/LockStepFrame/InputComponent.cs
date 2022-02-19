using System.Collections.Generic;

namespace ET
{
    public class InputComponent : Entity
    {
        public FrameMsg m_operate;
        public FrameMsg m_lastOperate;
        public List<int> m_lstExcludeTypes;
    }
}