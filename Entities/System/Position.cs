namespace Architecture
{
    public enum PositionType
    {
        Pixels,
        Percents
    }

    public class Position
    {
        private readonly Point _coordinates;
        public PositionType Type { get; private set; }

        public Point? LastCoordinates { get; private set; }

        public Position(Point p, PositionType type):this(p.X, p.Y, type) { }

        public Position(int x, int y, PositionType type)
        {
            if (type == PositionType.Percents)
            {
                if (x is < 0 or > 100)
                    throw new ArgumentOutOfRangeException(nameof(x),
                        "Position must be between 0 and 100 for percents.");
                if (y is < 0 or > 100)
                    throw new ArgumentOutOfRangeException(nameof(y),
                        "Position must be between 0 and 100 for percents.");
                Type = PositionType.Percents;
            }
            else
            {
                Type = PositionType.Pixels;
            }
            _coordinates = new Point(x, y);
        }

        public Point GetCoordinate(Screen screen, int width, int height) => 
            GetCoordinate(screen.Width, screen.Height, width, height);

        public Point GetCoordinate(int screenWidth, int screenHeight, 
            int objectWidth, int objectHeight)
        {
            if (Type == PositionType.Pixels)
                LastCoordinates = _coordinates;
            else
                LastCoordinates = new Point((screenWidth - objectWidth) * _coordinates.X / 100,
                    (screenHeight - objectHeight) * _coordinates.Y / 100);
            return LastCoordinates;
        }

    }
}
