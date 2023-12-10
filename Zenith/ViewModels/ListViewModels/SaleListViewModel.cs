using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Extensions;
using Zenith.Assets.Utils;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Repositories;
using Zenith.Views.ListViews;

namespace Zenith.ViewModels.ListViewModels
{
    public class SaleListViewModel : BaseListViewModel<Sale>
    {
        public SaleListViewModel(Repository<Sale> repository, SearchBaseDto searchModel, IObservable<Func<Sale, bool>> criteria)
            : base(repository, searchModel, criteria, PermissionTypes.Sales)
        {
            var configurationRepository = new ConfigurationRepository();

            SalesPrePrintDto = new SalesPrePrintDto
            {
                Materials = new MaterialRepository().AllIncludeMixed().Select(m => (Material)m.Clone()).ToObservableCollection(),
            };

            if (int.TryParse(configurationRepository.Single(ConfigurationKeys.LastPrintedFactorNumber).Value, out int lastPrintedFactorNo))
                SalesPrePrintDto.FactorNumber = lastPrintedFactorNo + 1;
            else
                SalesPrePrintDto.FactorNumber = 100301;

            AddNewCommand = ReactiveCommand.CreateFromObservable<bool, Unit>(isIndirectSale =>
                CreateCommand.Execute()
                .Do(_ => CreateUpdatePage.ViewModel.PageModel.IsIndirectSale = isIndirectSale));

            this.WhenAnyValue(vm => vm.IsInPrePrintMode)
                .Where(iipm => iipm)
                .Do(_ =>
                {
                    SalesPrePrintDto.Sites = new SiteRepository()
                        .Find(s => s.CompanyId == ActiveList.Select(s => s.CompanyId).FirstOrDefault())
                        .Select(s => (Site)s.Clone())
                        .ToObservableCollection();
                }).Subscribe();

            HidePrePrintGridCommand = ReactiveCommand.Create<Unit>(_ => IsInPrePrintMode = false);

            PrintAggregateFactorPreviewCommand = ReactiveCommand.Create<Unit>(_ =>
            {
                PrintableDeliveries.Clear();

                var salesToPrint = SalesPrePrintDto.LpoNumber.IsNullOrWhiteSpace() ? ActiveList.AsEnumerable() : ((SaleRepository)repository).FindByLpo(SalesPrePrintDto.LpoNumber);

                PrintableDeliveries.AddRange(
                    salesToPrint.SelectMany(s =>
                        s.Items.SelectMany(si => si.Deliveries)
                            .Where(d => (!SalesPrePrintDto.Sites.Any(site => site.IsSelected) || SalesPrePrintDto.Sites.Any(site => site.IsSelected && site.SiteId == d.Site.SiteId)) &&
                                        (!SalesPrePrintDto.Materials.Any(m => m.IsSelected) || SalesPrePrintDto.Materials.Any(m => m.IsSelected && m.MaterialId == d.SaleItem.MaterialId)) &&
                                        (SalesPrePrintDto.LpoNumber.IsNullOrWhiteSpace() || d.LpoNumber == SalesPrePrintDto.LpoNumber))).ToList());
            });

            PrintFactorCommand = ReactiveCommand.CreateRunInBackground<Sale>(sale =>
            {
                WordUtil.PrintFactor(null, null, null, null, null, repository.Single(sale.SaleId));
            });

            PrintAggregateFactorCommand = ReactiveCommand.CreateRunInBackground<Unit>(_ =>
            {
                var salesToPrint = SalesPrePrintDto.LpoNumber.IsNullOrWhiteSpace() ? ActiveList.AsEnumerable() : ((SaleRepository)repository).FindByLpo(SalesPrePrintDto.LpoNumber);

                WordUtil.PrintFactor(SalesPrePrintDto.FactorNumber, SalesPrePrintDto.Sites.Where(s => s.IsSelected).Select(s => s.SiteId).ToList(), SalesPrePrintDto.Materials.Where(m => m.IsSelected).Select(m => m.MaterialId).ToList(), SalesPrePrintDto.LpoNumber, PrintableDeliveries.Where(d => d.IsSelected).Select(d => d.DeliveryId).ToList(), salesToPrint.Select(s => repository.Single(s.SaleId)).ToArray());

                if (SalesPrePrintDto.FactorNumber > lastPrintedFactorNo)
                {
                    configurationRepository.AddOrUpdateRange(new List<Configuration>
                    {
                        new Configuration 
                        {
                            Key = $"{ConfigurationKeys.LastPrintedFactorNumber}",
                            Value = $"{SalesPrePrintDto.FactorNumber}"
                        }
                    });

                    lastPrintedFactorNo = SalesPrePrintDto.FactorNumber;
                }

                SalesPrePrintDto.FactorNumber = lastPrintedFactorNo + 1;
            });

        }

        public ReactiveCommand<bool, Unit> AddNewCommand { get; set; }
        public ReactiveCommand<Unit, Unit> HidePrePrintGridCommand { get; set; }
        public ReactiveCommand<Sale, Unit> PrintFactorCommand { get; set; }
        public ReactiveCommand<Unit, Unit> PrintAggregateFactorCommand { get; set; }
        public ReactiveCommand<Unit, Unit> PrintAggregateFactorPreviewCommand { get; set; }

        #region Print Filter
        [Reactive]
        public bool IsInPrePrintMode { get; set; }

        [Reactive]
        public SalesPrePrintDto SalesPrePrintDto { get; set; }

        [Reactive]
        public ObservableCollection<Delivery> PrintableDeliveries { get; set; } = new ObservableCollection<Delivery>();
        #endregion
    }
}
