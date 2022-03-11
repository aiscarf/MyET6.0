namespace ET
{
    public class Cell
    {
        public int F;
        public int G;
        public int H;
        public SVector3 centerPos;
        public short columnIndex;
        public short rowIndex;
        public bool close;
        public byte mFlagValue;
        public Cell parent;
        public byte mDynamicCount;
        public int m_nSession;

        public Cell(SVector3 centerPos, byte flag)
        {
            this.centerPos = centerPos;
            this.mFlagValue = flag;
            this.mDynamicCount = (byte)0;
        }

        public int Session
        {
            get { return this.m_nSession; }
            set
            {
                if (this.m_nSession != value)
                {
                    this.G = 0;
                    this.F = 0;
                    this.H = 0;
                    this.parent = (Cell)null;
                    this.close = false;
                }

                this.m_nSession = value;
            }
        }

        public bool Walkable(byte mask)
        {
            return ((int)this.mFlagValue & (int)mask) == 0;
        }

        public bool IsObstacle()
        {
            return this.mFlagValue != (byte)0;
        }

        // public bool IsObstacle(ObstacleType cType)
        // {
        //     return ((ObstacleType)this.mFlagValue & cType) != ~ObstacleType.All;
        // }

        public int CheckIntersect(SVector3 sOrgPos, SVector3 sOffset, int nCellSize)
        {
            int num = nCellSize / 2;
            return CPhysics.CheckAabbAndLine(this.centerPos, num, num, sOrgPos, sOffset, sOffset.magnitude);
        }

        public void Clear()
        {
            this.parent = (Cell)null;
        }
    }
}