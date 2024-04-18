using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Architecture.Entities
{
    public class Button : Entity
    {
        protected string Text;
        protected Color TextColor;

        protected readonly Color InitialTextColor;
        protected Color HoveringTextColor = Color.Blue;
        protected Color PressingTextColor = Color.Red;

        public bool IsPressed;
        protected bool IsHovered;
        
        private bool _hovering;
        private bool _pressing;
        protected SpriteFont Font;

        public Rectangle ButtonRectangle(Screen screen)
        {
            var coordinate = Position.GetCoordinate(screen, Width, Height);
            return new Rectangle(coordinate.X, coordinate.Y, Width, Height);
        }

        public Button(Position position, int drawOrder, Sprite.Sprite sprite, SpriteFont font, Color textColor, string text = "")
            :this(position, sprite.Width, sprite.Height, drawOrder, sprite, font, textColor, text) { }

        public Button(
            Position position, int width, int height, int drawOrder, Sprite.Sprite sprite, SpriteFont font, Color textColor, 
            string text = "") : base(position, width, height, drawOrder, sprite)
        {
            Font = font;
            InitialTextColor = textColor;
            TextColor = InitialTextColor;
            Text = text;
        }

        public virtual void OnHover()
        {
            TextColor = HoveringTextColor;
        }

        public virtual void OnLeave()
        {
            TextColor = InitialTextColor;
        }

        public virtual void OnPress()
        {
            TextColor = PressingTextColor;
        }

        public virtual void OnRelease()
        {
            TextColor = IsHovered ? HoveringTextColor : InitialTextColor;
        }

        public override void Draw(SpriteBatch spriteBatch, Screen screen)
        {
            if (IsPressed && !_pressing)
            {
                OnPress();
                _pressing = true;
            }
            else if (IsHovered && !_hovering)
            {
                OnHover();
                _hovering = true;
            }
            else
            {
                if (!IsPressed && _pressing)
                {
                    OnRelease();
                    _pressing = false;
                }

                if (!IsHovered && _hovering)
                {
                    OnLeave();
                    _hovering = false;
                }
                
            }
                
            

            var coordinates = Position.GetCoordinate(screen, Width, Height);

            Sprite?.Draw(spriteBatch, coordinates);

            if (string.IsNullOrEmpty(Text)) return;

            var x = (coordinates.X + (float)Width / 2) - (Font.MeasureString(Text).X / 2);
            var y = (coordinates.Y + (float)Height / 2) - (Font.MeasureString(Text).Y / 2);

            spriteBatch.DrawString(Font, Text, new Vector2(x, y), TextColor);
        }

        public bool CheckIntersection(Screen screen) => 
            new Rectangle(screen.MousePosition.X, screen.MousePosition.Y, 1, 1).Intersects(ButtonRectangle(screen));


        public override void Update(GameTime gameTime, Screen screen)
        {
            IsHovered = CheckIntersection(screen);
        }
    }
}
