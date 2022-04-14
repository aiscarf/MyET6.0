using Scarf.Moba;
using UnityEngine;

namespace ET
{
    public class MobaLauncher: MonoBehaviour
    {
        [Header("游戏玩法")]
        public EBattleGameMode GameMode = EBattleGameMode.EDefault;

        [Header("英雄Id")]
        public int HeroId = 1001;

        [Header("蓝=1, 红=2")]
        public int Camp = 1;

        private void Update()
        {
            UnityUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            UnityLateUpdate?.Invoke();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            UnityApplicationFocus?.Invoke(hasFocus);
        }

        public UnityUpdate UnityUpdate;
        public UnityUpdate UnityLateUpdate;
        public UnityApplicationFocus UnityApplicationFocus;
    }
}

public delegate void UnityUpdate();

public delegate void UnityApplicationFocus(bool hasFocus);