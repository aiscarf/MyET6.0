using System.Collections.Generic;
using ET.EventType;

namespace ET
{
    [ObjectSystem]
    public class UnitMoveComponentAwakeSystem : AwakeSystem<UnitMoveComponent>
    {
        public override void Awake(UnitMoveComponent self)
        {
            self.Master = self.GetParent<Unit>();
        }
    }

    [ObjectSystem]
    public class UnitMoveComponentDestroySystem : DestroySystem<UnitMoveComponent>
    {
        public override void Destroy(UnitMoveComponent self)
        {
            self.m_quePath.Clear();
        }
    }

    public static class UnitMoveSystem
    {
        public static void OnLogicFrame(this UnitMoveComponent self)
        {
            if (!self.m_bIsMoving)
                return;
            byte mask = self.GetMoveMask();
            int num = self.m_nSpeed;
            SVector3 svector3_1 = self.m_sNextPosition - self.Master.LogicPos;
            SVector3 svector3_2 = svector3_1.normalizedXz * num / 1000 *
                self.GetFrameSyncComponent().LOGIC_FRAME_DELTA / 1000;

            if (svector3_1.sqrMagnitudeXz <= svector3_2.sqrMagnitudeXz)
            {
                if (self.CheckObstacle(self.m_sNextPosition))
                {
                    SVector3 sNextPosition = self.m_sNextPosition;
                    self.GetMobaBattleComponent().GetComponent<BattleSceneComponent>().m_cPathfinding
                        .GetReachablePosition(self.Master.LogicPos, ref sNextPosition, true, mask);
                    self.Master.UpdateLogicPos(sNextPosition);
                    self.Stop();
                    return;
                }

                self.Master.UpdateLogicPos(self.m_sNextPosition);
                if (self.m_quePath.Count > 0)
                {
                    self.ProcessNextPoint();
                }
                else
                {
                    self.Stop();
                }
            }
            else
            {
                SVector3 posTarget = self.Master.LogicPos + svector3_2;
                if (self.CheckObstacle(posTarget))
                {
                    self.GetMobaBattleComponent().GetComponent<BattleSceneComponent>().m_cPathfinding
                        .GetReachablePosition(self.Master.LogicPos, ref posTarget, true, mask);
                    self.Master.UpdateLogicPos(posTarget);
                    self.Stop();
                    return;
                }

                self.Master.UpdateLogicPos(posTarget);
            }
        }

        public static bool Move(this UnitMoveComponent self, List<SVector3> lstPath, int nSpeed, bool bChangeForward,
            EMoveType eMoveType = EMoveType.ENormal)
        {
            if (!self.CanMove(eMoveType) || lstPath.Count <= 0)
                return false;
            self.m_quePath.Clear();
            for (int index = 0; index < lstPath.Count; ++index)
                self.m_quePath.Enqueue(lstPath[index]);
            self.Begin(nSpeed, bChangeForward, eMoveType);
            return true;
        }

        public static bool Move(this UnitMoveComponent self, SVector3 sTargetPos, int nSpeed, bool bChangeForward,
            EMoveType eMoveType = EMoveType.ENormal)
        {
            if (!self.CanMove(eMoveType) || SVector3.EqualsXz(sTargetPos, self.Master.LogicPos))
                return false;
            self.m_quePath.Clear();
            self.m_quePath.Enqueue(sTargetPos);
            self.Begin(nSpeed, bChangeForward, eMoveType);
            return true;
        }

        public static void Stop(this UnitMoveComponent self)
        {
            if (!self.m_bIsMoving)
                return;
            self.m_nSpeed = 0;
            self.m_bIsMoving = false;
            self.m_eMoveType = EMoveType.ENone;
            Game.EventSystem.Publish(new EventType.MobaUnitEndMove { unit = self.GetParent<Unit>() });
        }

        public static bool CanMove(this UnitMoveComponent self, EMoveType eMoveType = EMoveType.ENormal)
        {
            if (!IsNormalMove(eMoveType))
                return true;
            return !self.GetParent<Unit>().IsDie;
        }

