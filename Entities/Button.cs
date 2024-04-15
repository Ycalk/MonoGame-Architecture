using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Architecture.Entities
{
    public class Button : Entity
    {
        protected string Text;
        protected readonly Color InitialTextColor;
        protected Color TextColor;
        protected bool IsHovering;
        protected Sprite.Sprite ButtonSprite;
        protected SpriteFont Font;

        public Rectangle Rectangle => new(Position.X, Position.Y, Width, Height);

        public Button(
            int drawOrder, Sprite.Sprite buttonSprite, SpriteFont font, Color textColor, 
            Point position, int width, int height, string text = "") : base(position, width, height, drawOrder)
        {
            ButtonSprite = buttonSprite;
            Font = font;
            InitialTextColor = textColor;
            Text = text;
        }

        public virtual void OnHover()
        {
            TextColor = Color.Blue;
        }

        public virtual void OnLeave()
        {
            TextColor = InitialTextColor;
        }

        public virtual void OnClick()
        {
            TextColor = Color.Red;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsHovering)
                OnHover();
            else
                OnLeave();

            ButtonSprite.Draw(spriteBatch, Position);

            if (string.IsNullOrEmpty(Text)) return;

            var x = (Position.X + (float)Width / 2) - (Font.MeasureString(Text).X / 2);
            var y = (Position.Y + (float)Height / 2) - (Font.MeasureString(Text).Y / 2);

            spriteBatch.DrawString(Font, Text, new Vector2(x, y), TextColor);
        }

        public bool CheckIntersection(Screen screen) => 
            new Rectangle(screen.MousePosition.X, screen.MousePosition.Y, 1, 1).Intersects(Rectangle);


        public override void Update(GameTime gameTime, Screen screen)
        {
            IsHovering = CheckIntersection(screen);
        }
    }
}
