using ReactiveUI;
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

        private string viewTitle;
        public string ViewTitle
        {
            get { return viewTitle; }
            set { this.RaiseAndSetIfChanged(ref viewTitle, value); }
        }

        private string itemStatistics;
        public string ItemsStatistics
        {
            get { return itemStatistics; }
            set { this.RaiseAndSetIfChanged(ref itemStatistics, value); }
        }
    }
}
