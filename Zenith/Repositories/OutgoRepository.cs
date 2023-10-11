using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Zenith.Assets.Extensions;
using Zenith.Assets.Utils;
using Zenith.Assets.Values.Enums;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class OutgoRepository : Repository<Outgo>
    {
        CashRepository CashRepository = new CashRepository();
        AccountRepository AccountRepository = new AccountRepository();
        OutgoCategoryRepository OutgoCategoryRepository = new OutgoCategoryRepository();

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
                consumableAccount.CreditValue += outgo.Value;
                AccountRepository.Update(consumableAccount, consumableAccount.AccountId);

                var workshopAccount = AccountRepository.Single((short)1);
                workshopAccount.CreditValue -= outgo.Value;
                AccountRepository.Update(workshopAccount, workshopAccount.AccountId);
            }

            if (outgo.OutgoType != OutgoTypes.Direct)
                OutgoCategoryRepository.UpdateAmount(outgo.OutgoCategoryId, outgo.Amount * (outgo.OutgoType == OutgoTypes.BuyConsumables ? 1 : -1), outgo.Value * (outgo.OutgoType == OutgoTypes.BuyConsumables ? 1 : -1));

            return outgo;
        }

        public override Outgo Update(Outgo outgo, dynamic outgoId)
        {
            var oldOutgo = Single((int)outgoId);

            if (outgo.OutgoType != OutgoTypes.Direct)
                OutgoCategoryRepository.UpdateAmount(oldOutgo.OutgoCategoryId, oldOutgo.Amount * (oldOutgo.OutgoType == OutgoTypes.BuyConsumables ? -1 : 1), oldOutgo.Value * (oldOutgo.OutgoType == OutgoTypes.BuyConsumables ? -1 : 1));

            base.Update(outgo, outgo.OutgoId);

            if (outgo.OutgoType != OutgoTypes.UseConsumables)
            {
                var relatedCash = CashRepository.Find(c => (c.MoneyTransactionType == MoneyTransactionTypes.CashOutgo ||
                                                          c.MoneyTransactionType == MoneyTransactionTypes.NonCashOutgo ||
                                                          c.MoneyTransactionType == MoneyTransactionTypes.CashBuyConsumables ||
                                                          c.MoneyTransactionType == MoneyTransactionTypes.NonCashBuyConsumables) && c.RelatedEntityId == outgo.OutgoId)
                    .Select(c => MapperUtil.Mapper.Map<Cash>(c))
                    .FirstOrDefault();

                if (relatedCash is not null)
                {
                    MapperUtil.Mapper.Map(outgo, relatedCash);
                    CashRepository.Update(relatedCash, relatedCash.CashId);
                }
            }
            else
            {
                var consumableAccount = AccountRepository.Single((short)3);
                consumableAccount.CreditValue += outgo.Value - oldOutgo.Value;
                AccountRepository.Update(consumableAccount, consumableAccount.AccountId);

                var workshopAccount = AccountRepository.Single((short)1);
                workshopAccount.CreditValue -= outgo.Value - oldOutgo.Value;
                AccountRepository.Update(workshopAccount, workshopAccount.AccountId);
            }

            if (outgo.OutgoType != OutgoTypes.Direct)
                OutgoCategoryRepository.UpdateAmount(outgo.OutgoCategoryId, outgo.Amount * (outgo.OutgoType == OutgoTypes.BuyConsumables ? 1 : -1), outgo.Value * (outgo.OutgoType == OutgoTypes.BuyConsumables ? 1 : -1));

            return outgo;
        }

        public override void RemoveRange(IEnumerable<Outgo> outgoes)
        {
            var outgoesIds = outgoes.Where(o => o.OutgoType != OutgoTypes.UseConsumables).Select(b => b.OutgoId).ToList();

            base.RemoveRange(outgoes);

            var relatedCashes = CashRepository.Find(c => (c.MoneyTransactionType == MoneyTransactionTypes.CashOutgo ||
                                                          c.MoneyTransactionType == MoneyTransactionTypes.NonCashOutgo ||
                                                          c.MoneyTransactionType == MoneyTransactionTypes.CashBuyConsumables ||
                                                          c.MoneyTransactionType == MoneyTransactionTypes.NonCashBuyConsumables) && outgoesIds.Contains(c.RelatedEntityId));
            CashRepository.RemoveRange(relatedCashes);

            var valueToSubtractFromConsumableAccountAndAddToWorkshopAccountCredits = outgoes.Where(o => o.OutgoType == OutgoTypes.UseConsumables)
                .Sum(o => o.Value);

            var consumableAccount = AccountRepository.Single((short)3);
            consumableAccount.CreditValue -= valueToSubtractFromConsumableAccountAndAddToWorkshopAccountCredits;
            AccountRepository.Update(consumableAccount, consumableAccount.AccountId);

            var workshopAccount = AccountRepository.Single((short)1);
            workshopAccount.CreditValue += valueToSubtractFromConsumableAccountAndAddToWorkshopAccountCredits;
            AccountRepository.Update(workshopAccount, workshopAccount.AccountId);


            outgoes.Where(o => o.OutgoType != OutgoTypes.Direct)
                .ToList()
                .ForEach(o => OutgoCategoryRepository.UpdateAmount(o.OutgoCategoryId, o.Amount * (o.OutgoType == OutgoTypes.BuyConsumables ? -1 : 1), o.Value * (o.OutgoType == OutgoTypes.BuyConsumables ? -1 : 1)));
        }
    }
}
