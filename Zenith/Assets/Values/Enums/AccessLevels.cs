using System;
using System.ComponentModel.DataAnnotations;

namespace Zenith.Assets.Values.Enums
{
    [Flags]
    public enum AccessLevels
    {
        [Display(Name = "No access")]
        NoAccess = 0,
        [Display(Name = "View list")]
        CanRead = 1,
        [Display(Name = "Create")]
        CanCreate = 2,
        [Display(Name = "Edit")]
        CanUpdate = 4,
        [Display(Name = "Delete")]
        CanDelete = 8
    }
}
