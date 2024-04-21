using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Architecture
{
    public readonly struct Screen 
    {
        public int Width { get; }
        public int Height { get; }

        public Vector2 MousePosition { get; }

        public Screen(int width, int height, Vector2 mousePosition)
        {
            Width = width;
            Height = height;
            MousePosition = mousePosition;
        }

        public Screen(int width, int height, int mouseX, int mouseY) : 
            this(width, height, new Vector2(mouseX, mouseY))
        { }

        public override bool Equals(object? obj)
        {
            if (obj is not Screen screen)
                return false;
            return screen.Width == Width && screen.Height == Height;
        }

        public override int GetHashCode() =>
            HashCode.Combine(Width, Height);

        public static bool operator ==(Screen a, Screen b) => a.Equals(b);
        public static bool operator !=(Screen a, Screen b) => !(a == b);
    }
}
