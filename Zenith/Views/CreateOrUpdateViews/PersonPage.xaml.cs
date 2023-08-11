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

            this.WhenActivated(d =>
            {
                jobComboBox.ItemsSource = typeof(Jobs).ToCollection();
            });
        }
    }
}
