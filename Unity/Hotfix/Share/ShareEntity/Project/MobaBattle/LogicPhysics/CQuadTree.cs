using System.Collections.Generic;

namespace ET

{
    public class CQuadTree
    {
        private int m_nMaxLayer = 8;
        private int[] m_xDir = new int[4] { -1, 1, -1, 1 };
        private int[] m_zDir = new int[4] { 1, 1, -1, -1 };
        private CQuadNode m_cRootNode;

        public void Init(int nWidth, int nHeight)
        {
            this.m_cRootNode = new CQuadNode();
            this.m_cRootNode.nWidth = 1000 * this.GetPower(nWidth / 1000);
            this.m_cRootNode.nHeight = 1000 * this.GetPower(nHeight / 1000);
            this.m_cRootNode.sCenter = SVector3.zero;
            CCollider[] componentsInChildren = new CCollider[100];
            for (int index = 0; index != componentsInChildren.Length; ++index)
                this.AddCollider(this.m_cRootNode, componentsInChildren[index], 1);
        }

        public bool Raycast(SVector3 sOrigin, SVector3 sDirection, int nMaxDistance, int layerMask = 0)
        {
            sDirection.Normalize();
            return this.Raycast(this.m_cRootNode, sOrigin, sDirection, nMaxDistance, layerMask);
        }

        private bool Raycast(
            CQuadNode cQuadNode,
            SVector3 sOrigin,
            SVector3 sDirection,
            int nMaxDistance,
            int layerMask)
        {
            if (CPhysics.CheckAabbAndLine(cQuadNode.sCenter, cQuadNode.nWidth / 2, cQuadNode.nHeight / 2, sOrigin,
                    sDirection,
                    nMaxDistance) < 0 &&
                !CPhysics.CheckAabbAndPos(cQuadNode.sCenter, cQuadNode.nWidth / 2, cQuadNode.nHeight / 2, sOrigin))
                return false;
            List<CCollider> lstCollider = cQuadNode.lstCollider;
            if (lstCollider.Count > 0)
            {
                for (int index = 0; index != lstCollider.Count; ++index)
                {
                    CCollider ccollider = lstCollider[index];
                    if (ccollider.Raycast(sOrigin, sDirection, nMaxDistance, out SVector3 _, out SVector3 _))
                        return true;
                }
            }

            for (int index = 0; index != cQuadNode.arrChildren.Length; ++index)
            {
                CQuadNode arrChild = cQuadNode.arrChildren[index];
                if (arrChild != null && this.Raycast(arrChild, sOrigin, sDirection, nMaxDistance, layerMask))
                    return true;
            }

            return false;
        }

        public bool Raycast(
            SVector3 sOrigin,
            SVector3 sDirection,
            int nMaxDistance,
            out CRaycastHit hitInfo,
            int layerMask = 0)
        {
            sDirection.Normalize();
            return this.Raycast(this.m_cRootNode, sOrigin, sDirection, nMaxDistance, out hitInfo, layerMask);
        }

        private bool Raycast(
            CQuadNode cQuadNode,
            SVector3 sOrigin,
            SVector3 sDirection,
            int nMaxDistance,
            out CRaycastHit hitInfo,
            int layerMask)
        {
            if (CPhysics.CheckAabbAndLine(cQuadNode.sCenter, cQuadNode.nWidth / 2, cQuadNode.nHeight / 2, sOrigin,
                    sDirection,
                    nMaxDistance) < 0 &&
                !CPhysics.CheckAabbAndPos(cQuadNode.sCenter, cQuadNode.nWidth / 2, cQuadNode.nHeight / 2, sOrigin))
            {
                hitInfo.collider = (CCollider)null;
                hitInfo.distance = 0;
                hitInfo.point = SVector3.zero;
                hitInfo.normal = SVector3.zero;
                return false;
            }

            List<CCollider> lstCollider = cQuadNode.lstCollider;
            if (lstCollider.Count > 0)
            {
                for (int index = 0; index != lstCollider.Count; ++index)
                {
                    CCollider ccollider = lstCollider[index];
                    SVector3 sCrossPos;
                    SVector3 sNormal;
                    if (ccollider.Raycast(sOrigin, sDirection, nMaxDistance, out sCrossPos, out sNormal))
                    {
                        hitInfo.collider = lstCollider[index];
                        hitInfo.point = sCrossPos;
                        hitInfo.normal = sNormal;
                        hitInfo.distance = (sCrossPos - sOrigin).magnitude;
                        return true;
                    }
                }
            }

            for (int index = 0; index != cQuadNode.arrChildren.Length; ++index)
            {
                CQuadNode arrChild = cQuadNode.arrChildren[index];
                if (arrChild != null &&
                    this.Raycast(arrChild, sOrigin, sDirection, nMaxDistance, out hitInfo, layerMask))
                    return true;
            }

            hitInfo.collider = (CCollider)null;
            hitInfo.distance = 0;
            hitInfo.point = SVector3.zero;
            hitInfo.normal = SVector3.zero;
            return false;
        }

