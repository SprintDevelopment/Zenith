using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.PortableExecutable;
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
        MachineIncomeRepository MachineIncomeRepository = new MachineIncomeRepository();

        public override IEnumerable<Outgo> All()
        {
            return _context.Set<Outgo>()
                .Include(o => o.OutgoCategory)
                .Include(o => o.Company)
                .Where(o => !o.RelatedOutgoPlusTransportId.HasValue)
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
            {
                CashRepository.Add(MapperUtil.Mapper.Map<Cash>(outgo));

                if (outgo.OutgoType == OutgoTypes.DirectIncludeTransportation)
                {
                    var addedOutgo = MapperUtil.Mapper.Map<Outgo>(outgo);

                    addedOutgo.OutgoId = 0;
                    addedOutgo.OutgoCategoryId = 1;
                    addedOutgo.Amount = 1;
                    addedOutgo.CashState = CashStates.Cash;
                    addedOutgo.Value = outgo.MachineIncomeValue;
                    addedOutgo.MachineIncomeValue = 0;
                    addedOutgo.OutgoType = OutgoTypes.Direct;
                    addedOutgo.RelatedOutgoPlusTransportId = outgo.OutgoId;
                    addedOutgo.Comment = $"It's for transport section of outgo #{outgo.OutgoId}";

                    var addedIncome = new MachineIncome
                    {
                        IncomeCategoryId = 1,
                        Amount = 1,
                        Value = outgo.MachineIncomeValue,
                        DateTime = outgo.DateTime,
                        CompanyId = outgo.CompanyId,
                        CashState = CashStates.Cash,
                        MachineId = outgo.MachineId.Value,
                        RelatedOutgoPlusTransportId = outgo.OutgoId,
                        Comment = $"It's for transport section of outgo #{outgo.OutgoId}",
                    };

                    Add(addedOutgo);
                    MachineIncomeRepository.Add(addedIncome);
                }
            }
            else
            {
                var consumableAccount = AccountRepository.Single((short)3);
                consumableAccount.CreditValue += outgo.Value;
                AccountRepository.Update(consumableAccount, consumableAccount.AccountId);

                var workshopAccount = AccountRepository.Single((short)1);
                workshopAccount.CreditValue -= outgo.Value;
                AccountRepository.Update(workshopAccount, workshopAccount.AccountId);
            }

            if (outgo.OutgoType != OutgoTypes.Direct && outgo.OutgoType != OutgoTypes.DirectIncludeTransportation)
                OutgoCategoryRepository.UpdateAmount(outgo.OutgoCategoryId, outgo.Amount * (outgo.OutgoType == OutgoTypes.BuyConsumables ? 1 : -1), outgo.Value * (outgo.OutgoType == OutgoTypes.BuyConsumables ? 1 : -1));

            return outgo;
        }

        public override Outgo Update(Outgo outgo, dynamic outgoId)
        {
            var oldOutgo = Single((int)outgoId);

            if (outgo.OutgoType != OutgoTypes.Direct && outgo.OutgoType != OutgoTypes.DirectIncludeTransportation)
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

                if (outgo.OutgoType == OutgoTypes.DirectIncludeTransportation)
                {
                    var relatedAddedOutgo = _context.Set<Outgo>().FirstOrDefault(o => o.RelatedOutgoPlusTransportId == outgo.OutgoId);
                    if (relatedAddedOutgo != null)
                    {
                        relatedAddedOutgo.OutgoCategoryId = 1;
                        relatedAddedOutgo.Value = outgo.MachineIncomeValue;
                        relatedAddedOutgo.CompanyId = outgo.CompanyId;
                        relatedAddedOutgo.DateTime = outgo.DateTime;

                        Update(relatedAddedOutgo, relatedAddedOutgo.OutgoId);
                    }

                    var relatedAddedMachineIncome = _context.Set<MachineIncome>().FirstOrDefault(mi => mi.RelatedOutgoPlusTransportId == outgo.OutgoId);
                    if (relatedAddedMachineIncome != null)
                    {
                        relatedAddedMachineIncome.Value = outgo.MachineIncomeValue;
                        relatedAddedMachineIncome.DateTime = outgo.DateTime;
                        relatedAddedMachineIncome.CompanyId = outgo.CompanyId;
                        relatedAddedMachineIncome.MachineId = outgo.MachineId.Value;

                        MachineIncomeRepository.Update(relatedAddedMachineIncome, relatedAddedMachineIncome.IncomeId);
                    }
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

            if (outgo.OutgoType != OutgoTypes.Direct && outgo.OutgoType != OutgoTypes.DirectIncludeTransportation)
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


            outgoes.Where(o => o.OutgoType != OutgoTypes.Direct && o.OutgoType != OutgoTypes.DirectIncludeTransportation)
                .ToList()
                .ForEach(o => OutgoCategoryRepository.UpdateAmount(o.OutgoCategoryId, o.Amount * (o.OutgoType == OutgoTypes.BuyConsumables ? -1 : 1), o.Value * (o.OutgoType == OutgoTypes.BuyConsumables ? -1 : 1)));

            var directIncludeTransportationOutgoesIds = outgoes.Where(o => o.OutgoType == OutgoTypes.DirectIncludeTransportation).Select(o => o.OutgoId).ToList();
            if (directIncludeTransportationOutgoesIds.Any())
            {
                var relatedOutgoes = _context.Set<Outgo>().Where(o => o.RelatedOutgoPlusTransportId.HasValue && outgoesIds.Contains(o.RelatedOutgoPlusTransportId.Value));
                RemoveRange(relatedOutgoes);

                var relatedMachineIncomes = _context.Set<MachineIncome>().Where(mi => mi.RelatedOutgoPlusTransportId.HasValue && outgoesIds.Contains(mi.RelatedOutgoPlusTransportId.Value));
                MachineIncomeRepository.RemoveRange(relatedMachineIncomes);
            }

        }
    }
}
