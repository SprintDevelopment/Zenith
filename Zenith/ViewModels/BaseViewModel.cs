using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Models;

namespace Zenith.ViewModels
{
    public class BaseViewModel<T> : ReactiveObject where T : Model, new()
    {
        public BaseViewModel()
        {
        }

        [Reactive]
        public string ViewTitle { get; set; }

        [Reactive]
        public string ItemsStatistics { get; set; }
    }
}
