using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Linq;
using Zenith.Models;
using Zenith.Repositories;
using System.Reactive.Linq;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Utils;

namespace Zenith.ViewModels.CreateOrUpdateViewModels
{
    public class SalaryPaymentCreateOrUpdateViewModel : BaseCreateOrUpdateViewModel<SalaryPayment>
    {
        PersonRepository PersonRepository = new PersonRepository();

        public SalaryPaymentCreateOrUpdateViewModel(Repository<SalaryPayment> repository, bool containsDeleted = false)
            : base(repository, containsDeleted)
        {
            this.WhenAnyValue(vm => vm.PageModel)
                .Select(pm => pm.WhenAnyValue(m => m.PersonId, m => m.DateTime).Where(x => x.Item1 > 0))
                .Switch()
                .Do(x =>
                {
                    MapperUtil.Mapper.Map(new PersonRepository().GetSalaryStatistics(x.Item1, x.Item2), SalaryStatistics);
                }).Subscribe();

            this.WhenAnyValue(vm => vm.PageModel)
                .Select(pm => pm.WhenAnyValue(m => m.PersonId).Select(pid => PersonRepository.Single(pid)).WhereNotNull())
                .Switch()
                .Do(person =>
                {
                    PageModel.CostCenter = person.CostCenter;
                }).Subscribe();
        }

        [Reactive]
        public SalaryStatisticsDto SalaryStatistics { get; set; } = new SalaryStatisticsDto();
    }
}
