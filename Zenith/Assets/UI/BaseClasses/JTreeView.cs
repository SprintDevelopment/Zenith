using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
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

namespace Zenith.Assets.UI.BaseClasses
{
    public class JTreeView<T> : ActivatableUserControl, IViewFor where T : Model, new()
    {
        public string Title
        {
            get { return (string)base.GetValue(TitleProperty); }
            set { base.SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(JTreeView<T>), new PropertyMetadata(""));

        public T SelectedItem
        {
            get { return (T)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(T), typeof(JTreeView<T>), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public IEnumerable<T> ItemsSource
        {
            get { return (IEnumerable<T>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable<T>), typeof(JTreeView<T>), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

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

            contentGrid.Children.Add(treeViewPopup);


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

                this.WhenAnyValue(v => v.ItemsSource)
                    .WhereNotNull()
                    .Do(items => itemsTreeView.ItemsSource = items.GetHierarchyCollection())
                    .Subscribe().DisposeWith(d);

                Observable.FromEventPattern(itemsTreeView, nameof(itemsTreeView.SelectedItemChanged))
                    .Do(_ => SelectedItem = (T)itemsTreeView.SelectedItem)
                    .Subscribe().DisposeWith(d);

                //this.WhenAnyValue(v => v.SelectedItem)
                //    .WhereNotNull()
                //    .Where(x => x != itemsTreeView.SelectedValue)
                //    .Do(x =>
                //    {
                //        x.IsSelected = true;
                //        var node = x;
                //        node.IsExpanded = true;
                //    })
                //    .Subscribe().DisposeWith(d);
            });
        }

        public object? ViewModel { get; set; }
    }
}
