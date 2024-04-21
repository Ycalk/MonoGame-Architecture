using Microsoft.Xna.Framework;

namespace Architecture.Managers.System
{
    internal interface IManager
    {
        void Manage(GameTime gameTime, Screen screen);
    }
}
