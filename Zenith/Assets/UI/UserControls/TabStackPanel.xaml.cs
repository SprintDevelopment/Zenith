using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Zenith.Assets.UI.Helpers;
using Zenith.ViewModels;

namespace Zenith.Assets.UI.UserControls
{
    /// <summary>
    /// Interaction logic for TabStackPanel.xaml
    /// </summary>
    public partial class TabStackPanel : ActivatableUserControl, IViewFor<TabControlViewModel>
    {
        public TabStackPanel()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                ViewModel.Tabs
                    .ToObservableChangeSet()
                    .Transform(tvm => new TabHeader { ViewModel = tvm })
                    .OnItemAdded(addedTabHeader => tabsPanel.Children.Add(addedTabHeader))
                    .OnItemRemoved(removedTabHeader => tabsPanel.Children.Remove(removedTabHeader))
                    .Subscribe().DisposeWith(d);

                Observable.FromEventPattern(this, nameof(PreviewMouseMove)).Select(x => x.EventArgs as MouseEventArgs)
                    .Merge(Observable.FromEventPattern(this, nameof(PreviewMouseDoubleClick)).Select(x => { Debug.WriteLine("DOUBLE_CLICKED"); return x.EventArgs as MouseEventArgs; }))
                    .Do(x => x.Handled = true)
                    .Subscribe().DisposeWith(d);
            });
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (TabControlViewModel)value; }
        }
        
        public TabControlViewModel ViewModel { get; set; }
    }
}
