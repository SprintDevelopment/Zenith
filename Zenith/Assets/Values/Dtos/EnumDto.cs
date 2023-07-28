using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Assets.Values.Dtos
{
    public class EnumDto : BaseDto
    {
        public object Value { get; set; }
        public string Description { get; set; }

        public EnumDto(object value, string description)
        {
            Value = value;
            Description = description;
        }
    }
}
