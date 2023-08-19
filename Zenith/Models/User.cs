using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Zenith.Assets.Attributes;
using Zenith.Assets.Values.Constants;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Fody.Helpers;
using Zenith.Assets.Extensions;
using System;
using ReactiveUI;
using System.Runtime.Serialization;
using Zenith.Assets.Utils;
using System.Reactive.Linq;

namespace Zenith.Models
{
    [Model(SingleName = "کاربر", MultipleName = "کاربران")]
    public class User : Model
    {
        [Key]
        [Required(ErrorMessage = "نام کاربری نمی تواند خالی باشد")]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string Username { get; set; }

        [NotMapped]
        [Reactive]
        public string Password { get; set; }

        [Required]
        [MaxLength(LengthConstants.LARGE_STRING)]
        public string HashedPassword { get; private set; }

        [Reactive]
        public DateTime CreateDateTime { get; set; } = DateTime.Now;

        [Column(TypeName = "Image")]
        [Reactive]
        public byte[]? AvatarImageBytes { get; set; }

        public User()
        {
            this.ValidationRule(vm => vm.Username, oc => oc is not null, "نام کاربری نمی تواند خالی باشد");
            this.ValidationRule(vm => vm.Password, pass => !string.IsNullOrWhiteSpace(pass), "میزان هزینه باید بیشتر از 0 باشد");

            this.WhenAnyValue(m => m.Password)
                .Where(p => !p.IsNullOrWhiteSpace())
                .Do(p => HashedPassword = CryptoUtil.GenerateSaltedHashBytes(p))
                .Subscribe();
        }

        public override string ToString()
        {
            return Username;
        }

    }
}
