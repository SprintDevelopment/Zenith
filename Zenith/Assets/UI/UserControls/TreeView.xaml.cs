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
using Zenith.Assets.UI.Helpers;
using Zenith.Assets.Values.Dtos;
using Zenith.Models;

namespace Zenith.Assets.UI.UserControls
{
    /// <summary>
    /// Interaction logic for TreeView.xaml
    /// </summary>
    public partial class TreeView : ActivatableUserControl, IViewFor
    {
        public string Title
        {
            get { return (string)base.GetValue(TitleProperty); }
            set { base.SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(TreeView), new PropertyMetadata(""));

        public TreeViewItemDto SelectedItem
        {
            get { return (TreeViewItemDto)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }
        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register("SelectedValue", typeof(TreeViewItemDto), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public IEnumerable<TreeViewItemDto> ItemsSource
        {
            get { return (IEnumerable<TreeViewItemDto>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable<TreeViewItemDto>), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public TreeView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                Observable.FromEventPattern(showHidePopupButton, nameof(Button.Click))
                    .Do(_ => treeViewPopup.IsOpen = !treeViewPopup.IsOpen)
                    .Subscribe().DisposeWith(d);

                Observable.FromEventPattern(itemsTreeView, nameof(itemsTreeView.SelectedItemChanged))
                    .Do(_ => SelectedItem = (TreeViewItemDto)itemsTreeView.SelectedValue)
                    .Subscribe().DisposeWith(d);

                this.WhenAnyValue(v => v.SelectedItem)
                    .WhereNotNull()
                    .Where(x => x != itemsTreeView.SelectedValue)
                    .Do(x => 
                    {
                        x.IsSelected = true;
                        var node = x;
                        node.IsExpanded = true;

                        while (node.Parent is not null)
                        {
                            node = node.Parent;
                        }
                    })
                    .Subscribe().DisposeWith(d);
            });
        }

        public object? ViewModel { get; set; }
    }
}
