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
using System.Collections.ObjectModel;

namespace Zenith.Models
{
    public class User : Model
    {
        [Key]
        [Required]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string Username { get; set; }

        [NotMapped]
        [Reactive]
        public string Password { get; set; }

        [Required]
        [MaxLength(LengthConstants.LARGE_STRING)]
        public string HashedPassword { get; private set; } = "TEST";

        [Reactive]
        public DateTime CreateDateTime { get; set; } = DateTime.Now;

        [Reactive]
        public virtual ObservableCollection<UserPermission> Permissions { get; set; } = new ObservableCollection<UserPermission>();

        public User()
        {
            this.ValidationRule(vm => vm.Username, oc => oc is not null, "Enter username");
            this.ValidationRule(vm => vm.Password, pass => !string.IsNullOrWhiteSpace(pass), "Enter password");

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
