using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Architecture.Entities.System
{
    public abstract class Entity2D : Entity
    {
        public bool IsVisible { get; protected set; }
        public Position Position { get; protected set; }
        protected Sprite? Sprite;
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int DrawOrder { get; }

        private Vector2 _startPosition;
        private Vector2 _endPosition;
        private float _currentMovingTime;
        private float _totalMovingTime;
        private bool _isMoving;

        protected Entity2D(Position position, int width, int height, int drawOrder)
        {
            Position = position;
            Width = width;
            Height = height;
            DrawOrder = drawOrder;
            IsVisible = true;
        }

        protected Entity2D(Position position, int width, int height, int drawOrder, Sprite sprite) :
            this(position, width, height, drawOrder)
        {
            Sprite = sprite;
        }
        internal virtual void Draw(SpriteBatch spriteBatch, Screen screen)
        {
            if (IsVisible)
                Sprite?.Draw(spriteBatch, Position.GetCoordinate(screen, Width, Height));
        }

        public void MoveTo(Screen screen, Position position, float movingTime, bool considerDistance) =>
            MoveTo(screen, position.GetCoordinate(screen, Width, Height), movingTime, considerDistance);

        public void MoveTo(Screen screen, Vector2 position, float movingTime, bool considerDistance)
        {
            _startPosition = Position.GetCoordinate(screen, Width, Height);
            _endPosition = position;
            _totalMovingTime = movingTime * (considerDistance ? (position - _startPosition).Length() : 1);
            _currentMovingTime = 0;
            _isMoving = true;
        }

        internal virtual void Update(GameTime gameTime, Screen screen)
        {
            if (_isMoving)
            {
                _currentMovingTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_currentMovingTime >= _totalMovingTime)
                {
                    Position = new Position(_endPosition, PositionType.Pixels);
                    _isMoving = false;
                }
                else
                {
                    var newPosition = Vector2.Lerp(_startPosition,
                        _endPosition, _currentMovingTime / _totalMovingTime);
                    Position = new Position(newPosition, PositionType.Pixels);
                }
            }
        }

    }
}
