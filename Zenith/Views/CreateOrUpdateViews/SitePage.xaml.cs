using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Repositories;
using Zenith.ViewModels.CreateOrUpdateViewModels;

namespace Zenith.Views.CreateOrUpdateViews
{
    /// <summary>
    /// Interaction logic for SiteCategoryPage.xaml
    /// </summary>
    public partial class SitePage : BaseCreateOrUpdatePage<Site>
    {
        public SitePage()
        {
            InitializeComponent();

            ViewModel = new BaseCreateOrUpdateViewModel<Site>(new SiteRepository());

            this.WhenActivated(d =>
            {
                companyComboBox.ItemsSource = new CompanyRepository().All().ToList();
            });
        }
    }
}
