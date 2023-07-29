using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Assets.Attributes
{
    public class ModelAttribute : Attribute
    {
        public string SingleName { get; set; }
        public string MultipleName { get; set; }
    }
}
