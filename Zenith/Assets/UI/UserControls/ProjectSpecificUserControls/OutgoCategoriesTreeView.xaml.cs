using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
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
using Zenith.Assets.Extensions;
using Zenith.Assets.UI.BaseClasses;
using Zenith.Assets.UI.Helpers;
using Zenith.Assets.Values.Dtos;
using Zenith.Models;

namespace Zenith.Assets.UI.UserControls
{
    /// <summary>
    /// Interaction logic for TreeView.xaml
    /// </summary>
    public partial class OutgoCategoriesTreeView : JTreeView<OutgoCategory>
    {
        public OutgoCategoriesTreeView()
        {
            InitializeComponent();
        }
    }
}
