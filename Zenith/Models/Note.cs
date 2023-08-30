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
    [Model(SingleName = "Note", MultipleName = "Notes")]
    public class Note : Model
    {
        [Key]
        [Reactive]
        public int NoteId { get; set; }

        [Required]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string Subject { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        [Required]
        [Reactive]
        public NotifyTypes NotifyType { get; set; }

        [Reactive]
        public DateTime NotifyDateTime { get; set; } = DateTime.Now;

        public Note()
        {
            this.ValidationRule(vm => vm.Subject, subject => !subject.IsNullOrWhiteSpace(), "Enter the subject");
        }

        public override string ToString()
        {
            return Subject;
        }
    }
}
