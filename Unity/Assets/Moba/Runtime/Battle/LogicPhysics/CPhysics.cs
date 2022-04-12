namespace Scarf.Moba
{
    public class CPhysics
    {
        public const int MAX_LENGHT = 2100000;
        private static CQuadTree m_cCollisionTree;

        public static bool Raycast(SRay sRay, int layerMask = 0)
        {
            return CPhysics.Raycast(sRay.origin, sRay.direction, layerMask);
        }

        public static bool Raycast(SRay sRay, int nMaxDistance, int layerMask = 0)
        {
            return CPhysics.Raycast(sRay.origin, sRay.direction, nMaxDistance, layerMask);
        }

        public static bool Raycast(SRay sRay, out CRaycastHit hitInfo, int layerMask = 0)
        {
            return CPhysics.Raycast(sRay.origin, sRay.direction, out hitInfo, layerMask);
        }

        public static bool Raycast(SRay sRay, int nMaxDistance, out CRaycastHit hitInfo, int layerMask = 0)
        {
            return CPhysics.Raycast(sRay.origin, sRay.direction, nMaxDistance, out hitInfo, layerMask);
        }

        public static int RaycastAll(SRay sRay, CRaycastHit[] results, int layerMask = 0)
        {
            return CPhysics.RaycastAll(sRay.origin, sRay.direction, results, layerMask);
        }

        public static int RaycastAll(SRay sRay, int nMaxDistance, CRaycastHit[] results, int layerMask = 0)
        {
            return CPhysics.RaycastAll(sRay.origin, sRay.direction, nMaxDistance, results, layerMask);
        }

        public static bool Raycast(SVector3 sOrigin, SVector3 sDirection, int layerMask = 0)
        {
            return CPhysics.Raycast(sOrigin, sDirection, 2100000, layerMask);
        }

        public static bool Raycast(
            SVector3 sOrigin,
            SVector3 sDirection,
            int nMaxDistance,
            int layerMask = 0)
        {
            return nMaxDistance <= 2100000 && CPhysics.m_cCollisionTree != null &&
                   CPhysics.m_cCollisionTree.Raycast(sOrigin, sDirection, nMaxDistance, layerMask);
        }

        public static bool Raycast(
            SVector3 sOrigin,
            SVector3 sDirection,
            out CRaycastHit hitInfo,
            int layerMask = 0)
        {
            return CPhysics.Raycast(sOrigin, sDirection, 2100000, out hitInfo, layerMask);
        }

        public static bool Raycast(
            SVector3 sOrigin,
            SVector3 sDirection,
            int nMaxDistance,
            out CRaycastHit hitInfo,
            int layerMask = 0)
        {
            if (nMaxDistance <= 2100000 && CPhysics.m_cCollisionTree != null)
                return CPhysics.m_cCollisionTree.Raycast(sOrigin, sDirection, nMaxDistance, out hitInfo, layerMask);
            hitInfo.collider = (CCollider)null;
            hitInfo.distance = 0;
            hitInfo.point = SVector3.zero;
            hitInfo.normal = SVector3.zero;
            return false;
        }

        public static int RaycastAll(
            SVector3 sOrigin,
            SVector3 sDirection,
            CRaycastHit[] results,
            int layerMask = 0)
        {
            return CPhysics.RaycastAll(sOrigin, sDirection, 2100000, results, layerMask);
        }

        public static int RaycastAll(
            SVector3 sOrigin,
            SVector3 sDirection,
            int nMaxDistance,
            CRaycastHit[] results,
            int layerMask = 0)
        {
            return results == null || results.Length == 0 ||
                   (nMaxDistance > 2100000 || CPhysics.m_cCollisionTree == null)
                ? 0
                : CPhysics.m_cCollisionTree.RaycastAll(sOrigin, sDirection, nMaxDistance, results, layerMask);
        }

        public static bool CheckRectangleAndPos(
            SVector3 sCenter,
            SVector3 sDir,
            int nHalfWidth,
            int nHalfHeight,
            SVector3 sPos)
        {
            sDir.y = 0;
            int num1 = SVector3.Angle(sDir, SVector3.forward);
            if ((double)sDir.x < 0.0)
                num1 = 360 - num1;
            int num2 = CMath.Cos(num1);
            int num3 = CMath.Sin(num1);
            int num4 = sPos.x - sCenter.x;
            int num5 = sPos.z - sCenter.z;
            int num6 = (num4 * num2 - num5 * num3 + sCenter.x * 1000) / 1000;
            int num7 = (num4 * num3 + num5 * num2 + sCenter.z * 1000) / 1000;
            sPos.x = num6;
            sPos.z = num7;
            return CPhysics.CheckAabbAndPos(sCenter, nHalfWidth, nHalfHeight, sPos);
        }

        public static bool CheckAabbAndPos(
            SVector3 sCenter,
            int nHalfWidth,
            int nHalfHeight,
            SVector3 sPos)
        {
            SVector3 svector3 = sPos - sCenter;
            return CMath.Abs(svector3.x) <= nHalfWidth && CMath.Abs(svector3.z) <= nHalfHeight;
        }

        public static bool CheckRectangleAndLine(
            SVector3 sCenter,
            SVector3 sDir,
            int nHalfWidth,
            int nHalfHeight,
            SVector3 sOrgPos,
            ref SVector3 sOffset)
        {
            sDir.y = 0;
            int num1 = SVector3.Angle(sDir, SVector3.forward);
            if ((double)sDir.x < 0.0)
                num1 = 360 - num1;
            SVector3 svector3 = sOrgPos + sOffset;
            int num2 = CMath.Cos(num1);
            int num3 = CMath.Sin(num1);
            int num4 = sOrgPos.x - sCenter.x;
            int num5 = sOrgPos.z - sCenter.z;
            int num6 = (num4 * num2 - num5 * num3 + sCenter.x * 1000) / 1000;
            int num7 = (num4 * num3 + num5 * num2 + sCenter.z * 1000) / 1000;
            sOrgPos.x = num6;
            sOrgPos.z = num7;
            int num8 = svector3.x - sCenter.x;
            int num9 = svector3.z - sCenter.z;
            int num10 = (num8 * num2 - num9 * num3 + sCenter.x * 1000) / 1000;
            int num11 = (num8 * num3 + num9 * num2 + sCenter.z * 1000) / 1000;
            svector3.x = num10;
            svector3.z = num11;
            sOffset = svector3 - sOrgPos;
            int magnitudeXz = sOffset.magnitudeXz;
            sOffset.NormalizeXz();
            int num12 = CPhysics.CheckAabbAndLine(sCenter, nHalfWidth, nHalfHeight, sOrgPos, sOffset, magnitudeXz);
            if (num12 < 0)
                return false;
            sOffset = sOffset * num12 / 1000;
            return true;
        }

        public static bool CheckRectangleAndCircle(
            SVector3 sCenter,
            SVector3 sDir,
            int nHalfWidth,
            int nHalfHeight,
            SVector3 sCircleCenter,
            int nRadius)
        {
            sDir.NormalizeXz();
            SVector3 svector3 = new SVector3(-sDir.z, 0, sDir.x);
            SVector3 sPos2_1 = sCenter + svector3 * nHalfHeight / 1000;
            SVector3 sPos2_2 = sCenter + sDir * nHalfWidth / 1000;
            int line1 = CPhysics.DistanceFromPointToLine(sCircleCenter, sCenter, sPos2_2);
            int line2 = CPhysics.DistanceFromPointToLine(sCircleCenter, sCenter, sPos2_1);
            if (line1 > nHalfWidth + nRadius || line2 > nHalfHeight + nRadius)
                return false;
            return line1 <= nHalfWidth || line2 <= nHalfHeight ||
                   (line1 - nHalfWidth) * (line1 - nHalfWidth) + (line2 - nHalfHeight) * (line2 - nHalfHeight) <=
                   nRadius * nRadius;
        }

        public static bool CheckRectangleAndCircle2(SVector3 sCenter,
            SVector3 sDir,
            int nHalfWidth,
            int nHalfHeight,
            SVector3 sCircleCenter,
            int nRadius)
        {
            sDir.y = 0;
            int num1 = SVector3.Angle(SVector3.forward, sDir);
            if ((double)sDir.x < 0.0)
                num1 = 360 - num1;
            SVector3 sv0 = sCircleCenter - sCenter;

            int num1_1 = sv0.x;
            int num2_2 = sv0.z;
            int num3_3 = CMath.Cos(num1);
            int num4_4 = CMath.Sin(num1);
            int num5_5 = num1_1 * num3_3 - num2_2 * num4_4;
            int num6_6 = num1_1 * num4_4 + num2_2 * num3_3;

//      SVector3 sv1 = SVector3.RotateAroundPoint(SVector3.zero, sv0, num1);
            int num2 = CMath.Abs(num5_5);
            int num3 = CMath.Abs(num6_6);
            int num4 = num2 - nHalfWidth * 1000;
            int num5 = num3 - nHalfHeight * 1000;
            num4 = num4 < 0 ? 0 : num4;
            num5 = num5 < 0 ? 0 : num5;
            long num8 = (long)num4 * (long)num4 + (long)num5 * (long)num5;
            return num8 / 1000000L <= (long)nRadius * (long)nRadius;
        }

        // 轴对齐包围盒碰撞
        public static bool CheckRectangleAndRectangle(SVector3 rectCenter1, int halfWight1, int halfHeight1,
            SVector3 rectCenter2, int halfWight2, int halfHeight2)
        {
            int num_1_1 = rectCenter1.x - halfWight1;
            int num_1_2 = rectCenter1.x + halfWight1;
            int num_1_3 = rectCenter1.z - halfWight1;
            int num_1_4 = rectCenter1.z + halfWight1;

            int num_2_1 = rectCenter2.x - halfWight2;
            int num_2_2 = rectCenter2.x + halfWight2;
            int num_2_3 = rectCenter2.z - halfHeight2;
            int num_2_4 = rectCenter2.z + halfHeight2;

            bool bX = false;
            if ((num_1_1 - num_2_1) * (num_1_1 - num_2_2) < 0)
            {
                bX = true;
            }
            else if ((num_1_2 - num_2_1) * (num_1_2 - num_2_2) < 0)
            {
                bX = true;
            }

            if (bX)
            {
                if ((num_1_3 - num_2_3) * (num_1_3 - num_2_4) < 0)
                {
                    return true;
                }
                else if ((num_1_4 - num_2_3) * (num_1_4 - num_2_4) < 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool CheckRectangleAndSector(
            SVector3 sCenter,
            SVector3 sDir,
            int nHalfWidth,
            int nHalfHeight,
            SVector3 sSectorCenter,
            SVector3 sSectorForward,
            int nSectorRadius,
            int nSectorAngles)
        {
            return CPhysics.CheckRectangleAndCircle(sCenter, sDir, nHalfWidth, nHalfHeight, sSectorCenter,
                nSectorRadius);
        }

        public static bool CheckCircleAndLine(
            SVector3 sOrgPos,
            SVector3 sOffset,
            SVector3 sCenter,
            int nRadius,
            out SVector3 sCrossPoint)
        {
            sCrossPoint = sCenter;
            sCenter.y = 0;
            sOrgPos.y = 0;
            sOffset.y = 0;
            int magnitude = sOffset.magnitude;
            SVector3 normalized = sOffset.normalized;
            SVector3 svector3 = sCenter - sOrgPos;
            int num1 = (svector3.x * normalized.x + svector3.z * normalized.z) / normalized.magnitude;
            long num2 = (long)num1 * (long)num1 + (long)nRadius * (long)nRadius - svector3.sqrMagnitude;
            if (num2 >= 0L)
            {
                int num3 = CMath.Sqrt(num2);
                int num4 = num1 - num3;
                int num5 = num1 + num3;
                if (CMath.Abs(num4) > CMath.Abs(num5))
                {
                    int num6 = num4;
                    num4 = num5;
                    num5 = num6;
                }

                if (num4 >= 0 && num4 - magnitude <= 0)
                {
                    sCrossPoint.x = sOrgPos.x + num4 * normalized.x / 1000;
                    sCrossPoint.z = sOrgPos.z + num4 * normalized.z / 1000;
                    return true;
                }

                if (num5 >= 0 && num5 - magnitude <= 0)
                {
                    sCrossPoint.x = sOrgPos.x + num5 * normalized.x / 1000;
                    sCrossPoint.z = sOrgPos.z + num5 * normalized.z / 1000;
                    return true;
                }
            }

            return false;
        }

        public static bool CheckCircleAndLine(
            SVector3 sOrgPos,
            ref SVector3 sOffset,
            SVector3 sCenter,
            int nRadius)
        {
            SVector3 sCrossPoint;
            if (!CPhysics.CheckCircleAndLine(sOrgPos, sOffset, sCenter, nRadius, out sCrossPoint))
                return false;
            sOffset = sCrossPoint - sOrgPos;
            sOffset.y = 0;
            return true;
        }

        public static bool CheckCircleAndPos(SVector3 sCenter, int nRadius, SVector3 sPostion)
        {
            return (sCenter - sPostion).sqrMagnitudeXz <= (long)nRadius * (long)nRadius;
        }

        public static bool CheckSectorAndPos(
            SVector3 sCenter,
            SVector3 sForward,
            int nRadius,
            int nAngle,
            SVector3 sPos)
        {
            if (sPos == sCenter)
                return true;
            SVector3 sFrom = sPos - sCenter;
            sFrom.y = 0;
            return SVector3.Angle(sFrom, sForward) <= nAngle / 2 &&
                   (sCenter - sPos).sqrMagnitude < (long)nRadius * (long)nRadius;
        }

        public static bool CheckCircleAndCircle(
            SVector3 sCenter,
            int nRadius,
            SVector3 sCenter2,
            int nRadius2)
        {
            int num1 = sCenter.x - sCenter2.x;
            int num2 = sCenter.z - sCenter2.z;
            int num3 = nRadius + nRadius2;
            return (long)num1 * (long)num1 + (long)num2 * (long)num2 < (long)num3 * (long)num3;
        }

        public static bool CheckCircleAndSector(
            SVector3 sCircleCenter,
            int nCircleRadius,
            SVector3 sSectorCenter,
            SVector3 sSectorForward,
            int nSectorRadius,
            int nSectorAngle)
        {
            SVector3 a1 = sCircleCenter - sSectorCenter;
            a1.y = 0;
            int num1 = nCircleRadius + nSectorRadius;
            if (a1.sqrMagnitude > (long)(num1 * num1))
                return false;
            sSectorForward.NormalizeXz();
            int x = (int)(SVector3.Dot(a1, sSectorForward) / 1000L);
            int z = CMath.Abs(-a1.x * sSectorForward.z + a1.z * sSectorForward.x) / 1000;
            int num2 = nSectorAngle / 2;
            if (x > a1.magnitude * CMath.Cos(num2) / 1000)
                return true;
            SVector3 b = nSectorRadius * new SVector3(CMath.Cos(num2), 0, CMath.Sin(num2)) / 1000;
            SVector3 a2 = new SVector3(x, 0, z);
            int nValue = (int)(1000L * SVector3.Dot(a2, b) / b.sqrMagnitudeXz);
            return (a2 - CMath.Clamp(nValue, 0, 1000) * b / 1000).sqrMagnitude <=
                   (long)nCircleRadius * (long)nCircleRadius;
        }

        public static int CheckAabbAndLine(
            SVector3 sCenter,
            int nHalfWidth,
            int nHalfHeight,
            SVector3 sOrigin,
            SVector3 sDirection,
            int nMaxDistance)
        {
            int num1 = -2100000;
            int num2 = 2100000;
            sDirection.NormalizeXz();
            if (sDirection.x == 0)
            {
                if (sOrigin.x < sCenter.x - nHalfWidth || sOrigin.x > sCenter.x + nHalfWidth)
                    return -1;
            }
            else
            {
                int num3 = sDirection.x <= 0 ? -nHalfWidth : nHalfWidth;
                num1 = 1000 * (sCenter.x - num3 - sOrigin.x) / sDirection.x;
                num2 = 1000 * (sCenter.x + num3 - sOrigin.x) / sDirection.x;
                if (num1 > num2)
                    return -1;
            }

            if (sDirection.z == 0)
            {
                if (sOrigin.z < sCenter.z - nHalfWidth || sOrigin.z > sCenter.z + nHalfWidth)
                    return -1;
                if (sDirection.x == 0)
                    return 0;
            }
            else
            {
                int num3 = sDirection.z <= 0 ? -nHalfHeight : nHalfHeight;
                int num4 = 1000 * (sCenter.z - num3 - sOrigin.z) / sDirection.z;
                if (num4 > num1)
                    num1 = num4;
                int num5 = 1000 * (sCenter.z + num3 - sOrigin.z) / sDirection.z;
                if (num5 < num2)
                    num2 = num5;
                if (num1 > num2)
                    return -1;
            }

            if (num2 < 0)
                return -1;
            if (num1 < 0)
                num1 = num2;
            return num1 > nMaxDistance ? -1 : num1;
        }

        public static int DistanceFromPointToLine(SVector3 sPos, SVector3 sPos1, SVector3 sPos2)
        {
            int num1 = sPos2.z - sPos1.z;
            int num2 = sPos1.x - sPos2.x;
            int num3 = sPos2.x * sPos1.z - sPos1.x * sPos2.z;
            return num1 == 0 && num2 == 0
                ? 0
                : CMath.Abs(num1 * sPos.x + num2 * sPos.z + num3) / CMath.Sqrt(num1 * num1 + num2 * num2);
        }

        public static void Init()
        {
            CPhysics.m_cCollisionTree = new CQuadTree();
            CPhysics.m_cCollisionTree.Init(200000, 200000);
        }

        public static void Clear()
        {
            if (CPhysics.m_cCollisionTree == null)
                return;
            CPhysics.m_cCollisionTree = (CQuadTree)null;
        }
    }
}