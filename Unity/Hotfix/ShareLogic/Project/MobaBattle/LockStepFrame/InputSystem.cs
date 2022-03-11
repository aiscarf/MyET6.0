using System.Collections.Generic;

namespace ET
{
    public class InputComponentAwakeSystem: AwakeSystem<InputComponent>
    {
        public override void Awake(InputComponent self)
        {
            self.m_operate = new FrameMsg();
            self.m_lastOperate = new FrameMsg();
            self.m_lstExcludeTypes = new List<int>();
        }
    }

    public class InputComponentDestroySystem: DestroySystem<InputComponent>
    {
        public override void Destroy(InputComponent self)
        {
        }
    }

    public static class InputSystem
    {
        public static void InputOrderPriority(this InputComponent self, long serverId, EInputType eInputType, int arg1,
        int arg2)
        {
            if ((int)eInputType > self.m_operate.Optype)
            {
                self.m_operate.Optype = (int)eInputType;
                self.m_operate.Uid = serverId;
                self.m_operate.Arg1 = arg1;
                self.m_operate.Arg2 = arg2;
            }
        }
        
        public static void CollectInput(this InputComponent self)
        {
            if (self.m_operate.Optype == (int)EInputType.None)
            {
                return;
            }

            if (self.m_lastOperate.Uid == self.m_operate.Uid && self.m_lastOperate.Optype == self.m_operate.Optype &&
                self.m_lastOperate.Arg1 == self.m_operate.Arg1 && self.m_lastOperate.Arg2 == self.m_operate.Arg2)
            {
                return;
            }

            Game.EventSystem.Publish(new EventType.MobaBattleCommitOperate() { inputComponent = self, frameMsg = self.m_operate });
            self.m_operate.Optype = (int)EInputType.None;
        }

        public static void HandleFrameData(this InputComponent self, B2C_OnFrame frameData)
        {
            if (frameData == null)
            {
                return;
            }
        
            var battleSceneComponent = self.GetParent<MobaBattleEntity>().GetComponent<BattleSceneComponent>();
            for (int i = 0; i < frameData.Msg.Count; i++)
            {
                var frameMsg = frameData.Msg[i];
                if (frameMsg == null)
                    continue;
                var unit = battleSceneComponent.GetUnitByServerId(frameMsg.Uid);
                if (unit == null)
                    continue;
                self.HandleOperate(unit, frameMsg.Optype, frameMsg.Arg1, frameMsg.Arg2);
            }
        }

        public static void HandleOperate(this InputComponent self, Unit heroUnit, int opt, int arg1, int arg2)
        {
            // 加个指令屏蔽
            if (self.m_lstExcludeTypes.Contains(opt))
                return;
            switch ((EInputType)opt)
            {
                case EInputType.Move:
                    var unitMoveComponent = heroUnit.GetComponent<UnitMoveComponent>();
                    if (unitMoveComponent == null)
                        break;
                    if (!unitMoveComponent.CanMove())
                        break;
                    unitMoveComponent.Move(arg1, 0);
                    break;
                case EInputType.MoveEnd:
                    var unitMoveComponent2 = heroUnit.GetComponent<UnitMoveComponent>();
                    if (unitMoveComponent2 == null)
                        break;
                    if (!unitMoveComponent2.CanStopMove())
                        break;
                    unitMoveComponent2.MoveEnd();
                    break;
            }
        }
    }
}