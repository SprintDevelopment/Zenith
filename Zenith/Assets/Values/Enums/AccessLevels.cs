using System;
using System.ComponentModel.DataAnnotations;

namespace Zenith.Assets.Values.Enums
{
    [Flags]
    public enum AccessLevels
    {
        [Display(Name = "عدم دسترسی")]
        NoAccess = 0,
        [Display(Name = "دسترسی مشاهده")]
        CanRead = 1,
        [Display(Name = "دسترسی ایجاد")]
        CanCreate = 2,
        [Display(Name = "دسترسی ویرایش")]
        CanUpdate = 4,
        [Display(Name = "دسترسی حذف")]
        CanDelete = 8
    }
}
