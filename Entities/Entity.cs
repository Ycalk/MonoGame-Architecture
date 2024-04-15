using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Architecture.Entities
{
    public abstract class Entity : IGameEntity
    {
        public bool IsVisible { get; protected set; }
        protected Point Position;
        protected PositionType PositionType;
        protected Sprite.Sprite? Sprite;
        protected int Width;
        protected int Height;
        public int DrawOrder { get; }

        protected Entity(int drawOrder)
        {
            IsVisible = true;
            DrawOrder = drawOrder;
        }

        protected Entity()
        {
        }

        protected Entity(Point positionInPixels, int width, int height, int drawOrder)
        {
            Position = positionInPixels;
            Width = width;
            Height = height;
            DrawOrder = drawOrder;
            IsVisible = true;
            PositionType = PositionType.Pixels;
        }

        protected Entity(Point position, int width, int height, int drawOrder, Sprite.Sprite sprite, PositionType positionType, Screen screen)
        {
            if (positionType == PositionType.Pixels)
                Position = position;
            else
            {
                if (position.X < 0 || position.X > 100 || position.Y < 0 || position.Y > 100)
                    throw new ArgumentOutOfRangeException(nameof(position), "Position must be between 0 and 100 for percents.");
                Position = new Point(
                    (int)(position.X * screen.Width / 100.0),
                    (int)(position.Y * screen.Height / 100.0));
            }
                
            Width = width;
            Height = height;
            DrawOrder = drawOrder;
            IsVisible = true;
            Sprite = sprite;
        }

        public virtual void Draw(SpriteBatch spriteBatch) =>
            Sprite?.Draw(spriteBatch, Position);

        public virtual void Update(GameTime gameTime, Screen screen)
        {
        }

        public virtual void OnWindowResize(Screen oldScreen, Screen newScreen)
        {
            var delta = new Point(newScreen.Width - oldScreen.Width, newScreen.Height - oldScreen.Height);
            Position += delta;
        }
    }
}
