namespace Scarf.Moba
{
    public class BattleRandom
    {
        private System.Random random;

        private int randomCount;

        public void Init(int seed)
        {
            this.random = new System.Random(seed);
            this.randomCount = 0;
        }

        public void Clear()
        {
            this.randomCount = 0;
            this.random = null;
        }

        public int Random(int max)
        {
            ++this.randomCount;
            return this.random.Next(max);
        }

        public int Random(int min, int max)
        {
            ++this.randomCount;
            return this.random.Next(min, max);
        }

        public SVector3 RandomCircleXZ(SVector3 center, int radius)
        {
            int num = this.Random(0, 360);
            radius = this.Random(0, radius);
            return new SVector3(center.x + CMath.Cos(num) * radius / 1000, 0, center.z + CMath.Sin(num) * radius / 1000);
        }

        public SVector3 RandomSectorXZ(SVector3 center, SVector3 forward, int radius, int angle)
        {
            int a = this.Random(0, angle);
            a -= angle / 2;
            int b = this.Random(0, radius);
            return center + (SQuaternion.AngleAxis(a, SVector3.up) * forward).normalizedXz * b / 1000;
        }
    }
}