using UnityEngine;

namespace ET
{
    public class MobaBattleViewComponentAwakeSystem : AwakeSystem<MobaBattleViewComponent>
    {
        public override void Awake(MobaBattleViewComponent self)
        {
            
        }
    }

    public static class MobaBattleViewSystem
    {
        public static void InitRoot(this MobaBattleViewComponent self, GameObject[] rootArr)
        {
            for (int i = 0; i < rootArr.Length; i++)
            {
                var root = rootArr[i];
                if (root.name == "SceneRoot")
                {
                    self.SceneRoot = root;
                }
                else if (root.name == "MapRoot")
                {
                    self.MapRoot = root;
                }
                else if (root.name == "UnitRoot")
                {
                    self.UnitRoot = root;
                }
                else if (root.name == "ChaseCamera")
                {
                    self.ChaseCamera = root.GetComponent<ChaseCamera>();
                    self.ChaseCamera.Init();
                }
            }
        }
    }
}