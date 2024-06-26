﻿using Architecture.Entities.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Architecture.Entities
{
    public class Button : Entity2D, IInteractive
    {
        protected string Text;
        protected Color TextColor;
        protected readonly Color InitialTextColor;
        internal bool Ignoring;

        protected Color HoveringTextColor = Color.Blue;
        protected Color PressingTextColor = Color.Red;

        public bool IsPressed { get; internal set; }
        public bool IsHovered { get; protected set; }

        private bool _hovering;
        private bool _pressing;
        protected SpriteFont Font;

        public Rectangle ButtonRectangle(Screen screen)
        {
            var coordinate = Position.GetCoordinate(screen, Width, Height);
            return new Rectangle((int)coordinate.X, (int)coordinate.Y, Width, Height);
        }

        public Button(Position position, int drawOrder, Sprite sprite, SpriteFont font, Color textColor, string text = "")
            :this(position, sprite.Width, sprite.Height, drawOrder, sprite, font, textColor, text) { }

        public Button(
            Position position, int width, int height, int drawOrder, Sprite sprite, SpriteFont font, Color textColor, 
            string text = "") : base(position, width, height, drawOrder, sprite)
        {
            Font = font;
            InitialTextColor = textColor;
            TextColor = InitialTextColor;
            Text = text;
        }

        protected virtual void OnHover()
        {
            TextColor = HoveringTextColor;
        }

        protected virtual void OnLeave()
        {
            TextColor = InitialTextColor;
        }

        protected virtual void OnPress()
        {
            TextColor = PressingTextColor;
        }

        protected virtual void OnRelease()
        {
            TextColor = IsHovered ? HoveringTextColor : InitialTextColor;
        }

        internal override void Draw(SpriteBatch spriteBatch, Screen screen)
        {
            base.Draw(spriteBatch, screen);

            var coordinates = Position.GetCoordinate(screen, Width, Height);

            if (string.IsNullOrEmpty(Text)) return;

            var x = (coordinates.X + (float)Width / 2) - (Font.MeasureString(Text).X / 2);
            var y = (coordinates.Y + (float)Height / 2) - (Font.MeasureString(Text).Y / 2);
            if (IsVisible)
                spriteBatch.DrawString(Font, Text, new Vector2(x, y), TextColor);
        }

        public bool CheckIntersection(Screen screen) => 
            new Rectangle((int)screen.MousePosition.X, (int)screen.MousePosition.Y, 1, 1).Intersects(ButtonRectangle(screen));

        internal override void Update(GameTime gameTime, Screen screen)
        {
            base.Update(gameTime, screen);
            IsHovered = CheckIntersection(screen);
            if (Ignoring) return;
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
                    if (CheckIntersection(screen))
                        OnRelease();
                    _pressing = false;
                }
                if (IsHovered || !_hovering) return;
                OnLeave();
                _hovering = false;
            }
        }
    }
}
