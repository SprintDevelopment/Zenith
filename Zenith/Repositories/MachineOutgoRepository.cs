using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Zenith.Assets.Utils;
using Zenith.Assets.Values.Enums;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class MachineOutgoRepository : Repository<MachineOutgo>
    {
        CashRepository CashRepository = new CashRepository();
        AccountRepository AccountRepository = new AccountRepository();
        OutgoCategoryRepository OutgoCategoryRepository = new OutgoCategoryRepository();

        public override IEnumerable<MachineOutgo> All()
        {
            return _context.Set<MachineOutgo>()
                .Include(o => o.OutgoCategory)
                .Include(o => o.Machine)
                .Include(o => o.Company)
                .AsEnumerable();
        }

        public override MachineOutgo Single(dynamic id)
        {
            int intId = (int)id;
            return _context.Set<MachineOutgo>()
                .Include(o => o.OutgoCategory)
                .Include(o => o.Machine)
                .Include(o => o.Company)
                .SingleOrDefault(o => o.OutgoId == intId);
        }

        public override MachineOutgo Add(MachineOutgo machineOutgo)
        {
            base.Add(machineOutgo);

            if (machineOutgo.OutgoType != OutgoTypes.UseConsumables)
                CashRepository.Add(MapperUtil.Mapper.Map<Cash>(machineOutgo));
            else
            {
                var consumableAccount = AccountRepository.Single((short)3);
                consumableAccount.CreditValue += machineOutgo.Value;
                AccountRepository.Update(consumableAccount, consumableAccount.AccountId);

                var transportationAccount = AccountRepository.Single((short)2);
                transportationAccount.CreditValue -= machineOutgo.Value;
                AccountRepository.Update(transportationAccount, transportationAccount.AccountId);
            }

            if (machineOutgo.OutgoType != OutgoTypes.Direct)
                OutgoCategoryRepository.UpdateAmount(machineOutgo.OutgoCategoryId, machineOutgo.Amount * (machineOutgo.OutgoType == OutgoTypes.BuyConsumables ? 1 : -1), machineOutgo.Value * (machineOutgo.OutgoType == OutgoTypes.BuyConsumables ? 1 : -1));

            return machineOutgo;
        }

        public override MachineOutgo Update(MachineOutgo machineOutgo, dynamic machineOutgoId)
        {
            var oldMachineOutgo = Single((int)machineOutgoId);

            if (machineOutgo.OutgoType != OutgoTypes.Direct)
                OutgoCategoryRepository.UpdateAmount(oldMachineOutgo.OutgoCategoryId, oldMachineOutgo.Amount * (oldMachineOutgo.OutgoType == OutgoTypes.BuyConsumables ? -1 : 1), oldMachineOutgo.Value * (oldMachineOutgo.OutgoType == OutgoTypes.BuyConsumables ? -1 : 1));

            base.Update(machineOutgo, machineOutgo.OutgoId);

            if (machineOutgo.OutgoType != OutgoTypes.UseConsumables)
            {
                var relatedCash = CashRepository.Find(c => c.MoneyTransactionType == MoneyTransactionTypes.NonCashMachineOutgo && c.RelatedEntityId == machineOutgo.OutgoId)
                    .Select(c => MapperUtil.Mapper.Map<Cash>(c))
                    .FirstOrDefault();

                if (relatedCash is not null)
                {
                    MapperUtil.Mapper.Map(machineOutgo, relatedCash);
                    CashRepository.Update(relatedCash, relatedCash.CashId);
                }
            }
            else
            {
                var consumableAccount = AccountRepository.Single((short)3);
                consumableAccount.CreditValue += machineOutgo.Value - oldMachineOutgo.Value;
                AccountRepository.Update(consumableAccount, consumableAccount.AccountId);

                var transportationAccount = AccountRepository.Single((short)2);
                transportationAccount.CreditValue -= machineOutgo.Value - oldMachineOutgo.Value;
                AccountRepository.Update(transportationAccount, transportationAccount.AccountId);
            }

            if (machineOutgo.OutgoType != OutgoTypes.Direct)
                OutgoCategoryRepository.UpdateAmount(machineOutgo.OutgoCategoryId, machineOutgo.Amount * (machineOutgo.OutgoType == OutgoTypes.BuyConsumables ? 1 : -1), machineOutgo.Value * (machineOutgo.OutgoType == OutgoTypes.BuyConsumables ? 1 : -1));

            return machineOutgo;
        }

        public override void RemoveRange(IEnumerable<MachineOutgo> machineOutgoes)
        {
            var machineOutgoesIds = machineOutgoes.Where(o => o.OutgoType != OutgoTypes.UseConsumables).Select(b => b.OutgoId).ToList();

            base.RemoveRange(machineOutgoes);

            var relatedCashes = CashRepository.Find(c => c.MoneyTransactionType == MoneyTransactionTypes.NonCashMachineOutgo && machineOutgoesIds.Contains(c.RelatedEntityId));
            CashRepository.RemoveRange(relatedCashes);

            var valueToSubtractFromConsumableAccountAndAddToTransportationAccountCredits = machineOutgoes.Where(o => o.OutgoType == OutgoTypes.UseConsumables)
                .Sum(o => o.Value);

            var consumableAccount = AccountRepository.Single((short)3);
            consumableAccount.CreditValue -= valueToSubtractFromConsumableAccountAndAddToTransportationAccountCredits;
            AccountRepository.Update(consumableAccount, consumableAccount.AccountId);

            var transportationAccount = AccountRepository.Single((short)2);
            transportationAccount.CreditValue += valueToSubtractFromConsumableAccountAndAddToTransportationAccountCredits;
            AccountRepository.Update(transportationAccount, transportationAccount.AccountId);


            machineOutgoes.Where(mo => mo.OutgoType != OutgoTypes.Direct)
                .ToList()
                .ForEach(o => OutgoCategoryRepository.UpdateAmount(o.OutgoCategoryId, o.Amount * (o.OutgoType == OutgoTypes.BuyConsumables ? -1 : 1), o.Value * (o.OutgoType == OutgoTypes.BuyConsumables ? -1 : 1)));
        }
    }
}
