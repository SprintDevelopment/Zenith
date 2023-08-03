using System;
using System.Windows;
using System.Windows.Controls;
using Zenith.Assets.UI.CustomEventArgs;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;

namespace Zenith.Assets.UI.UserControls
{
    /// <summary>
    /// Interaction logic for Dialog.xaml
    /// </summary>
    public partial class Dialog : UserControl
    {
        public Dialog()
        {
            InitializeComponent();

            closeButton.Click += (s, e) => { Returned?.Invoke(s, new DialogEventArgs { DialogResult = DialogResults.Cancel }); Visibility = Visibility.Collapsed; };
        }

        public void Initialize(DialogDto dialogDto)
        {
            this.DataContext = dialogDto;

            choicesContainerGrid.Children.Clear();
            choicesContainerGrid.ColumnDefinitions.Clear();

            int counter = 0;
            foreach (var choice in dialogDto.Choices)
            {
                choicesContainerGrid.ColumnDefinitions.Add(new ColumnDefinition());

                var newButton = new Button { Style = FindResource("ConfirmButtonStyle") as Style, Content = choice.Text, Margin = new Thickness(4, 0, 4, 0) };
                newButton.SetValue(Grid.ColumnProperty, counter++);
                newButton.Click += (s, e) => { Returned?.Invoke(s, new DialogEventArgs { DialogResult = choice.DialogResult }); Visibility = Visibility.Collapsed; };
                choicesContainerGrid.Children.Add(newButton);
            }

            Visibility = Visibility.Visible;
        }

        public event EventHandler<DialogEventArgs> Returned;
    }
}
