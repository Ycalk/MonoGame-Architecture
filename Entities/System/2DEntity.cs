using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Architecture.Entities.System
{
    public abstract class Entity2D : Entity
    {
        public bool IsVisible { get; protected set; }
        protected Position Position;
        protected Sprite? Sprite;
        protected int Width;
        protected int Height;
        public int DrawOrder { get; }

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

        internal virtual void Update(GameTime gameTime, Screen screen)
        {
        }

    }
}
