using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Zenith.Assets.Attributes;
using ReactiveUI;
using Zenith.Assets.Values.Constants;
using ReactiveUI.Validation.Extensions;
using Zenith.Assets.Extensions;
using ReactiveUI.Fody.Helpers;
using Zenith.Assets.Values.Enums;
using System;

namespace Zenith.Models
{
    public class PersonnelAbsence : Model
    {
        [Key]
        [Reactive]
        public int PersonnelAbsenceId { get; set; }

        [Required]
        [Reactive]
        public int PersonId { get; set; }

        [ForeignKey(nameof(PersonId))]
        public virtual Person Person { get; set; }

        [Reactive]
        public DateTime DateTime { get; set; } = DateTime.Now;

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public PersonnelAbsence()
        {
        }

        public override string ToString()
        {
            return Person?.FullName;
        }
    }
}
