using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Enums;

namespace Zenith.Assets.Attributes
{
    public class SearchAttribute : Attribute
    {
        public SearchItemControlTypes ControlType { get; set; } = SearchItemControlTypes.TextBox;

        public Type ValueSourceType { get; set; }
    }
}
