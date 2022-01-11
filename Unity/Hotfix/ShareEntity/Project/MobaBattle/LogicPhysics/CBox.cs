namespace ET
{
    public class CBox : CCollider
    {
        private static long[] arrDirDotAxis = new long[3];
        private static long[] arrOcDotAxis = new long[3];
        private static int[] arrExtend = new int[3];
        private static SVector3[] arrAxis = new SVector3[3];
        public SVector3 center;
        public SVector3 size;
        public SVector3 xAxis;
        public SVector3 yAxis;
        public SVector3 zAxis;
        public int radius;

        public override bool Raycast(
            SVector3 sOrigin,
            SVector3 sDirection,
            int nMaxDistance,
            out SVector3 sCrossPos,
            out SVector3 sNormal)
        {
            sCrossPos = SVector3.zero;
            sNormal = SVector3.zero;
            int num1 = 0;
            bool flag = false;
            long num2 = 0;
            long num3 = 0;
            int index1 = 0;
            int num4 = 0;
            SVector3 a = this.center - sOrigin;
            CBox.arrExtend[0] = this.size.x;
            CBox.arrExtend[1] = this.size.y;
            CBox.arrExtend[2] = this.size.z;
            CBox.arrAxis[0] = this.xAxis;
            CBox.arrAxis[1] = this.yAxis;
            CBox.arrAxis[2] = this.zAxis;
            for (int index2 = 0; index2 < 3; ++index2)
            {
                CBox.arrDirDotAxis[index2] = SVector3.Dot(sDirection, CBox.arrAxis[index2]);
                CBox.arrOcDotAxis[index2] = SVector3.Dot(a, CBox.arrAxis[index2]);
                if (CMath.Abs(CBox.arrDirDotAxis[index2]) == 0L)
                {
                    num1 |= 1 << index2;
                }
                else
                {
                    int num5 = (CBox.arrDirDotAxis[index2] <= 0L ? -CBox.arrExtend[index2] : CBox.arrExtend[index2]) *
                               500;
                    if (!flag)
                    {
                        num2 = 1000L * (CBox.arrOcDotAxis[index2] - (long)num5) / CBox.arrDirDotAxis[index2];
                        num3 = 1000L * (CBox.arrOcDotAxis[index2] + (long)num5) / CBox.arrDirDotAxis[index2];
                        index1 = num4 = index2;
                        flag = true;
                    }
                    else
                    {
                        long num6 = 1000L * (CBox.arrOcDotAxis[index2] - (long)num5) / CBox.arrDirDotAxis[index2];
                        if (num6 > num2)
                        {
                            num2 = num6;
                            index1 = index2;
                        }

                        long num7 = 1000L * (CBox.arrOcDotAxis[index2] + (long)num5) / CBox.arrDirDotAxis[index2];
                        if (num7 < num3)
                        {
                            num3 = num7;
                            num4 = index2;
                        }
                    }

                    if (num2 > num3)
                        return false;
                }
            }

            if (num1 > 0)
            {
                for (int index2 = 0; index2 < 3; ++index2)
                {
                    if ((num1 & 1 << index2) > 0 &&
                        (CMath.Abs(CBox.arrOcDotAxis[index2] - num2 * CBox.arrDirDotAxis[index2] / 1000L) >
                         500L * (long)CBox.arrExtend[index2] ||
                         CMath.Abs(CBox.arrOcDotAxis[index2] - num3 * CBox.arrDirDotAxis[index2] / 1000L) >
                         500L * (long)CBox.arrExtend[index2]))
                        return false;
                }
            }

            if (num2 < 0L)
            {
                if (num3 < 0L)
                    return false;
                num2 = num3;
                index1 = num4;
            }

            if (num2 > (long)nMaxDistance)
                return false;
            int x = (int)((long)sDirection.x * num2 / 1000L);
            int y = (int)((long)sDirection.y * num2 / 1000L);
            int z = (int)((long)sDirection.z * num2 / 1000L);
            sCrossPos = sOrigin + new SVector3(x, y, z);
            SVector3 svector3 = sCrossPos - this.center;
            sNormal = SVector3.Dot(CBox.arrAxis[index1], sCrossPos) < 0L ? -CBox.arrAxis[index1] : CBox.arrAxis[index1];
            return true;
        }

        public override void UpdateCollider()
        {
            // BoxCollider component = this.GetComponent<BoxCollider>();
            // Vector3 vector3 = this.transform.position + component.center;
            // this.center.x = (int)((double)vector3.x * 1000.0);
            // this.center.y = (int)((double)vector3.y * 1000.0);
            // this.center.z = (int)((double)vector3.z * 1000.0);
            // Vector3 lossyScale = this.transform.lossyScale;
            // Vector3 size = component.size;
            // this.size.x = (int)((double)size.x * (double)lossyScale.x * 1000.0);
            // this.size.y = (int)((double)size.y * (double)lossyScale.y * 1000.0);
            // this.size.z = (int)((double)size.z * (double)lossyScale.z * 1000.0);
            // Vector3 right = this.transform.right;
            // this.xAxis.x = (int)((double)right.x * 1000.0);
            // this.xAxis.y = (int)((double)right.y * 1000.0);
            // this.xAxis.z = (int)((double)right.z * 1000.0);
            // this.xAxis.Normalize();
            // Vector3 up = this.transform.up;
            // this.yAxis.x = (int)((double)up.x * 1000.0);
            // this.yAxis.y = (int)((double)up.y * 1000.0);
            // this.yAxis.z = (int)((double)up.z * 1000.0);
            // this.yAxis.Normalize();
            // this.zAxis = SVector3.Cross(this.xAxis, this.yAxis);
            // this.zAxis.Normalize();
            // this.radius = CMath.Sqrt((long)this.size.x * (long)this.size.x + (long)this.size.y * (long)this.size.y +
            //                          (long)this.size.z * (long)this.size.z);
        }

        public override int CheckAabb(SVector3 sCenter, int nHalfWidth, int nHalfHeight)
        {
            bool flag = false;
            SVector3 svector3_1 = this.xAxis * this.size.x / 1000;
            SVector3 svector3_2 = this.yAxis * this.size.y / 1000;
            SVector3 svector3_3 = this.zAxis * this.size.z / 1000;
            SVector3 sPos1 = this.center - (svector3_1 + svector3_2 + svector3_3) / 2;
            if (CPhysics.CheckAabbAndPos(sCenter, nHalfWidth, nHalfHeight, sPos1))
                flag = true;
            SVector3 sPos2 = sPos1 + svector3_1;
            if (CPhysics.CheckAabbAndPos(sCenter, nHalfWidth, nHalfHeight, sPos2))
            {
                if (!flag)
                    return 1;
            }
            else if (flag)
                return 1;

            SVector3 sPos3 = sPos2 + svector3_2;
            if (CPhysics.CheckAabbAndPos(sCenter, nHalfWidth, nHalfHeight, sPos3))
            {
                if (!flag)
                    return 1;
            }
            else if (flag)
                return 1;

            SVector3 sPos4 = sPos3 - svector3_1;
            if (CPhysics.CheckAabbAndPos(sCenter, nHalfWidth, nHalfHeight, sPos4))
            {
                if (!flag)
                    return 1;
            }
            else if (flag)
                return 1;

            SVector3 sPos5 = sPos4 + svector3_3;
            if (CPhysics.CheckAabbAndPos(sCenter, nHalfWidth, nHalfHeight, sPos5))
            {
                if (!flag)
                    return 1;
            }
            else if (flag)
                return 1;

            SVector3 sPos6 = sPos5 + svector3_1;
            if (CPhysics.CheckAabbAndPos(sCenter, nHalfWidth, nHalfHeight, sPos6))
            {
                if (!flag)
                    return 1;
            }
            else if (flag)
                return 1;

            SVector3 sPos7 = sPos6 - svector3_2;
            if (CPhysics.CheckAabbAndPos(sCenter, nHalfWidth, nHalfHeight, sPos7))
            {
                if (!flag)
                    return 1;
            }
            else if (flag)
                return 1;

            SVector3 sPos8 = sPos7 - svector3_1;
            if (CPhysics.CheckAabbAndPos(sCenter, nHalfWidth, nHalfHeight, sPos8))
            {
                if (!flag)
                    return 1;
            }
            else if (flag)
                return 1;

            return flag ? 2 : 0;
        }
    }
}