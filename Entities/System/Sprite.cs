using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Architecture.Entities.System
{
    public class Sprite
    {
        public Texture2D Texture { get; }

        private int _width;
        private int _height;

        public int Width
        {
            get => _width;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Width), "Width must be greater than or equal to 0.");
                _width = value;
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Height), "Height must be greater than or equal to 0.");
                _height = value;
            }
        }

        public Sprite(Texture2D texture, int width, int height)
        {
            Texture = texture;
            Width = width;
            Height = height;
        }

        public Sprite(GraphicsDevice graphicsDevice, Color color, int width, int height)
        {
            Texture = new Texture2D(graphicsDevice, width, height);
            var data = new Color[width * height];
            for (var i = 0; i < data.Length; ++i)
                data[i] = color;
            Texture.SetData(data);
            Width = width;
            Height = height;
        }

        public static Texture2D GeSolidColorTexture(GraphicsDevice graphicsDevice, Color color, int width, int height)
        {
            var texture = new Texture2D(graphicsDevice, width, height);
            var data = new Color[width * height];
            for (var i = 0; i < data.Length; ++i)
                data[i] = color;
            texture.SetData(data);
            return texture;
        }

        public static Texture2D CombineTextures(GraphicsDevice graphicsDevice, Texture2D texture1, Texture2D texture2)
        {
            var width = Math.Max(texture1.Width, texture2.Width);
            var height = Math.Max(texture1.Height, texture2.Height);
            
            var result = new RenderTarget2D(graphicsDevice, width, height);

            graphicsDevice.SetRenderTarget(result);
            graphicsDevice.Clear(Color.Transparent);

            var spriteBatch = new SpriteBatch(graphicsDevice);
            spriteBatch.Begin();

            spriteBatch.Draw(texture1, new Rectangle(0, 0, width, height), Color.White);

            spriteBatch.Draw(texture2, new Rectangle(0, 0, width, height), Color.White);

            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);
            return result;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 positionInPixels) =>
            spriteBatch.Draw(Texture, new Rectangle((int)positionInPixels.X, (int)positionInPixels.Y, Width, Height), Color.White);

    }
}
