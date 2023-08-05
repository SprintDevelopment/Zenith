using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Zenith.Assets.Attributes;
using ReactiveUI;
using Zenith.Assets.Values.Constants;
using ReactiveUI.Validation.Extensions;
using Zenith.Assets.Extensions;
using ReactiveUI.Fody.Helpers;

namespace Zenith.Models
{
    [Model(SingleName = "یادداشت", MultipleName = "یادداشت ها")]
    public class Note : Model
    {
        [Key]
        [Reactive]
        public int NoteId { get; set; }

        [Required(ErrorMessage = "موضوع یادداشت نمی تواند خالی باشد")]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string Subject { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        ////[Required]
        ////public NotifyTypes NotifyType
        ////{
        ////    get { return notifytType; }
        ////    set { this.RaiseAndSetIfChanged(ref notifytType, value); }
        ////}

        //public DateTime NotifyDateTime
        //{
        //    get { return notifyDateTime; }
        //    set { this.RaiseAndSetIfChanged(ref notifyDateTime, value); }
        //}

        public Note()
        {
            this.ValidationRule(vm => vm.Subject, subject => !subject.IsNullOrWhiteSpace(), "موضوع نمی تواند خالی باشد");
            ////this.ValidationRule(vm => vm.NotifyType, notifyType => notifytType > 0, "روش(های) اطلاعرسانی را انتخاب کنید");
        }

        public override string ToString()
        {
            return Subject;
        }
    }
}
