using ReactiveUI.Fody.Helpers;

namespace Zenith.Assets.Values.Dtos
{
	public class SearchBaseDto : BaseDto
    {
		public string Title { get; set; }

        [Reactive]
        public int OnlyForRefreshAfterUpdate { get; set; }
    }
}
