using Architecture.Entities;
using Microsoft.Xna.Framework;

namespace Architecture.Managers
{
    public class ButtonManager : Manager
    {
        private bool _clicked;
        public ButtonManager(IEnumerable<Button> buttons)
        {
            foreach (var el in buttons)
                Add(el);
        }

        public void OnClick() => _clicked = true;
        public override void Manage(GameTime gameTime, Screen screen)
        {
            base.Manage(gameTime, screen);
            if (!_clicked) return;
            foreach (var button in Entities.Cast<Button>().Where(button => button.CheckIntersection(screen)))
            {
                button.OnClick();
                _clicked = false;
            }
        }

    }
}
