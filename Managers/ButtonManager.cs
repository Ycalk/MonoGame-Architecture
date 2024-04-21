using Architecture.Entities;
using Architecture.Managers.System;
using Microsoft.Xna.Framework;

namespace Architecture.Managers
{
    public class ButtonManager : Manager
    {
        private bool _press;
        public ButtonManager(IEnumerable<Button> buttons)
        {
            foreach (var el in buttons)
                Add(el);
        }

        public void OnButtonPress() => _press = true;
        public void OnButtonRelease() => _press = false;

        public override void Manage(GameTime gameTime, Screen screen)
        {
            base.Manage(gameTime, screen);
            if (!_press)
            {
                foreach (var button in Entities.Cast<Button>())
                    button.IsPressed = false;
            }
            else
            {
                foreach (var button in Entities.Cast<Button>().Where(button => button.CheckIntersection(screen)))
                    button.IsPressed = true;
            }
        }

    }
}
