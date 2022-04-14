using System.Collections.Generic;

namespace Scarf.Moba
{
    public class UnitMoveComponent: CComponent
    {
        public Unit Master { get; set; }
        private bool m_bChangeForward;
        private int m_nSpeed;
        private SVector3 m_sNextPosition;
        private EMoveType m_eMoveType;
        private Queue<SVector3> m_quePath = new Queue<SVector3>();
        private bool m_bIsMoving;

        private int Arg1;
        private int Arg2;

        protected override void OnInit()
        {
            this.Master = this.Parent as Unit;
        }

        public override void OnFrameSyncUpdate(int delta)
        {
            if (!this.m_bIsMoving)
                return;
            byte mask = this.GetMoveMask();
            int num = this.m_nSpeed;
            SVector3 svector3_1 = this.m_sNextPosition - this.Master.LogicPos;
            SVector3 svector3_2 = svector3_1.normalizedXz * num / 1000 * delta / 1000;

            bool b = svector3_1.sqrMagnitudeXz <= svector3_2.sqrMagnitudeXz;
            SVector3 posTarget = b? this.m_sNextPosition : this.Master.LogicPos + svector3_2;

            if (this.CheckObstacle(posTarget))
            {
                this.Battle.BattleScene.Pathfinding.GetReachablePosition(this.Master.LogicPos, ref posTarget, true, mask);
                this.Master.UpdateLogicPos(posTarget);
                this.Stop();
                return;
            }

            this.Master.UpdateLogicPos(this.m_sNextPosition);

            if (!b)
                return;

            if (this.m_quePath.Count <= 0)
            {
                this.Stop();
                return;
            }

            this.ProcessNextPoint();
        }

        public bool Move(List<SVector3> lstPath, int nSpeed, bool bChangeForward, EMoveType eMoveType = EMoveType.ENormal)
        {
            if (!this.CanMove(eMoveType) || lstPath.Count <= 0)
                return false;
            this.m_quePath.Clear();
            for (int index = 0; index < lstPath.Count; ++index)
                this.m_quePath.Enqueue(lstPath[index]);
            this.Begin(nSpeed, bChangeForward, eMoveType);
            return true;
        }

        public bool Move(SVector3 sTargetPos, int nSpeed, bool bChangeForward, EMoveType eMoveType = EMoveType.ENormal)
        {
            if (!this.CanMove(eMoveType) || SVector3.EqualsXz(sTargetPos, this.Master.LogicPos))
                return false;
            this.m_quePath.Clear();
            this.m_quePath.Enqueue(sTargetPos);
            this.Begin(nSpeed, bChangeForward, eMoveType);
            return true;
        }

        public void Stop()
        {
            if (!this.m_bIsMoving)
                return;
            this.m_nSpeed = 0;
            this.m_bIsMoving = false;
            this.m_eMoveType = EMoveType.ENone;

            this.Battle.EventMgr.Publish(new EventType.UnitMoveEnd() { unit = this.Master });
        }

        public bool CanMove(EMoveType eMoveType = EMoveType.ENormal)
        {
            if (!IsNormalMove(eMoveType))
                return true;
            return !this.Master.IsDie;
        }

        public bool CanStopMove()
        {
            return this.Master.IsDie || this.IsMoving() && IsNormalMove(this.m_eMoveType);
        }

        public bool IsMoving()
        {
            return this.m_bIsMoving;
        }

        public bool IsNormalMove(EMoveType eMoveType)
        {
            if (eMoveType == EMoveType.ENormal)
                return true;
            if (eMoveType == EMoveType.ESystem)
                return true;
            if (eMoveType == EMoveType.ESystemIgnoreTerrain)
                return true;
            return false;
        }

        private byte GetMoveMask()
        {
            byte result = byte.MaxValue;
            switch (this.m_eMoveType)
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

        private bool CheckObstacle(SVector3 sNewPos)
        {
            if (this.m_eMoveType == EMoveType.ESkillMoveIgnoreTerrain ||
                this.m_eMoveType == EMoveType.ESystemIgnoreTerrain ||
                this.m_eMoveType == EMoveType.EHurtMoveIgnoreAll)
                return false;
            return !this.Battle.BattleScene.Map.IsReachable(sNewPos, this.GetMoveMask());
        }

        private void ProcessNextPoint()
        {
            if (this.m_quePath.Count <= 0)
                return;
            this.m_sNextPosition = this.m_quePath.Dequeue();
            if (this.m_bChangeForward)
                this.Master.UnitRotate?.SetForward((this.m_sNextPosition - this.Master.LogicPos).normalizedXz, false, EForwardType.EMove);
            this.Battle.EventMgr.Publish(
                new EventType.UnitTargetPos() { unit = this.Master, targetPos = this.m_sNextPosition, speed = this.m_nSpeed });
        }

        private void Begin(int nSpeed, bool bChangeForward, EMoveType eMoveType)
        {
            this.m_bIsMoving = true;
            this.m_nSpeed = nSpeed;
            this.m_bChangeForward = bChangeForward;
            this.m_eMoveType = eMoveType;
            this.Battle.EventMgr.Publish(new EventType.UnitMoveBegin { unit = this.Master, speed = this.m_nSpeed });
            this.ProcessNextPoint();
        }

        #region 切换状态机

        public void Move(int arg1, int arg2)
        {
            this.Arg1 = arg1;
            this.Arg2 = arg2;

            // TODO ChangeFsmState
            // TODO 角色播放动画.
            // TODO 角色进行移动.

            int angle = arg1;
            SVector3 inputForward = (SQuaternion.AngleAxis(angle, SVector3.up) * this.Master.BornForward).normalizedXz;
            int nSpeed = 10000;
            this.m_sNextPosition = this.Master.LogicPos + inputForward * nSpeed / 1000 * 100 / 1000;
            this.Move(m_sNextPosition, nSpeed, true, EMoveType.ENormal);
        }

        public void MoveEnd()
        {
            this.Arg1 = 0;
            this.Arg2 = 0;
            this.Stop();
        }

        #endregion
    }
}