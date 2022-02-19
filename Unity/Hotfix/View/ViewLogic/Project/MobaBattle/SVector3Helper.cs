using UnityEngine;

namespace ET
{
    public static class SVector3Helper
    {
        public static Vector3 ToUnity(this SVector3 self, float fScale = 0.001f)
        {
            return new Vector3((float)self.x * fScale, (float)self.y * fScale, (float)self.z * fScale);
        }
    }
}