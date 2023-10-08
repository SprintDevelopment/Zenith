using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Assets.Values.Dtos
{
    public class SalaryStatisticsDto : BaseDto
    {
        [Reactive]
        public DateTime LastPaymentDate { get; set; }

        [Reactive]
        public DateTime PaymentDate { get; set; }

        public int LastDaysCount { get => PaymentDate.Subtract(LastPaymentDate).Days; }

        [Reactive]
        public int OffDaysCount { get; set; }

        public int WorkingDaysCount { get => LastDaysCount - OffDaysCount; }

        [Reactive]
        public float Salary { get; set; }

        public float DailySalary { get => Salary / 30f; }

        public float WorkingDaysSalary { get => WorkingDaysCount * DailySalary; }

        [Reactive]
        public float Credit { get; set; }

        [Reactive]
        public float Overtime { get; set; }

        public float Overall { get => WorkingDaysSalary + Credit + Overtime; }

        [Reactive]
        public string LastPaymentAndDaysLastString { get; set; } = string.Empty;

        [Reactive]
        public string OffDaysAndWorkingDaysString { get; set; } = string.Empty;

        [Reactive]
        public string SalaryAndDailySalaryString { get; set; } = string.Empty;

        [Reactive]
        public string CreditAndOvertimeString { get; set; } = string.Empty;

        [Reactive]
        public string OverallString { get; set; } = string.Empty;

        public SalaryStatisticsDto()
        {
            this.WhenAnyValue(d => d.LastPaymentDate, d => d.OffDaysCount, d => d.Salary, d => d.Credit, d => d.Overtime)
                .Do(d =>
                {
                    LastPaymentAndDaysLastString = string.Format((string)App.Current.Resources["SalaryPaymentPage.LastPaymentAndDaysLastStringFormat"], LastPaymentDate, LastDaysCount);
                    OffDaysAndWorkingDaysString = string.Format((string)App.Current.Resources["SalaryPaymentPage.OffDaysAndWorkingDaysStringFormat"], OffDaysCount, WorkingDaysCount);
                    SalaryAndDailySalaryString = string.Format((string)App.Current.Resources["SalaryPaymentPage.SalaryAndDailySalaryStringFormat"], Salary, DailySalary, WorkingDaysSalary);
                    CreditAndOvertimeString = string.Format((string)App.Current.Resources["SalaryPaymentPage.CreditAndOvertimeStringFormat"], Credit, Overtime);
                    OverallString = string.Format((string)App.Current.Resources["SalaryPaymentPage.OverallStringFormat"], WorkingDaysSalary, Credit, Overtime, Overall);
                }).Subscribe();
        }
    }
}
