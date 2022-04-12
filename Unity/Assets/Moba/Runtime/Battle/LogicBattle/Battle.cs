namespace Scarf.Moba
{
    public class Battle: CObject
    {
        public TimerFrameSys TimerFrameSys { get; private set; }

        public CCoroutineManager CCoroutineManager { get; private set; }

        public BattleRandom BattleRandom { get; private set; }

        public EventMgr EventMgr { get; private set; }

        public EBattleGameMode CurBattleMode { get; private set; }

        public BattleScene BattleScene { get; private set; }

        public BattleData BattleData { get; private set; }

        public Battle(BattleData battleData)
        {
            this.BattleData = battleData;
            this.CurBattleMode = battleData.BattleGameMode;
            this.Battle = this;
            this.TimerFrameSys = new TimerFrameSys();
            this.CCoroutineManager = new CCoroutineManager();
            this.BattleRandom = new BattleRandom();
            this.EventMgr = new EventMgr();
            this.BattleScene = new BattleScene();
            this.BattleScene.Battle = this;

            // DONE: 根据不同游戏玩法初始化不同流程组件.
            switch (CurBattleMode)
            {
                case EBattleGameMode.EDefault:
                case EBattleGameMode.E3v3:
                    this.Battle.AddComponent<Mode3v3Process>();
                    break;
                case EBattleGameMode.ENoviceGuide:
                    // TODO 新手引导玩法模式.
                    break;
                case EBattleGameMode.E1v1:
                    this.Battle.AddComponent<Mode1v1Process>();
                    break;
                case EBattleGameMode.EBoss:
                    this.Battle.AddComponent<ModeBossProcess>();
                    break;
                case EBattleGameMode.EScuffle:
                    this.Battle.AddComponent<ModeScuffleProcess>();
                    break;
                case EBattleGameMode.EDefense:
                    this.Battle.AddComponent<ModeDefenseWarProcess>();
                    break;
            }
        }

        protected override void OnInit()
        {
            // DONE: 正序初始化.
            this.TimerFrameSys.Init();
            this.CCoroutineManager.Init();
            this.BattleRandom.Init(this.BattleData.RandomSeed);
            this.EventMgr.Init();
            this.BattleScene.Init();
        }

        protected override void OnStart()
        {
            this.BattleScene.Start();
        }

        protected override void OnDestroy()
        {
            // DONE: 倒序卸载.
            this.BattleScene.Dispose();
            this.EventMgr.Clear();
            this.BattleRandom.Clear();
            this.CCoroutineManager.Clear();
            this.TimerFrameSys.Clear();
        }

        public override void OnFrameSyncUpdate(int delta)
        {
            // DONE: 时间相关组件.
            TimerFrameSys.OnLogicFrame(delta);
            CCoroutineManager.Tick();

            // DONE: 战斗流程组件.
            base.OnFrameSyncUpdate(delta);

            // DONE: 战斗场景及玩法组件.
            BattleScene.OnFrameSyncUpdate(delta);
        }

        #region 各种Id生成器

        private int stateId;

        public int GenerateStateId()
        {
            return ++stateId;
        }

        private int buffId;

        public int GenerateBuffId()
        {
            return ++this.buffId;
        }

        #endregion
    }
}