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
        protected readonly CubeManager CubeManager;
        protected readonly GraphicsDevice Graphics;

        public Scene(IEnumerable<Button> buttons, 
            IEnumerable<Image> images, 
            IEnumerable<Text> texts, 
            IEnumerable<Cube> cubes,
            GraphicsDevice graphics,
            Texture2D background)
        {
            Graphics = graphics;
            ButtonManager = new ButtonManager(buttons);
            ImageManager = new ImageManager(images);
            TextManager = new TextManager(texts);
            CubeManager = new CubeManager(graphics, cubes);
            Background = background;
        }

        public virtual void Update(GameTime gameTime, Screen screen)
        {
            ButtonManager.Manage(gameTime, screen);
            ImageManager.Manage(gameTime, screen);
            TextManager.Manage(gameTime, screen);
            CubeManager.Manage(gameTime, screen);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Screen screen)
        {
            if (CubeManager.Elements.Count == 0) 
                spriteBatch.Draw(Background, new Rectangle(0, 0, screen.Width, screen.Height), Color.White);
            else
                Graphics.Clear(Color.White);
            ButtonManager.DrawEntities(spriteBatch, screen);
            ImageManager.DrawEntities(spriteBatch, screen);
            TextManager.DrawEntities(spriteBatch, screen);
            CubeManager.DrawCubes();
        }

        public virtual void ButtonPress()
        {
            ButtonManager.OnButtonPress();
            CubeManager.OnButtonPress();
        }

        public virtual void ButtonRelease()
        {
            ButtonManager.OnButtonRelease();
            CubeManager.OnButtonRelease();
        }
        
        public virtual void LeftArrowPress() => CubeManager.OnLeftArrowPress();
        public virtual void RightArrowPress() => CubeManager.OnRightArrowPress();
        
    }
}
