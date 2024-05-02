using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Architecture.Entities.System
{
    public interface IInteractive
    {
        public bool IsPressed { get; }

        public bool IsHovered { get; }
    }
}
