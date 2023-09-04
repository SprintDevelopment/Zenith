using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Zenith.Assets.Attributes;
using Zenith.Assets.Values.Constants;
using Zenith.Assets.Values.Enums;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using Zenith.Assets.Extensions;

namespace Zenith.Models
{
    public class Person : Model
    {
        [Key]
        [Reactive]
        public int PersonId { get; set; }

        [Reactive]
        public Jobs Job { get; set; }

        [Required]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.CALL_NUMBERS)]
        [Reactive]
        public string Tel { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.CALL_NUMBERS)]
        [Reactive]
        public string Mobile { get; set; } = string.Empty;

        [NotMapped]
        public string CallNumbers
        {
            get
            {
                var result = "";
                if (Tel != "") result = Tel;
                if (Mobile != "") result += result == "" ? Mobile : ", " + Mobile;

                return result;
            }
        }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string NationalCode { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.LARGE_STRING)]
        [Reactive]
        public string Address { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        [Reactive]
        public bool IsDeleted { get; set; }

        public Person()
        {
            this.ValidationRule(vm => vm.FirstName, fName => !fName.IsNullOrWhiteSpace(), "Enter first name");
            this.ValidationRule(vm => vm.LastName, lName => !lName.IsNullOrWhiteSpace(), "Enter last name");
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
