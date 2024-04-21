using Architecture.Entities;
using Architecture.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Architecture
{
    public class Scene
    {
        protected Texture2D Background;

        protected readonly ButtonManager ButtonManager;
        protected readonly ImageManager ImageManager;
        protected readonly TextManager TextManager;

        public Scene(IEnumerable<Button> buttons, IEnumerable<Image> images, IEnumerable<Text> texts, Texture2D background)
        {
            ButtonManager = new ButtonManager(buttons);
            ImageManager = new ImageManager(images);
            TextManager = new TextManager(texts);
            Background = background;
        }

        public virtual void Update(GameTime gameTime, Screen screen)
        {
            ButtonManager.Manage(gameTime, screen);
            ImageManager.Manage(gameTime, screen);
            TextManager.Manage(gameTime, screen);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Screen screen)
        {
            spriteBatch.Draw(Background, new Rectangle(0, 0, screen.Width, screen.Height), Color.White);
            ButtonManager.DrawEntities(spriteBatch, screen);
            ImageManager.DrawEntities(spriteBatch, screen);
            TextManager.DrawEntities(spriteBatch, screen);
        }

        public virtual void ButtonPress() =>
            ButtonManager.OnButtonPress();
        

        public virtual void ButtonRelease() =>
            ButtonManager.OnButtonRelease();
        
    }
}
