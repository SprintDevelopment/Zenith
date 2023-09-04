using System;
using System.ComponentModel.DataAnnotations;

namespace Zenith.Assets.Values.Enums
{
    [Flags]
    public enum AccessLevels
    {
        NoAccess = 0,
        CanRead = 1,
        CanCreate = 2,
        CanUpdate = 4,
        CanDelete = 8
    }
}
