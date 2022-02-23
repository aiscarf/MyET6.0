using System.Collections.Generic;

namespace ET
{
    public class UIMediatorManager : Entity
    {
        public static UIMediatorManager Instance { get; set; }
        public Dictionary<string, IMediator> m_allUiMediator;
        public Dictionary<string, IUIEvent> m_allUiEvents;
    }
}