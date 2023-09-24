using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Zenith.Assets.UI.Helpers;
using Zenith.Models.ReportModels;
using Zenith.ViewModels.ListViewModels;
using Zenith.ViewModels.ReportViewModels;

namespace Zenith.Views.ReportViews
{
    public class BaseReportPage<T> : ActivatablePage, IViewFor<BaseReportViewModel<T>> where T : ReportModel, new()
    {
        public BaseReportPage()
        {
            this.WhenActivated(d =>
            {
                this.DataContext = ViewModel;

                var modalBackRect = ((Grid)Content).Children.OfType<Rectangle>().Single(r => r.Name == "modalBackRect");
                modalBackRect.InputBindings.Add(new MouseBinding(ViewModel.HideSearchGridCommand, new MouseGesture(MouseAction.LeftClick)));

                ViewModel.IsInSearchMode = true;
            });
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (BaseReportViewModel<T>)value; }
        }

        public BaseReportViewModel<T> ViewModel { get; set; }
    }
}
