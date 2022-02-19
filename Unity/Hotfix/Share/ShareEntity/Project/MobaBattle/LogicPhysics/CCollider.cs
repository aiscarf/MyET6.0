namespace ET
{
    public class CCollider
    {
        public virtual bool Raycast(
            SVector3 sOrigin,
            SVector3 sDirection,
            int nDis,
            out SVector3 sCrossPos,
            out SVector3 sNormal)
        {
            sCrossPos = SVector3.zero;
            sNormal = SVector3.zero;
            return false;
        }

        public virtual void UpdateCollider()
        {
        }

        public virtual int CheckAabb(SVector3 sCenter, int nHalfWidth, int nHalfHeight)
        {
            return 0;
        }

        private void Update()
        {
            UpdateCollider();
        }
    }
}