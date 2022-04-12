namespace Scarf.Moba
{
    public struct SRect
    {
        public int x;
        public int y;
        public int width;
        public int height;

        public SRect(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public int xMin
        {
            get { return this.x; }
            set { this.x = value; }
        }

        public int yMin
        {
            get { return this.y; }
            set { this.y = value; }
        }

        public int xMax
        {
            get { return this.x + this.width; }
            set { this.width = value - this.x; }
        }

        public int yMax
        {
            get { return this.y + this.height; }
            set { this.height = value - this.y; }
        }

        public int xCenter
        {
            get { return this.x + this.width / 2; }
            set { this.x = value - this.width / 2; }
        }

        public int yCenter
        {
            get { return this.y + this.height / 2; }
            set { this.y = value - this.height / 2; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || (object)obj.GetType() != (object)this.GetType())
                return false;
            SRect srect = (SRect)obj;
            if (this.x == srect.x && this.y == srect.y && this.width == srect.width)
                return this.height == srect.height;
            return false;
        }

        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^ this.y.GetHashCode() ^ this.width.GetHashCode() ^ this.height.GetHashCode();
        }

        public override string ToString()
        {
            return this.x.ToString() + ", " + this.y.ToString() + ", " + this.width.ToString() + ", " +
                   this.height.ToString();
        }

        public static bool operator ==(SRect lhs, SRect rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.width == rhs.width && lhs.height == rhs.height;
        }

        public static bool operator !=(SRect lhs, SRect rhs)
        {
            return !(lhs == rhs);
        }
    }
}