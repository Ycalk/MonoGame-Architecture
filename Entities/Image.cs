﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture.Entities.System;

namespace Architecture.Entities
{
    public class Image : Entity2D
    {
        public Image(Position position, Sprite sprite, int drawOrder) :
            this(position, sprite.Width, sprite.Height, drawOrder, sprite) {}
        public Image(Position position, int width, int height, int drawOrder, Sprite sprite) 
            : base(position, width, height, drawOrder, sprite) {}

    }
}
