using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Constants;
using Zenith.Assets.Values.Enums;

namespace Zenith.Models
{
    public class Configuration : Model
    {
        [Key]
        [Reactive]
        public string Key { get; set; }

        [Required]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string Value { get; set; }
    }
}
