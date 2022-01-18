using System.Collections.Generic;

namespace ET
{
    public class UIMediatorManager : Entity
    {
        public static UIMediatorManager Instance { get; set; }
        public Dictionary<string, IMediator> AllMediator;
    }
}