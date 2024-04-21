using Microsoft.Xna.Framework;

namespace Architecture.Entities.System
{
    public enum PositionType
    {
        Pixels,
        Percents
    }

    public class Position
    {
        private readonly Vector2 _coordinates;
        public PositionType Type { get; }

        public Vector2 LastCoordinates { get; private set; }

        public Position(Vector2 p, PositionType type) : this(p.X, p.Y, type) { }

        public Position(float x, float y, PositionType type)
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
            _coordinates = new Vector2(x, y);
        }

        public Vector2 GetCoordinate(Screen screen, int width, int height) =>
            GetCoordinate(screen.Width, screen.Height, width, height);

        public Vector2 GetCoordinate(int screenWidth, int screenHeight,
            int objectWidth, int objectHeight)
        {
            if (Type == PositionType.Pixels)
                LastCoordinates = _coordinates;
            else
                LastCoordinates = new Vector2((screenWidth - objectWidth) * _coordinates.X / 100,
                    (screenHeight - objectHeight) * _coordinates.Y / 100);
            return LastCoordinates;
        }

    }
}
