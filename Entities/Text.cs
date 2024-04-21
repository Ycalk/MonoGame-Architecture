using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture.Entities.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Architecture.Entities
{
    public class Text : Entity
    {
        public string Value
        {
            get => _text;
            set => _textOnUpdate = value;
        }

        private string _text;
        private string _textOnUpdate;
        private readonly SpriteFont _font;
        protected Color TextColor;

        public Text(Position position, int drawOrder, SpriteFont font, Color textColor, string value) 
            : base(position, 0, 0, drawOrder)
        {
            _font = font;
            _textOnUpdate = value;
            TextColor = textColor;
            UpdateText();
        }

        private void UpdateText()
        {
            _text = _textOnUpdate;
            var (width, height) = _font.MeasureString(Value);
            Width = (int) width;
            Height = (int) height;
        }

        public override void Draw(SpriteBatch spriteBatch, Screen screen)
        {
            if (_textOnUpdate != _text)
                UpdateText();
            if (IsVisible)
                spriteBatch.DrawString(_font, _textOnUpdate, 
                    Position.GetCoordinate(screen, Width, Height), 
                    TextColor);
        }
    }
}
