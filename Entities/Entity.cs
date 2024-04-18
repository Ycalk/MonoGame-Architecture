using Architecture.Entities.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Architecture.Entities
{
    public abstract class Entity : IGameEntity
    {
        public bool IsVisible { get; protected set; }
        protected Position Position;
        protected Sprite? Sprite;
        protected int Width;
        protected int Height;
        public int DrawOrder { get; }

        protected Entity(Position positionInPixels, int width, int height, int drawOrder)
        {
            Position = positionInPixels;
            Width = width;
            Height = height;
            DrawOrder = drawOrder;
            IsVisible = true;
        }

        protected Entity(Position position, int width, int height, int drawOrder, Sprite sprite) :
            this(position, width, height, drawOrder)
        {
            Sprite = sprite;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Screen screen) =>
            Sprite?.Draw(spriteBatch, Position.GetCoordinate(screen, Width, Height));

        public virtual void Update(GameTime gameTime, Screen screen)
        {
        }
    }
}
