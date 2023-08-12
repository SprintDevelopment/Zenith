using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Assets.Values.Dtos
{
    public class TreeViewItemDto : BaseDto
    {
        [Reactive]
        public object Id { get; set; }

        [Reactive]
        public TreeViewItemDto Parent { get; set; }

        [Reactive]
        public string Title { get; set; }

        [Reactive]
        public bool IsExpanded { get; set; }

        [Reactive]
        public IEnumerable<TreeViewItemDto> Children { get; set; } = Enumerable.Empty<TreeViewItemDto>();
    }
}
