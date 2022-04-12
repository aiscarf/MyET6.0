namespace Scarf.Moba
{
    public struct SQuaternion
    {
        public int w;
        public int x;
        public int y;
        public int z;

        public SQuaternion(int x = 0, int y = 0, int z = 0, int w = 0)
        {
            this.w = w;
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static SQuaternion AngleAxis(int nAngle, SVector3 sAxis)
        {
            int num1 = nAngle / 2;
            int num2 = CMath.Sin(num1);
            long num3 = (long)(1000 * CMath.Cos(num1));
            long num4 = (long)(num2 * sAxis.x);
            long num5 = (long)(num2 * sAxis.y);
            long num6 = (long)(num2 * sAxis.z);
            int num7 = CMath.Sqrt(num3 * num3 + num4 * num4 + num5 * num5 + num6 * num6);
            int w = (int)(1000L * num3 / (long)num7);
            return new SQuaternion((int)(1000L * num4 / (long)num7), (int)(1000L * num5 / (long)num7),
                (int)(1000L * num6 / (long)num7), w);
        }

        public static SVector3 operator *(SQuaternion sRota, SVector3 sPoint)
        {
            SVector3 a = new SVector3(sRota.x, sRota.y, sRota.z);
            SVector3 b = SVector3.Cross(a, sPoint) / 1000;
            SVector3 svector3_1 = SVector3.Cross(a, b) / 1000;
            SVector3 svector3_2 = 2 * b * sRota.w / 1000;
            SVector3 svector3_3 = svector3_1 * 2;
            return sPoint + svector3_2 + svector3_3;
        }

        // TODO 四元数 * 四元数

        // TODO 四元数的线性插值

        // TODO 四元数的球性插值

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType())
                return false;
            SQuaternion squaternion = (SQuaternion)obj;
            return this.w == squaternion.w && this.x == squaternion.x && this.y == squaternion.y &&
                   this.z == squaternion.z;
        }

        public override int GetHashCode()
        {
            return this.w.GetHashCode() ^ this.x.GetHashCode() ^ this.y.GetHashCode() ^ this.z.GetHashCode();
        }

        public override string ToString()
        {
            return this.w.ToString() + "," + this.x.ToString() + ", " + this.y.ToString() + ", " + this.z.ToString();
        }

        public static bool operator ==(SQuaternion lhs, SQuaternion rhs)
        {
            return lhs.w == rhs.w && lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
        }

        public static bool operator !=(SQuaternion lhs, SQuaternion rhs)
        {
            return !(lhs == rhs);
        }
    }
}