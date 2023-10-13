using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zenith.Assets.Utils;
using System.Threading.Tasks;
using Zenith.Models;
using Zenith.Assets.Values.Enums;

namespace Zenith.Repositories
{
    public class ChequeRepository : Repository<Cheque>
    {
        CashRepository CashRepository = new CashRepository();

        public override Cheque Add(Cheque cheque)
        {
            base.Add(cheque);

            CashRepository.Add(MapperUtil.Mapper.Map<Cash>(cheque));

            return cheque;
        }

        public override Cheque Update(Cheque cheque, dynamic chequeId)
        {
            var oldCheque = Single((int)chequeId);

            base.Update(cheque, cheque.ChequeId);

            var relatedCash = CashRepository.Find(c => (c.MoneyTransactionType == MoneyTransactionTypes.NotPassedPaidCheque ||
                                                      c.MoneyTransactionType == MoneyTransactionTypes.NotPassedRecievedCheque ||
                                                      c.MoneyTransactionType == MoneyTransactionTypes.PassedPaidCheque ||
                                                      c.MoneyTransactionType == MoneyTransactionTypes.PassedRecievedCheque) && c.RelatedEntityId == cheque.ChequeId)
                .Select(c => MapperUtil.Mapper.Map<Cash>(c))
                .FirstOrDefault();

            if (relatedCash is not null)
            {
                MapperUtil.Mapper.Map(cheque, relatedCash);
                CashRepository.Update(relatedCash, relatedCash.CashId);
            }

            return cheque;
        }

        public override void RemoveRange(IEnumerable<Cheque> cheques)
        {
            var chequesIds = cheques.Select(b => b.ChequeId).ToList();

            base.RemoveRange(cheques);

            var relatedCashes = CashRepository.Find(c => (c.MoneyTransactionType == MoneyTransactionTypes.NotPassedPaidCheque ||
                                                      c.MoneyTransactionType == MoneyTransactionTypes.NotPassedRecievedCheque ||
                                                      c.MoneyTransactionType == MoneyTransactionTypes.PassedPaidCheque ||
                                                      c.MoneyTransactionType == MoneyTransactionTypes.PassedRecievedCheque) && chequesIds.Contains(c.RelatedEntityId));
            CashRepository.RemoveRange(relatedCashes);
        }
    }
}
