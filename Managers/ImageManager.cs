using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture.Entities;
using Architecture.Managers.System;

namespace Architecture.Managers
{
    public class ImageManager : Manager
    {
        public ImageManager(IEnumerable<Image> images)
        {
            foreach (var el in images)
                Add(el);
        }
    }
}
