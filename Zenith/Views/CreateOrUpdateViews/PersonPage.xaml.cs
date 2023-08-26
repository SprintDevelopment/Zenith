using ReactiveUI;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Repositories;
using Zenith.ViewModels.CreateOrUpdateViewModels;

namespace Zenith.Views.CreateOrUpdateViews
{
    /// <summary>
    /// Interaction logic for PersonPage.xaml
    /// </summary>
    public partial class PersonPage : BaseCreateOrUpdatePage<Person>
    {
        public PersonPage()
        {
            InitializeComponent();

            ViewModel = new BaseCreateOrUpdateViewModel<Person>(new PersonRepository());

            // Should be out of this.WhenActivated !!
            jobComboBox.ItemsSource = typeof(Jobs).ToCollection();

            this.WhenActivated(d =>
            {
            });
        }
    }
}
