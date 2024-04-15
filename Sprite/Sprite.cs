using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Architecture.Sprite
{
    public class Sprite
    {
        public Texture2D Texture { get; set; }

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

        public void Draw(SpriteBatch spriteBatch, Point position) =>
            spriteBatch.Draw(Texture, new Rectangle(position.X, position.Y, Width, Height), Color.White);

    }
}
