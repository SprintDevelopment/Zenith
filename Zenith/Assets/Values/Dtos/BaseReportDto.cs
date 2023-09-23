using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zenith.Assets.Values.Dtos
{
    public class BaseReportDto : ReactiveObject
    {
        [Reactive]
        public int DisplayOrder { get; set; }
    }
}
