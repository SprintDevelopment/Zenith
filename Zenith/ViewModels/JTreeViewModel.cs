using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.ViewModels
{
    public class JTreeViewModel<T> : ReactiveObject
    {
        [Reactive]
        public IEnumerable<T> ItemsSource { get; set; }

        [Reactive]
        public T SelectedItem { get; set; }
    }
}
