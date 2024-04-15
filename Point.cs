namespace Architecture
{
    public readonly struct Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Point point)
                return false;
            return point.X == X && point.Y == Y;
        }

        public override int GetHashCode() =>
            HashCode.Combine(X, Y);

        public Point WithX(int x) => new(x, Y);
        public Point WithY(int y) => new(X, y);
        public static Point Zero => new(0, 0);
        public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);
        public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y);
        public static bool operator ==(Point a, Point b) => a.Equals(b);
        public static bool operator !=(Point a, Point b) => !(a == b);
    }
}
