namespace ET
{
    public struct CRaycastHit
    {
        public CCollider collider;
        public int distance;
        public SVector3 point;
        public SVector3 normal;
    }
}