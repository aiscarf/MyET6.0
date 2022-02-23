using System.Collections.Generic;

namespace ET
{
    public class UnitMoveComponent : Entity
    {
        public Unit Master;
        public bool m_bChangeForward;
        public int m_nSpeed;
        public SVector3 m_sNextPosition;
        public EMoveType m_eMoveType;
        public Queue<SVector3> m_quePath = new Queue<SVector3>();
        public bool m_bIsMoving;

        public int Arg1;
        public int Arg2;
    }
}