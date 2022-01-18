using System.Collections.Generic;

namespace ET
{
    public class ZoneSceneManagerComponent: Entity
    {
        public static ZoneSceneManagerComponent Instance;
        
        /// <summary>
        /// 管理所有场景
        /// </summary>
        public Dictionary<int, Scene> ZoneScenes = new Dictionary<int, Scene>();
        
        /// <summary>
        /// 当前处于的场景
        /// </summary>
        public Scene CurScene { get; set; }
    }
}