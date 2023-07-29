using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Attributes;
using ReactiveUI;
using Zenith.Assets.Values.Constants;
using ReactiveUI.Validation.Extensions;
using Zenith.Assets.Extensions;

namespace Zenith.Models
{
    [DataContract]
    [Table("Notes")]
    [Model(SingleName = "یادداشت", MultipleName = "یادداشت ها")]
    public class Note : Model
    {
        private int noteId;
        private string subject;
        private string comment = "";
        ////private NotifyTypes notifytType;
        private DateTime notifyDateTime = DateTime.Today;

        [DataMember]
        [Key]
        public int NoteId
        {
            get { return noteId; }
            set { this.RaiseAndSetIfChanged(ref noteId, value); }
        }

        [DataMember]
        [Required(ErrorMessage = "موضوع یادداشت نمی تواند خالی باشد")]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        public string Subject
        {
            get { return subject; }
            set { this.RaiseAndSetIfChanged(ref subject, value); }
        }

        [DataMember]
        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        public string Comment
        {
            get { return comment; }
            set { this.RaiseAndSetIfChanged(ref comment, value); }
        }

        ////[DataMember]
        ////[Required]
        ////public NotifyTypes NotifyType
        ////{
        ////    get { return notifytType; }
        ////    set { this.RaiseAndSetIfChanged(ref notifytType, value); }
        ////}

        [DataMember]
        public DateTime NotifyDateTime
        {
            get { return notifyDateTime; }
            set { this.RaiseAndSetIfChanged(ref notifyDateTime, value); }
        }

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
