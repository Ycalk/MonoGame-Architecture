using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture.Entities;
using Architecture.Managers.System;

namespace Architecture.Managers
{
    public class TextManager : Manager
    {
        public TextManager(IEnumerable<Text> texts)
        {
            foreach (var el in texts)
                Add(el);
        }
    }
}
