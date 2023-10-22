using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using Zenith.Assets.Extensions;
using Zenith.Assets.UI.Helpers;
using Zenith.Assets.Values.Dtos;

namespace Zenith.Assets.UI.UserControls
{
    /// <summary>
    /// Interaction logic for MultiSelectControl.xaml
    /// </summary>
    public partial class MultiSelectControl : ActivatableUserControl, IViewFor<object>
    {
        public string Title
        {
            get { return (string)base.GetValue(TitleProperty); }
            set { base.SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(MultiSelectControl), new PropertyMetadata(""));

        public ObservableCollection<EnumDto> ItemsSource
        {
            get { return (ObservableCollection<EnumDto>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<EnumDto>), typeof(MultiSelectControl), new PropertyMetadata(null));

        public int SelectionValue
        {
            get { return (int)GetValue(SelectionValueProperty); }
            set { SetValue(SelectionValueProperty, value); }
        }
        public static readonly DependencyProperty SelectionValueProperty = DependencyProperty.Register("SelectionValue", typeof(int), typeof(MultiSelectControl), new PropertyMetadata(0));

        public MultiSelectControl()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {

                Observable.FromEventPattern(showHidePopupButton, nameof(Button.Click))
                    .Do(_ => selectionPopup.IsOpen = !selectionPopup.IsOpen)
                    .Subscribe().DisposeWith(d);

                this.WhenAnyValue(v => v.SelectionValue)
                    .Do(v =>
                    {
                        ItemsSource.Select(item => item.IsSelected = ((int)item.Value | SelectionValue) == SelectionValue).ToList();
                        selectedItemsTextBox.Text = ItemsSource.Where(item => item.IsSelected).Select(item => item.Description).Join(", ");
                    })
                    .Subscribe().DisposeWith(d);

                ItemsSource.ToObservableChangeSet()
                    .AutoRefresh(i => i.IsSelected)
                    .ToCollection()
                    .Select(items => items.Where(i => i.IsSelected).Sum(i => (int)i.Value))
                    .BindTo(this, v => v.SelectionValue)
                    .DisposeWith(d);
            });
        }

        public object? ViewModel { get; set; }
    }
}
