using ReactiveUI.Fody.Helpers;
using Zenith.Assets.Attributes;
using Zenith.Assets.Values.Dtos;

namespace Zenith.Models.SearchModels
{
    public class UserSearchModel : SearchBaseDto
    {
        [Search(Title = "نام کاربری")]
        [Reactive]
        public string Username { get; set; }
    }
}
