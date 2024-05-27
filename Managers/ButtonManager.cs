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

        public void Ignore(Button button) =>
            Entities.Cast<Button>().First(b => b == button).Ignoring = true;

        public void DisableIgnore(Button button) =>
            Entities.Cast<Button>().First(b => b == button).Ignoring = false;

        public IEnumerable<Button> Ignoring() =>
            Entities.Cast<Button>().Where(b => b.Ignoring);

        public override void Manage(GameTime gameTime, Screen screen)
        {
            if (!_press)
            {
                foreach (var button in Entities.Cast<Button>())
                    button.IsPressed = false;
            }
            else
            {
                foreach (var button in Entities.Cast<Button>()
                             .Where(button => button.CheckIntersection(screen)))
                    button.IsPressed = true;
            }
            base.Manage(gameTime, screen);
        }

    }
}
