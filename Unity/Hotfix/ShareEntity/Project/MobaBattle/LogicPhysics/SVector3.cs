using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace ET
{
    [Serializable]
    public struct SVector3
    {
        public static SVector3 zero = new SVector3(0, 0, 0);
        public static SVector3 one = new SVector3(1000, 1000, 1000);
        public static SVector3 back = new SVector3(0, 0, -1000);
        public static SVector3 down = new SVector3(0, -1000, 0);
        public static SVector3 forward = new SVector3(0, 0, 1000);
        public static SVector3 left = new SVector3(-1000, 0, 0);
        public static SVector3 right = new SVector3(1000, 0, 0);
        public static SVector3 up = new SVector3(0, 1000, 0);
        public static int epsilon = 0;
        private static int k1OverSqrt2 = 707;
        private static int[] matrix3x3 = new int[9];
        public int x;
        public int y;
        public int z;

        public SVector3(SerializationInfo info, StreamingContext context)
        {
            this.x = info.GetInt32(nameof(x));
            this.y = info.GetInt32(nameof(y));
            this.z = info.GetInt32(nameof(z));
        }

        public SVector3(int x = 0, int y = 0, int z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public SVector3(Vector3 sUnityVec, int nScale = 1000)
        {
            this.x = (int)((double)sUnityVec.x * (double)nScale);
            this.y = (int)((double)sUnityVec.y * (double)nScale);
            this.z = (int)((double)sUnityVec.z * (double)nScale);
        }

        // public SVector3(CSPoint csPoint)
        // {
        //   this.x = csPoint.x;
        //   this.y = 0;
        //   this.z = csPoint.z;
        // }

//  public void GetObjectData(SerializationInfo info, StreamingContext context)
//  {
//    info.AddValue("x", this.x);
//    info.AddValue("y", this.y);
//    info.AddValue("z", this.z);
//  }

        public int magnitude
        {
            get { return CMath.Sqrt(this.sqrMagnitude); }
        }

        public int magnitudeXz
        {
            get { return CMath.Sqrt(this.sqrMagnitudeXz); }
        }

        public long sqrMagnitude
        {
            get { return (long)this.x * (long)this.x + (long)this.y * (long)this.y + (long)this.z * (long)this.z; }
        }

        public long sqrMagnitudeXz
        {
            get { return (long)this.x * (long)this.x + (long)this.z * (long)this.z; }
        }

        public SVector3 normalized
        {
            get
            {
                int magnitude = this.magnitude;
                return new SVector3((int)(1000L * (long)this.x / (long)magnitude),
                    (int)(1000L * (long)this.y / (long)magnitude), (int)(1000L * (long)this.z / (long)magnitude));
            }
        }

        public SVector3 normalizedXz
        {
            get
            {
                int num = this.magnitudeXz;
                if (num == 0)
                    num = 1000;
                return new SVector3((int)(1000L * (long)this.x / (long)num), 0,
                    (int)(1000L * (long)this.z / (long)num));
            }
        }

        public void Normalize()
        {
            int magnitude = this.magnitude;
            if (magnitude == 0)
                return;
            this.x = (int)(1000L * (long)this.x / (long)magnitude);
            this.y = (int)(1000L * (long)this.y / (long)magnitude);
            this.z = (int)(1000L * (long)this.z / (long)magnitude);
        }

        public void NormalizeXz()
        {
            this.y = 0;
            int magnitude = this.magnitude;
            if (magnitude == 0)
                return;
            this.x = (int)(1000L * (long)this.x / (long)magnitude);
            this.z = (int)(1000L * (long)this.z / (long)magnitude);
        }

        public static SVector3 Lerp(SVector3 sFrom, SVector3 sDest, int t)
        {
            if (t <= 0)
                return sFrom;
            return t >= 1000 ? sDest : (t * sDest + (1000 - t) * sFrom) / 1000;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sRefPos"></param>
        /// <param name="sOrigin"></param>
        /// <param name="angle"> 逆时针角度 </param>
        /// <returns></returns>
        public static SVector3 RotateAroundPoint(SVector3 sRefPos, SVector3 sOrigin, int angle)
        {
            int num1 = sOrigin.x - sRefPos.x;
            int num2 = sOrigin.z - sRefPos.z;
            int num3 = CMath.Cos(angle);
            int num4 = CMath.Sin(angle);
            int num5 = (num1 * num3 - num2 * num4) / 1000 + sRefPos.x;
            int num6 = (num1 * num4 + num2 * num3) / 1000 + sRefPos.z;
            return new SVector3(num5, 0, num6);
        }

        public static SVector3 MoveTowards(SVector3 sFrom, SVector3 sDest, int clampedDistance)
        {
            SVector3 svector3 = sDest - sFrom;
            long sqrMagnitude = svector3.sqrMagnitude;
            long num1 = (long)(clampedDistance * clampedDistance);
            if (sqrMagnitude <= num1)
                return sDest;
            int num2 = CMath.Sqrt(sqrMagnitude);
            return num2 > SVector3.epsilon ? sFrom + svector3 / num2 * clampedDistance : sFrom;
        }

        private static int ClampedMove(int lhs, int rhs, int clampedDelta)
        {
            int a = rhs - lhs;
            return a > 0 ? lhs + CMath.Min(a, clampedDelta) : lhs - CMath.Min(-a, clampedDelta);
        }

        private static SVector3 OrthoNormalVectorFast(SVector3 n)
        {
            SVector3 svector3;
            if (CMath.Abs(n.z) > SVector3.k1OverSqrt2)
            {
                int num = CMath.Sqrt((long)n.y * (long)n.y + (long)n.z * (long)n.z);
                svector3.x = 0;
                svector3.y = -n.z * 1000 / num;
                svector3.z = n.y * 1000 / num;
            }
            else
            {
                int num = CMath.Sqrt((long)n.x * (long)n.x + (long)n.y * (long)n.y);
                svector3.x = -n.y * 1000 / num;
                svector3.y = n.x * 1000 / num;
                svector3.z = 0;
            }

            return svector3;
        }

        private static int GetMatrix3x3(int row, int column)
        {
            return SVector3.matrix3x3[row + column * 3];
        }

        private static void SetMatrix3x3(int row, int column, int value)
        {
            SVector3.matrix3x3[row + column * 3] = value;
        }

        private static void SetMatrix3x3(SVector3 axis, int angle)
        {
            int num1 = CMath.Sin(angle);
            int num2 = CMath.Cos(angle);
            int x = axis.x;
            int y = axis.y;
            int z = axis.z;
            int num3 = x * x / 1000;
            int num4 = y * y / 1000;
            int num5 = z * z / 1000;
            int num6 = x * y / 1000;
            int num7 = y * z / 1000;
            int num8 = z * x / 1000;
            int num9 = x * num1 / 1000;
            int num10 = y * num1 / 1000;
            int num11 = z * num1 / 1000;
            int num12 = 1000 - num2;
            SVector3.SetMatrix3x3(0, 0, num12 * num3 / 1000 + num2);
            SVector3.SetMatrix3x3(1, 0, num12 * num6 / 1000 - num11);
            SVector3.SetMatrix3x3(2, 0, num12 * num8 / 1000 + num10);
            SVector3.SetMatrix3x3(0, 1, num12 * num6 / 1000 + num11);
            SVector3.SetMatrix3x3(1, 1, num12 * num4 / 1000 + num2);
            SVector3.SetMatrix3x3(2, 1, num12 * num7 / 1000 - num9);
            SVector3.SetMatrix3x3(0, 2, num12 * num8 / 1000 - num10);
            SVector3.SetMatrix3x3(1, 2, num12 * num7 / 1000 + num9);
            SVector3.SetMatrix3x3(2, 2, num12 * num5 / 1000 + num2);
        }

        private static SVector3 Matrix3x3MultiplySVector3(SVector3 v)
        {
            SVector3 svector3;
            svector3.x = SVector3.matrix3x3[0] * v.x / 1000 + SVector3.matrix3x3[3] * v.y / 1000 +
                         SVector3.matrix3x3[6] * v.z / 1000;
            svector3.y = SVector3.matrix3x3[1] * v.x / 1000 + SVector3.matrix3x3[4] * v.y / 1000 +
                         SVector3.matrix3x3[7] * v.z / 1000;
            svector3.z = SVector3.matrix3x3[2] * v.x / 1000 + SVector3.matrix3x3[5] * v.y / 1000 +
                         SVector3.matrix3x3[8] * v.z / 1000;
            return svector3;
        }

        public static SVector3 RotateTowards(
            SVector3 sFrom,
            SVector3 sDest,
            int angleMove,
            int magnitudeMove)
        {
            int magnitude1 = sFrom.magnitude;
            int magnitude2 = sDest.magnitude;
            if (magnitude1 <= SVector3.epsilon || magnitude2 <= SVector3.epsilon)
                return SVector3.MoveTowards(sFrom, sDest, magnitudeMove);
            SVector3 svector3_1 = sFrom * 1000 / magnitude1;
            SVector3 b = sDest * 1000 / magnitude2;
            long num1 = SVector3.Dot(svector3_1, b);
            if (num1 >= (long)(1000000 - SVector3.epsilon))
                return SVector3.MoveTowards(sFrom, sDest, magnitudeMove);
            if (num1 <= (long)(SVector3.epsilon - 1000000))
            {
                SVector3.SetMatrix3x3(SVector3.OrthoNormalVectorFast(svector3_1), angleMove);
                SVector3 svector3_2 = SVector3.Matrix3x3MultiplySVector3(svector3_1);
                int num2 = SVector3.ClampedMove(magnitude1, magnitude2, magnitudeMove);
                svector3_2.x = svector3_2.x * num2 / 1000;
                svector3_2.y = svector3_2.y * num2 / 1000;
                svector3_2.z = svector3_2.z * num2 / 1000;
                return svector3_2;
            }

            int a = CMath.ACos((int)num1);
            SVector3 axis = SVector3.Cross(svector3_1, b);
            axis.Normalize();
            SVector3.SetMatrix3x3(axis, CMath.Min(a, angleMove));
            SVector3 svector3_3 = SVector3.Matrix3x3MultiplySVector3(svector3_1);
            int num3 = SVector3.ClampedMove(magnitude1, magnitude2, magnitudeMove);
            svector3_3.x = svector3_3.x * num3 / 1000;
            svector3_3.y = svector3_3.y * num3 / 1000;
            svector3_3.z = svector3_3.z * num3 / 1000;
            return svector3_3;
        }

        public static bool ApproximatelyXZ(SVector3 a, SVector3 b)
        {
            a.y = 0;
            b.y = 0;
            return SVector3.Angle(a, b) <= 5;
        }

        public Vector3 ToUnity(float fScale = 0.001f)
        {
            return new Vector3((float)this.x * fScale, (float)this.y * fScale, (float)this.z * fScale);
        }

        // public CSPoint ToProto()
        // {
        //   return new CSPoint() { x = this.x, z = this.z };
        // }

        public static int Angle(SVector3 sFrom, SVector3 sTo)
        {
            return sFrom == SVector3.zero || sTo == SVector3.zero
                ? 0
                : CMath.ACos((int)(1000L * SVector3.Dot(sFrom, sTo) /
                                   ((long)CMath.Sqrt(sFrom.sqrMagnitude) * (long)CMath.Sqrt(sTo.sqrMagnitude))));
        }

        public static int SignedAngle(SVector3 sFrom, SVector3 sTo, SVector3 sAxis)
        {
            int num = SVector3.Angle(sFrom, sTo);
            SVector3 b = SVector3.Cross(sFrom, sTo);
            if (SVector3.Dot(sAxis, b) < 0L)
                num = -num;
            return num;
        }

        public static long Dot(SVector3 a, SVector3 b)
        {
            return (long)a.x * (long)b.x + (long)a.y * (long)b.y + (long)a.z * (long)b.z;
        }

        public static SVector3 Cross(SVector3 a, SVector3 b)
        {
            long num1 = (long)a.y * (long)b.z - (long)a.z * (long)b.y;
            long num2 = (long)a.z * (long)b.x - (long)a.x * (long)b.z;
            long num3 = (long)a.x * (long)b.y - (long)a.y * (long)b.x;
            if (!CMath.IsOverstepInt(num1) && !CMath.IsOverstepInt(num2) && !CMath.IsOverstepInt(num3))
                return new SVector3((int)num1, (int)num2, (int)num3);
            Debug.LogError((object)"数值越界，超Int");
            return SVector3.one;
        }

        public static SVector3 GetNormalizeDir(SVector3 a, SVector3 b)
        {
            return (b - a).normalized;
        }

        public static SVector3 GetNormalizeDirXz(SVector3 a, SVector3 b)
        {
            b -= a;
            b.y = 0;
            return b.normalized;
        }

        public static SVector3 operator +(SVector3 a, SVector3 b)
        {
            a.x += b.x;
            a.y += b.y;
            a.z += b.z;
            return a;
        }

        public static SVector3 operator -(SVector3 a)
        {
            a.x = -a.x;
            a.y = -a.y;
            a.z = -a.z;
            return a;
        }

        public static SVector3 operator -(SVector3 a, SVector3 b)
        {
            a.x -= b.x;
            a.y -= b.y;
            a.z -= b.z;
            return a;
        }

        public static SVector3 operator *(SVector3 a, int b)
        {
            a.x *= b;
            a.y *= b;
            a.z *= b;
            return a;
        }

        public static SVector3 operator *(int a, SVector3 b)
        {
            b.x *= a;
            b.y *= a;
            b.z *= a;
            return b;
        }

        public static SVector3 operator /(SVector3 a, int b)
        {
            if (b != 0)
            {
                a.x /= b;
                a.y /= b;
                a.z /= b;
            }

            return a;
        }

        public static bool EqualsXz(SVector3 lhs, SVector3 rhs)
        {
            return lhs.x == rhs.x && lhs.z == rhs.z;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
                return false;
            SVector3 svector3 = (SVector3)obj;
            return this.x == svector3.x && this.y == svector3.y && this.z == svector3.z;
        }

        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^ this.y.GetHashCode() ^ this.z.GetHashCode();
        }

        public override string ToString()
        {
            return this.x.ToString() + ", " + this.y.ToString() + ", " + this.z.ToString();
        }

        public static bool operator ==(SVector3 lhs, SVector3 rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
        }

        public static bool operator !=(SVector3 lhs, SVector3 rhs)
        {
            return !(lhs == rhs);
        }
    }
}