        public int RaycastAll(
            SVector3 sOrigin,
            SVector3 sDirection,
            int nMaxDistance,
            CRaycastHit[] results,
            int layerMask = 0)
        {
            sDirection.Normalize();
            return this.RaycastAll(this.m_cRootNode, 0, sOrigin, sDirection, nMaxDistance, results, layerMask);
        }

        private int RaycastAll(
            CQuadNode cQuadNode,
            int nTotalCount,
            SVector3 sOrigin,
            SVector3 sDirection,
            int nMaxDistance,
            CRaycastHit[] results,
            int layerMask)
        {
            if (CPhysics.CheckAabbAndLine(cQuadNode.sCenter, cQuadNode.nWidth / 2, cQuadNode.nHeight / 2, sOrigin,
                    sDirection,
                    nMaxDistance) < 0 &&
                !CPhysics.CheckAabbAndPos(cQuadNode.sCenter, cQuadNode.nWidth / 2, cQuadNode.nHeight / 2, sOrigin))
                return nTotalCount;
            List<CCollider> lstCollider = cQuadNode.lstCollider;
            if (lstCollider.Count > 0)
            {
                for (int index = 0; index != lstCollider.Count; ++index)
                {
                    CCollider ccollider = lstCollider[index];
                    SVector3 sCrossPos;
                    SVector3 sNormal;
                    if (ccollider.Raycast(sOrigin, sDirection, nMaxDistance, out sCrossPos, out sNormal))
                    {
                        results[nTotalCount].collider = ccollider;
                        results[nTotalCount].distance = (sCrossPos - sOrigin).magnitude;
                        results[nTotalCount].point = sCrossPos;
                        results[nTotalCount].normal = sNormal;
                        ++nTotalCount;
                        if (nTotalCount >= results.Length)
                            return results.Length;
                    }
                }
            }

            for (int index = 0; index != cQuadNode.arrChildren.Length; ++index)
            {
                CQuadNode arrChild = cQuadNode.arrChildren[index];
                if (arrChild != null)
                {
                    nTotalCount = this.RaycastAll(arrChild, nTotalCount, sOrigin, sDirection, nMaxDistance, results,
                        layerMask);
                    if (nTotalCount >= results.Length)
                        return results.Length;
                }
            }

            return nTotalCount;
        }

        private void AddCollider(CQuadNode cQuadNode, CCollider cCollider, int nLayer)
        {
            if (cQuadNode.nWidth <= 1 || cQuadNode.nHeight <= 1 || nLayer > this.m_nMaxLayer)
            {
                cQuadNode.lstCollider.Add(cCollider);
            }
            else
            {
                int num1 = cQuadNode.nWidth / 2;
                int num2 = cQuadNode.nHeight / 2;
                int nHalfWidth = num1 / 2;
                int nHalfHeight = num2 / 2;
                for (int index = 0; index != 4; ++index)
                {
                    SVector3 sCenter = cQuadNode.sCenter;
                    sCenter.x += nHalfWidth * this.m_xDir[index];
                    sCenter.z += nHalfHeight * this.m_zDir[index];
                    if (cCollider.CheckAabb(sCenter, nHalfWidth, nHalfHeight) == 2)
                    {
                        CQuadNode cQuadNode1 = cQuadNode.arrChildren[index];
                        if (cQuadNode1 == null)
                        {
                            cQuadNode1 = new CQuadNode();
                            cQuadNode1.sCenter = sCenter;
                            cQuadNode1.nWidth = num1;
                            cQuadNode1.nHeight = num2;
                            cQuadNode.arrChildren[index] = cQuadNode1;
                        }

                        this.AddCollider(cQuadNode1, cCollider, nLayer + 1);
                        return;
                    }
                }

                cQuadNode.lstCollider.Add(cCollider);
            }
        }

        private int GetPower(int nValue)
        {
            int num = 2;
            while (num < nValue)
                num *= 2;
            return num;
        }
    }
}