namespace ET
{
    public class MobaBattleEntity : Entity
    {
        /// <summary>
        /// 游戏模式
        /// </summary>
        public EBattleMode m_battleMode = EBattleMode.EDefault;

        /// <summary>
        /// 是否联网
        /// </summary>
        public bool m_bIsNet;
        
        /// <summary>
        /// 游戏流程
        /// </summary>
        public AMobaBattleProcess m_battleProcess;
    }
}