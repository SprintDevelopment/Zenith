using ReactiveUI.Fody.Helpers;
using Zenith.Assets.Attributes;
using Zenith.Assets.Values.Dtos;

namespace Zenith.Models.SearchModels
{
    public class PersonSearchModel : SearchBaseDto
    {
        [Search(Title = "نام پرسنل")]
        [Reactive]
        public string Name { get; set; }
    }
}
