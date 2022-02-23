using UnityEngine.SceneManagement;

namespace ET
{
    [ObjectSystem]
    public class SceneChangeComponentAwakeSystem : AwakeSystem<SceneChangeComponent, string>
    {
        public override void Awake(SceneChangeComponent self, string sceneName)
        {
            self.tcs = ETTask.Create(true);
            self.loadMapOperation = SceneManager.LoadSceneAsync(sceneName);
        }
    }

    [ObjectSystem]
    public class SceneChangeComponentUpdateSystem : UpdateSystem<SceneChangeComponent>
    {
        public override void Update(SceneChangeComponent self)
        {
            if (self.loadMapOperation == null)
            {
                return;
            }
            
            // TODO 推送加载条的进度.
            int progress = (int)(self.loadMapOperation.progress * 100);

            if (!self.loadMapOperation.isDone)
            {
                return;
            }
            
            if (self.tcs == null)
            {
                return;
            }

            ETTask tcs = self.tcs;
            self.tcs = null;
            tcs.SetResult();
        }
    }
    
    [ObjectSystem]
    public class SceneChangeComponentDestroySystem : DestroySystem<SceneChangeComponent>
    {
        public override void Destroy(SceneChangeComponent self)
        {
            self.loadMapOperation = null;
            self.tcs = null;
        }
    }
}