using Microsoft.Xna.Framework;

namespace Architecture.Managers
{
    public interface IManager
    {
        void Manage(GameTime gameTime, Screen screen);
    }
}
