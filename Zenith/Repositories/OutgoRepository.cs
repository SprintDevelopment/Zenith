using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Zenith.Assets.Utils;
using Zenith.Assets.Values.Enums;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class OutgoRepository : Repository<Outgo>
    {
        CashRepository CashRepository = new CashRepository();
        AccountRepository AccountRepository = new AccountRepository();

        public override IEnumerable<Outgo> All()
        {
            return _context.Set<Outgo>()
                .Include(o => o.OutgoCategory)
                .Include(o => o.Company)
                .AsEnumerable();
        }

        public override Outgo Single(dynamic id)
        {
            int intId = (int)id;
            return _context.Set<Outgo>()
                .Include(o => o.OutgoCategory)
                .Include(o => o.Company)
                .SingleOrDefault(o => o.OutgoId == intId);
        }

        public override Outgo Add(Outgo outgo)
        {
            base.Add(outgo);

            if (outgo.OutgoType != OutgoTypes.UseConsumables)
                CashRepository.Add(MapperUtil.Mapper.Map<Cash>(outgo));
            else
            {
                var consumableAccount = AccountRepository.Single((short)3);
                consumableAccount.Balance -= outgo.Value;
                consumableAccount.CreditValue += outgo.Value;
                AccountRepository.Update(consumableAccount, consumableAccount.AccountId);

                var workshopAccount = AccountRepository.Single((short)1);
                workshopAccount.Balance += outgo.Value;
                workshopAccount.CreditValue -= outgo.Value;
                AccountRepository.Update(consumableAccount, consumableAccount.AccountId);
            }

            return outgo;
        }

        public override Outgo Update(Outgo outgo, dynamic outgoId)
        {
            base.Update(outgo, outgo.OutgoId);

            if (outgo.OutgoType != OutgoTypes.UseConsumables)
            {
                var relatedCash = CashRepository.Find(c => c.MoneyTransactionType == MoneyTransactionTypes.Outgo && c.RelatedEntityId == outgo.OutgoId)
                    .Select(c => MapperUtil.Mapper.Map<Cash>(c))
                    .FirstOrDefault();

                if (relatedCash is not null)
                {
                    MapperUtil.Mapper.Map(outgo, relatedCash);
                    CashRepository.Update(relatedCash, relatedCash.CashId);
                }
            }

            return outgo;
        }

        public override void RemoveRange(IEnumerable<Outgo> outgoes)
        {
            var outgoesIds = outgoes.Where(o => o.OutgoType != OutgoTypes.UseConsumables).Select(b => b.OutgoId).ToList();

            base.RemoveRange(outgoes);

            var relatedCashes = CashRepository.Find(c => c.MoneyTransactionType == MoneyTransactionTypes.Outgo && outgoesIds.Contains(c.RelatedEntityId));
            CashRepository.RemoveRange(relatedCashes);
        }
    }
}
