using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Zenith.Assets.Utils;
using Zenith.Assets.Values.Enums;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class SalaryPaymentRepository : Repository<SalaryPayment>
    {
        CashRepository CashRepository = new CashRepository();

        public override IEnumerable<SalaryPayment> All()
        {
            return _context.Set<SalaryPayment>()
                .Include(o => o.Personnel)
                .AsEnumerable();
        }

        public override SalaryPayment Single(dynamic id)
        {
            int intId = (int)id;
            return _context.Set<SalaryPayment>()
                .Include(o => o.Personnel)
                .SingleOrDefault(o => o.SalaryPaymentId == intId);
        }

        public override SalaryPayment Add(SalaryPayment salaryPayment)
        {
            var salaryStatistics = new PersonRepository().GetSalaryStatistics(salaryPayment.PersonId, salaryPayment.DateTime);
            salaryPayment.Credit = salaryStatistics.Overall - salaryPayment.PaidValue;
            
            base.Add(salaryPayment);


            //PersonRepository.UpdateCredit(salaryPayment.PersonId, newCredit);
            CashRepository.Add(MapperUtil.Mapper.Map<Cash>(salaryPayment));

            return salaryPayment;
        }

        public override void RemoveRange(IEnumerable<SalaryPayment> salaryPayments)
        {
            var paymentsIds = salaryPayments.Select(b => b.SalaryPaymentId).ToList();
            
            base.RemoveRange(salaryPayments);

            var relatedCashes = CashRepository.Find(c => (c.MoneyTransactionType == MoneyTransactionTypes.WorkshopSalary || c.MoneyTransactionType == MoneyTransactionTypes.TransportaionSalary) && paymentsIds.Contains(c.RelatedEntityId));
            CashRepository.RemoveRange(relatedCashes);
        }
    }
}
