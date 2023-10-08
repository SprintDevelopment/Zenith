using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Zenith.Assets.Values.Dtos;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class PersonRepository : Repository<Person>
    {
        public override Person Single(dynamic id)
        {
            int intId = (int)id;
            return _context.Set<Person>()
                .Include(s => s.PersonnelAbsences)
                .Include(s => s.PersonnelOvertimes)
                .SingleOrDefault(s => s.PersonId == intId);
        }

        public SalaryStatisticsDto GetSalaryStatistics(int pid, DateTime paymentDate)
        {
            var person = Single(pid);
            var lastPayment = _context.SalaryPayments
                .OrderBy(sp => sp.DateTime)
                .LastOrDefault(sp => sp.PersonId == pid)
                ?? new SalaryPayment() { DateTime = person.StartDate };

            var statistics = new SalaryStatisticsDto()
            {
                PaymentDate = paymentDate,
                LastPaymentDate = lastPayment.DateTime,
                OffDaysCount = person.PersonnelAbsences.Where(pa => pa.DateTime > lastPayment.DateTime && pa.DateTime <= paymentDate).Count(),
                Overtime = person.PersonnelOvertimes.Where(pa => pa.DateTime > lastPayment.DateTime && pa.DateTime <= paymentDate).Sum(po => po.Amount),
                Salary = person.Salary,
                Credit = lastPayment.Credit,
            };

            return statistics;
        }
    }
}
