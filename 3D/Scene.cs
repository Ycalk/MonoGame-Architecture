using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace Architecture._3D
{
    public class Scene : Architecture.Scene
    {
        public Scene(IEnumerable<Button> buttons, Texture2D background) : base(buttons, background)
        {
        }
    }
}
