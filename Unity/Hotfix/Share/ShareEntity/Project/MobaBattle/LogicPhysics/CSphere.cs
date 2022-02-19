namespace ET
{
    public class CSphere : CCollider
    {
        public SVector3 center;
        public int radius;

        public static bool Raycast(
            SVector3 sOrigin,
            SVector3 sDirection,
            int nDis,
            SVector3 sCenter,
            int nRadius)
        {
            SVector3 b = sCenter - sOrigin;
            long num1 = SVector3.Dot(sDirection, b) / 1000L;
            long num2 = num1 * num1 + (long)nRadius * (long)nRadius - b.sqrMagnitude;
            if (num2 >= 0L)
            {
                int num3 = CMath.Sqrt(num2);
                long num4 = num1 - (long)num3;
                long num5 = num1 + (long)num3;
                if (CMath.Abs(num4) > CMath.Abs(num5))
                {
                    long num6 = num4;
                    num4 = num5;
                    num5 = num6;
                }

                if (num4 >= 0L && num4 - (long)nDis <= 0L || num5 >= 0L && num5 - (long)nDis <= 0L)
                    return true;
            }

            return false;
        }

        public override bool Raycast(
            SVector3 sOrigin,
            SVector3 sDirection,
            int nDis,
            out SVector3 sCrossPos,
            out SVector3 sNormal)
        {
            SVector3 b = this.center - sOrigin;
            long num1 = SVector3.Dot(sDirection, b) / 1000L;
            long num2 = num1 * num1 + (long)this.radius * (long)this.radius - b.sqrMagnitude;
            sCrossPos = SVector3.zero;
            sNormal = SVector3.zero;
            if (num2 >= 0L)
            {
                int num3 = CMath.Sqrt(num2);
                long num4 = num1 - (long)num3;
                long num5 = num1 + (long)num3;
                if (CMath.Abs(num4) > CMath.Abs(num5))
                {
                    long num6 = num4;
                    num4 = num5;
                    num5 = num6;
                }

                if (num4 >= 0L && num4 - (long)nDis <= 0L)
                {
                    sCrossPos = sOrigin + (int)num4 * sDirection / 1000;
                    sNormal = sCrossPos - this.center;
                    sNormal.Normalize();
                    return true;
                }

                if (num5 >= 0L && num5 - (long)nDis <= 0L)
                {
                    sCrossPos = sOrigin + (int)num5 * sDirection / 1000;
                    sNormal = sCrossPos - this.center;
                    sNormal.Normalize();
                    return true;
                }
            }

            return false;
        }

        public override void UpdateCollider()
        {
            // SphereCollider component = this.GetComponent<SphereCollider>();
            // Vector3 vector3 = this.transform.position + component.center;
            // this.center.x = (int)((double)vector3.x * 1000.0);
            // this.center.y = (int)((double)vector3.y * 1000.0);
            // this.center.z = (int)((double)vector3.z * 1000.0);
            // Vector3 lossyScale = this.transform.lossyScale;
            // float num = Mathf.Max(lossyScale.x, lossyScale.y, lossyScale.z);
            // this.radius = (int)((double)component.radius * (double)num * 1000.0);
        }

        public override int CheckAabb(SVector3 sCenter, int nHalfWidth, int nHalfHeight)
        {
            bool flag = false;
            SVector3 center = this.center;
            center.x -= this.radius;
            if (CPhysics.CheckAabbAndPos(sCenter, nHalfWidth, nHalfHeight, center))
                flag = true;
            center.x = this.center.x + this.radius;
            if (CPhysics.CheckAabbAndPos(sCenter, nHalfWidth, nHalfHeight, center))
                flag = true;
            else if (flag)
                return 1;
            center.x = this.center.x;
            center.z = this.center.z - this.radius;
            if (CPhysics.CheckAabbAndPos(sCenter, nHalfWidth, nHalfHeight, center))
                flag = true;
            else if (flag)
                return 1;
            center.z = this.center.z + this.radius;
            if (CPhysics.CheckAabbAndPos(sCenter, nHalfWidth, nHalfHeight, center))
                flag = true;
            if (flag)
                return 2;
            return CPhysics.CheckAabbAndPos(this.center, this.radius, this.radius, sCenter) ? 1 : 0;
        }
    }
}