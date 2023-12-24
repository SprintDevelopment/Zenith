using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Enums;

namespace Zenith.Assets.Extensions
{
    public static class CountUnitsExtensions
    {
        public static int ToInt(this CountUnits countUnit) =>
            countUnit switch
            {
                CountUnits.Meter => 1,
                CountUnits._3MeterTrip => 3,
                CountUnits._20MeterTrip => 20,
                CountUnits._35MeterTrip => 35,
                CountUnits._45MeterTrip => 45,
                _ => 1,
            };
    }
}
