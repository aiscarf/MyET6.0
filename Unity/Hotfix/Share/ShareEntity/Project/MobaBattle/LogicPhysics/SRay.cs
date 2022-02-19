using System;

namespace ET
{
    [Serializable]
    public struct SRay
    {
        public SVector3 direction;
        public SVector3 origin;

        public SRay(SVector3 sOrigin, SVector3 sDirection)
        {
            this.origin = sOrigin;
            this.direction = sDirection;
        }

        public SVector3 GetPoint(int nDistance)
        {
            return this.origin + this.direction * nDistance;
        }

        public override string ToString()
        {
            return this.origin.ToString() + ", " + this.direction.ToString();
        }
    }
}