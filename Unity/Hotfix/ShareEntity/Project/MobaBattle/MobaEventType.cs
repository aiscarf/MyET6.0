using System.Collections.Generic;

namespace ET
{
    namespace EventType
    {
        public struct EnterMobaBegin
        {
            public EBattleMode BattleMode;
            public int RandomSeed;
            public string ScenePfbPath;
            public string MapConfigPath;
            public List<MobaPlayerInfo> PlayerInfos;
            public List<string> NeedLoadPanelIds;
        }
        
        public struct EnterMobaFinish
        {
        }
        
        public struct MobaBattleInit
        {
            public EBattleMode eBattleMode;
            public bool bIsNet;
            public MapData mapData;
            public List<MobaPlayerInfo> PlayerInfos;
        }

        public struct MobaBattleStart
        {
            
        }

        public struct MobaGameEntryAwake
        {

        }

        public struct MobaBattleProcessInit
        {

        }

        public struct MobaBattleProcessDestroy
        {

        }

        public struct MobaBattleProcessStart
        {

        }

        public struct MobaBattleCommitOperate
        {
            public InputComponent inputComponent;
            public FrameMsg frameMsg;
        }
        public struct MobaCreateUnit
        {
            public Unit unit;
        }

        public struct MobaUnitPosition
        {
            public Unit unit;
            public SVector3 pos;
        }

        public struct MobaUnitForward
        {
            public Unit unit;
            public SVector3 forward;
            public bool bImmediately;
        }

        public struct MobaUnitBeginMove
        {
            public Unit unit;
        }

        public struct MobaUnitEndMove
        {
            public Unit unit;
        }

        public struct MobaUnitTargetPos
        {
            public Unit unit;
            public SVector3 targetPos;
            public int speed;
        }
    }
}