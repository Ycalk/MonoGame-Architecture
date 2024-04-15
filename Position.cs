namespace Architecture
{
    public enum PositionType
    {
        Pixels,
        Percents
    }

    public struct Position
    {
        private Point _info;
        public PositionType Type { get; private set; }

        public Position(int x, int y, PositionType type)
        {
            if (type == PositionType.Pixels)
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
            _info = new Point(x, y);
        }

        public Point GetCoordinate(Screen screen)
        {
            if (Type == PositionType.Pixels)
                return _info;
            return new Point(
                               (int)(_info.X * screen.Width / 100.0),
                                              (int)(_info.Y * screen.Height / 100.0)
                                          );
        }

    }
}