        public static bool CanStopMove(this UnitMoveComponent self)
        {
            return self.Master.IsDie || self.IsMoving() && IsNormalMove(self.m_eMoveType);
        }

        public static bool IsMoving(this UnitMoveComponent self)
        {
            return self.m_bIsMoving;
        }

        public static bool IsNormalMove(EMoveType eMoveType)
        {
            if (eMoveType == EMoveType.ENormal)
                return true;
            if (eMoveType == EMoveType.ESystem)
                return true;
            if (eMoveType == EMoveType.ESystemIgnoreTerrain)
                return true;
            return false;
        }

        private static byte GetMoveMask(this UnitMoveComponent self)
        {
            byte result = byte.MaxValue;
            switch (self.m_eMoveType)
            {
                case EMoveType.ENone:
                    break;
                case EMoveType.ENormal:
                    break;
                case EMoveType.ESystem:
                    break;
                case EMoveType.EHurtMove:
                    break;
                case EMoveType.EHurtMoveIgnoreAll:
                    result = 0;
                    break;
                case EMoveType.ESkillMove:
                    break;
                case EMoveType.ESkillMoveIgnoreTerrain:
                case EMoveType.ESystemIgnoreTerrain:
                    result = 0;
                    break;
                case EMoveType.EFear:
                    break;
            }

            return result;
        }

        private static bool CheckObstacle(this UnitMoveComponent self, SVector3 sNewPos)
        {
            if (self.m_eMoveType == EMoveType.ESkillMoveIgnoreTerrain ||
                self.m_eMoveType == EMoveType.ESystemIgnoreTerrain ||
                self.m_eMoveType == EMoveType.EHurtMoveIgnoreAll)
                return false;
            var battleSceneComponent = self.GetMobaBattleComponent().GetComponent<BattleSceneComponent>();
            return !battleSceneComponent.m_cMap.IsReachable(sNewPos, self.GetMoveMask());
        }

        private static void ProcessNextPoint(this UnitMoveComponent self)
        {
            if (self.m_quePath.Count <= 0)
                return;
            self.m_sNextPosition = self.m_quePath.Dequeue();
            if (self.m_bChangeForward)
                self.Master.SetForward((self.m_sNextPosition - self.Master.LogicPos).normalizedXz, false,
                    EForwardType.EMove);
            Game.EventSystem.Publish(new MobaUnitTargetPos()
                { unit = self.Master, targetPos = self.m_sNextPosition, speed = self.m_nSpeed });
        }

        private static void Begin(this UnitMoveComponent self, int nSpeed, bool bChangeForward, EMoveType eMoveType)
        {
            self.m_bIsMoving = true;
            self.m_nSpeed = nSpeed;
            self.m_bChangeForward = bChangeForward;
            self.m_eMoveType = eMoveType;
            Game.EventSystem.Publish(new EventType.MobaUnitBeginMove { unit = self.GetParent<Unit>() });
            self.ProcessNextPoint();
        }

        #region 切换状态机

        public static void Move(this UnitMoveComponent self, int arg1, int arg2)
        {
            self.Arg1 = arg1;
            self.Arg2 = arg2;

            // TODO ChangeFsmState
            // TODO 角色播放动画.
            // TODO 角色进行移动.

            var unit = self.GetParent<Unit>();
            int angle = arg1;
            SVector3 inputForward = (SQuaternion.AngleAxis(angle, SVector3.up) * unit.BornForward).normalizedXz;
            int nSpeed = 10000;
            var m_sNextPosition = unit.LogicPos + inputForward * nSpeed / 1000 * self.GetFrameSyncComponent().LOGIC_FRAME_DELTA / 1000;
            self.Move(m_sNextPosition, nSpeed, true, EMoveType.ENormal);
        }

        public static void MoveEnd(this UnitMoveComponent self)
        {
            self.Arg1 = 0;
            self.Arg2 = 0;

            // TODO ChangeFsmState
            self.Stop();
        }

        #endregion
    }
}