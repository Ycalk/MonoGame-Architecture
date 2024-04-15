using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Architecture.Entities
{
    public interface IGameEntity
    {
        public int DrawOrder { get; }
        public void Update(GameTime gameTime, Screen screen);
        public void Draw(SpriteBatch spriteBatch);
        public void OnWindowResize(Screen oldScreen, Screen newScreen);
    }
}
