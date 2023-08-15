using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Zenith.Assets.Extensions;
using Zenith.Assets.UI.Helpers;
using Zenith.Assets.Values.Dtos;
using Zenith.Models;
using Zenith.ViewModels;

namespace Zenith.Assets.UI.BaseClasses
{
    public class JTreeViewBase : ActivatableUserControl
    {
        public string Title
        {
            get { return (string)base.GetValue(TitleProperty); }
            set { base.SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(JTreeViewBase), new PropertyMetadata(""));
    }

    public class JTreeView<T> : JTreeViewBase, IViewFor<JTreeViewModel<T>> where T : Model, new()
    {
        JTextBox selectedItemTitleTextBox;
        Button showHidePopupButton;
        Popup treeViewPopup;
        TreeView itemsTreeView;

        public JTreeView()
        {
            //<baseclasses:JTextBox x:Name="SelectedItemTitleTextBox" Title="{Binding Title, ElementName=owner}" MinWidth="300" Margin="0"/>
            //<Button x:Name="showHidePopupButton" Width="42" HorizontalAlignment="Right" Margin="0,1" Style="{DynamicResource SimpleCenteredButtonStyle}" BorderThickness="0">
            //    <Image Source = "/Zenith;component/Resources/Images/Common/down.png" HorizontalAlignment="Center" VerticalAlignment="Center" Stretch="Fill" Width="14" Height="14"/>
            //</Button>

            //<Popup x:Name="treeViewPopup" Grid.Row="1" PopupAnimation="{DynamicResource {x:Static SystemParameters.ComboBoxPopupAnimationKey}}" StaysOpen="False">
            //    <Grid Height = "320" Width="{Binding ActualWidth, ElementName=SelectedItemTitleTextBox}" Background="White">
            //        <Rectangle Stroke = "#FF3CA3F4" />
            //        < TreeView x:Name="itemsTreeView" ItemsSource="{Binding HierarchicalItemsSource, ElementName=owner}" 
            //                                         VirtualizingStackPanel.IsVirtualizing="False" />
            //    </Grid>
            //</Popup>

            var contentGrid = new Grid();
            contentGrid.RowDefinitions.AddRange(
                new RowDefinition[]
                {
                    new RowDefinition { Height = new GridLength(42) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }
                });
            selectedItemTitleTextBox = new JTextBox { MinWidth = 300, Margin = new Thickness(0) };
            contentGrid.Children.Add(selectedItemTitleTextBox);

            showHidePopupButton = new Button { Width = 42, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 1, 0, 1), BorderThickness = new Thickness(0), Style = (Style)FindResource("SimpleCenteredButtonStyle") };
            showHidePopupButton.Content = new Image
            {
                Source = new BitmapImage(new Uri(@"/Zenith;component/Resources/Images/Common/down.png", UriKind.Relative)),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Stretch = Stretch.Fill,
                Width = 14,
                Height = 14
            };
            contentGrid.Children.Add(showHidePopupButton);

            treeViewPopup = new Popup { PopupAnimation = SystemParameters.ComboBoxPopupAnimation, StaysOpen = false };
            treeViewPopup.SetValue(Grid.RowProperty, 1);

            var popupContentGrid = new Grid { Height = 320, Background = Brushes.White };
            popupContentGrid.Children.Add(new Rectangle { Stroke = new SolidColorBrush(Color.FromRgb(60, 163, 244)) });
            itemsTreeView = new TreeView();
            itemsTreeView.SetValue(VirtualizingStackPanel.IsVirtualizingProperty, false);
            popupContentGrid.Children.Add(itemsTreeView);
            treeViewPopup.Child = popupContentGrid;
            contentGrid.Children.Add(treeViewPopup);

            this.Content = contentGrid;
            ViewModel = new JTreeViewModel<T>();
            this.WhenActivated(d =>
            {
                Observable.FromEventPattern(showHidePopupButton, nameof(Button.Click))
                    .Do(_ => treeViewPopup.IsOpen = !treeViewPopup.IsOpen)
                    .Subscribe().DisposeWith(d);

                this.WhenAnyValue(v => v.Title)
                    .BindTo(this, v => v.selectedItemTitleTextBox.Text)
                    .DisposeWith(d);

                this.WhenAnyValue(v => v.selectedItemTitleTextBox.ActualWidth)
                    .BindTo(this, v => v.treeViewPopup.Width)
                    .DisposeWith(d);

                ViewModel.WhenAnyValue(vm => vm.ItemsSource)
                    .WhereNotNull()
                    .Select(i => i.GetHierarchyCollection())
                    .Do(hi => itemsTreeView.ItemsSource = hi)
                    .Subscribe().DisposeWith(d);

                //this.OneWayBind(ViewModel, vm => vm.ItemsSource, v => v.itemsTreeView.ItemsSource, i => i?.GetHierarchyCollection()).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.SelectedItem, v => v.selectedItemTitleTextBox.Text, s => s?.ToString()).DisposeWith(d);

                Observable.FromEventPattern(itemsTreeView, nameof(itemsTreeView.SelectedItemChanged))
                    .Do(_ =>
                    {
                        ViewModel.SelectedItem = ViewModel.ItemsSource?.SingleOrDefault(item => item.GetKeyPropertyValue().ToString() == ((TreeViewItemDto)itemsTreeView.SelectedItem).Id.ToString());
                        treeViewPopup.IsOpen = false;
                    })
                    .Subscribe().DisposeWith(d);

                ViewModel.WhenAnyValue(vm => vm.SelectedItem)
                    .WhereNotNull()
                    .Where(x => x != itemsTreeView.SelectedValue)
                    .Select(x => ((IEnumerable<TreeViewItemDto>)itemsTreeView.ItemsSource).SearchInHierarchyCollection(x))
                    .WhereNotNull()
                    .Do(tvi => 
                    {
                        tvi.IsSelected = true;
                        while (tvi.Parent is not null) 
                        {
                            tvi = tvi.Parent;
                            tvi.IsExpanded = true;
                        }
                    }).Subscribe().DisposeWith(d);
            });
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (JTreeViewModel<T>)value; }
        }

        public JTreeViewModel<T> ViewModel { get; set; }
    }
}
