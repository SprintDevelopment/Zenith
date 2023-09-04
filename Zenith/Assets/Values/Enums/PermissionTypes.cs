using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Assets.Values.Enums
{
    public enum PermissionTypes
    {
        DontCare = -1,
        Buys,
        Sales,
        Companies,
        Sites,
        Materials,
        Mixtures,
        Machines,
        Outgoes,
        OutgoCategories,
        Personnel,
        Users,
        Notes,
    }
}
