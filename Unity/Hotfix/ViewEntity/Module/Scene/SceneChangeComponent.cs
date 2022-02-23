using UnityEngine;

namespace ET
{
    public class SceneChangeComponent: Entity
    {
        public AsyncOperation loadMapOperation;
        public ETTask tcs;
        
        // TODO 存储一些数据, 用于区分是哪个加载的界面.
    }
}