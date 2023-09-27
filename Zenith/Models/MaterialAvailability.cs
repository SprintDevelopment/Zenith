using ReactiveUI.Fody.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Zenith.Models
{
    public class MaterialAvailability : Model
    {
        [Key]
        [Reactive]
        public long MaterialAvailabilityId { get; set; }

        [Reactive] 
        public int MaterialId { get; set; }

        [Reactive]
        public float BuyPrice { get; set; }

        [Reactive]
        public float TotalBoughtCount { get; set; }

        [Reactive]
        public float AvailableCount { get; set; }
    }
}
