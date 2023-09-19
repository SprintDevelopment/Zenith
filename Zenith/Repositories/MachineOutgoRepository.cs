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

            CashRepository.Add(MapperUtil.Mapper.Map<Cash>(machineOutgo));

            return machineOutgo;
        }

        public override MachineOutgo Update(MachineOutgo machineOutgo, dynamic machineOutgoId)
        {
            base.Update(machineOutgo, machineOutgo.OutgoId);

            var relatedCash = CashRepository.Find(c => c.MoneyTransactionType == MoneyTransactionTypes.MachineOutgo && c.RelatedEntityId == machineOutgo.OutgoId)
                .Select(c => MapperUtil.Mapper.Map<Cash>(c))
                .FirstOrDefault();

            if (relatedCash is not null)
            {
                MapperUtil.Mapper.Map(machineOutgo, relatedCash);
                CashRepository.Update(relatedCash, relatedCash.CashId);
            }

            return machineOutgo;
        }

        public override void RemoveRange(IEnumerable<MachineOutgo> machineOutgoes)
        {
            var machineOutgoesIds = machineOutgoes.Select(b => b.OutgoId).ToList();

            base.RemoveRange(machineOutgoes);

            var relatedCashes = CashRepository.Find(c => c.MoneyTransactionType == MoneyTransactionTypes.MachineOutgo && machineOutgoesIds.Contains(c.RelatedEntityId));
            CashRepository.RemoveRange(relatedCashes);
        }
    }
}
