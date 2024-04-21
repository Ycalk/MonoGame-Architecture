using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Architecture.Entities.System
{
    public interface IGameEntity
    {
        public int DrawOrder { get; }
        public void Update(GameTime gameTime, Screen screen);
        public void Draw(SpriteBatch spriteBatch, Screen screen);
    }
}
