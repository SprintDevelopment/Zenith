using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using ReactiveUI;
using System.Reactive.Linq;

namespace Zenith.Assets.UI.BaseClasses
{
    class JGrid : Grid
    {
        public bool IsSelected
        {
            get { return (bool)base.GetValue(IsSelectedProperty); }
            set { base.SetValue(IsSelectedProperty, value); }
        }
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(JGrid), new PropertyMetadata(false));


        public ICommand Command
        {
            get { return (ICommand)base.GetValue(CommandProperty); }
            set { base.SetValue(CommandProperty, value);  }
        }
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(JGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public JGrid()
        {
            this.WhenAnyValue(g => g.Command)
                .WhereNotNull()
                .Do(command =>
                {
                    this.InputBindings.Clear();
                    this.InputBindings.Add(new MouseBinding(Command, new MouseGesture(MouseAction.LeftDoubleClick)) { CommandParameter = DataContext });
                }).Subscribe();
            //this.PreviewMouseLeftButtonDown += (s, e) => { if (e.ClickCount == 2) Command?.Execute(DataContext); };
        }
    }
}
