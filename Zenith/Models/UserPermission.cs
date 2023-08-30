using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Constants;
using Zenith.Assets.Values.Enums;
using Zenith.Assets.Attributes;
using ReactiveUI;
using System.Reactive.Linq;

namespace Zenith.Models
{
    [Model(SingleName = "User permission", MultipleName = "User permissions")]
    public class UserPermission : Model
    {
        [Key]
        [Reactive]
        public int UserPermissionId { get; set; }

        [Required]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string Username { get; set; }

        [ForeignKey(nameof(Username))]
        [Reactive]
        public virtual User User { get; set; }

        [Reactive]
        public PermissionTypes PermissionType { get; set; }

        [Reactive]
        public AccessLevels AccessLevel { get; set; }

        [NotMapped]
        [Reactive]
        public bool HasReadAccess { get; set; }

        [NotMapped]
        [Reactive]
        public bool HasCreateAccess { get; set; }

        [NotMapped]
        [Reactive]
        public bool HasUpdateAccess { get; set; }

        [NotMapped]
        [Reactive]
        public bool HasDeleteAccess { get; set; }

        public UserPermission()
        {
            var locked = false;

            this.WhenAnyValue(m => m.AccessLevel)
                .Do(al =>
                {
                    if (!locked)
                    {
                        locked = true;
                        HasReadAccess = AccessLevel.HasFlag(AccessLevels.CanRead);
                        HasCreateAccess = AccessLevel.HasFlag(AccessLevels.CanCreate);
                        HasUpdateAccess = AccessLevel.HasFlag(AccessLevels.CanUpdate);
                        HasDeleteAccess = AccessLevel.HasFlag(AccessLevels.CanDelete);
                        locked = false;
                    }
                }).Subscribe();

            this.WhenAnyValue(m => m.HasReadAccess, m => m.HasCreateAccess, m => m.HasUpdateAccess, m => m.HasDeleteAccess)
                .Select(x => new[]
                {
                    x.Item1 ? 1 : 0,
                    x.Item2 ? 2 : 0,
                    x.Item3 ? 4 : 0,
                    x.Item4 ? 8 : 0
                }).Do(values =>
                {
                    if (!locked)
                    {
                        locked = true;
                        AccessLevel = (AccessLevels)values.Sum();
                        locked = false;
                    }
                })
                .Subscribe();
        }
    }
}
