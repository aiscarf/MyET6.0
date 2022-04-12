using System.Collections.Generic;

namespace Scarf.Moba
{
    public class CQuadNode
    {
        public CQuadNode[] arrChildren = new CQuadNode[4];
        public List<CCollider> lstCollider = new List<CCollider>();
        public SVector3 sCenter;
        public int nWidth;
        public int nHeight;
        public CQuadNode cParent;
    }
}