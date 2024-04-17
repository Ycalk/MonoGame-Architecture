using Architecture.Entities;
using Architecture.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Point = Architecture.Point;

namespace Architecture
{
    public class Scene
    {
        Texture2D Background { get; }

        protected readonly ButtonManager ButtonManager;

        public Scene(IEnumerable<Button> buttons, Texture2D background)
        {
            ButtonManager = new ButtonManager(buttons);
            Background = background;
        }

        public virtual void Update(GameTime gameTime, Screen screen)
        {
            ButtonManager.Manage(gameTime, screen);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Screen screen)
        {
            spriteBatch.Draw(Background, new Rectangle(0, 0, screen.Width, screen.Height), Color.White);
            ButtonManager.DrawEntities(spriteBatch, screen);
        }
    }
}
