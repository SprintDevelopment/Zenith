using ReactiveUI.Fody.Helpers;

namespace Zenith.Models
{
    public class Outgo : OutgoBase
    {
        [Reactive]
        public int? RelatedOutgoPlusTransportId { get; set; }
    }
}
