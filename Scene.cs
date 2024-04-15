using Architecture.Entities;
using Architecture.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Point = Architecture.Point;

namespace Architecture
{
    public class Scene
    {
        public Sprite.Sprite Background { get; }

        protected readonly ButtonManager ButtonManager;
        protected Point Position;

        public Scene(IEnumerable<Button> buttons, Sprite.Sprite background, Point position)
        {
            ButtonManager = new ButtonManager(buttons);
            Background = background;
        }

        public virtual void OnWindowResize(Screen oldScreen, Screen newScreen)
        {
            Background.Height = newScreen.Height;
            Background.Width = newScreen.Width;
            ButtonManager.OnWindowResize(oldScreen, newScreen);
        }

        public virtual void Update(GameTime gameTime, Screen screen)
        {
            ButtonManager.Manage(gameTime, screen);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Background.Draw(spriteBatch, Position);
            ButtonManager.DrawEntities(spriteBatch);
        }
    }
}
