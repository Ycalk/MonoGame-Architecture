using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture.Entities.System;

namespace Architecture.Entities
{
    public class Image : Entity
    {
        public Image(Position position, int width, int height, int drawOrder, Sprite sprite) 
            : base(position, width, height, drawOrder, sprite) {}

    }
}
