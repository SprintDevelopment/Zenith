using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using Zenith.Assets.Utils;
using Zenith.Assets.Values.Enums;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class DeliveryRepository : Repository<Delivery>
    {
        MachineRepository MachineRepository = new MachineRepository();
        MachineOutgoRepository MachineOutgoRepository = new MachineOutgoRepository();

        public override Delivery Single(dynamic id)
        {
            long longId = (long)id;
            return _context.Set<Delivery>()
                .Include(d => d.Site)
                .Include(d => d.Machine)
                .Include(d => d.Driver)
                .SingleOrDefault(s => s.DeliveryId == longId);
        }

        public override IEnumerable<Delivery> Find(Expression<Func<Delivery, bool>> predicate) =>
            _context.Set<Delivery>()
                .Include(d => d.Site)
                .Include(d => d.Machine)
                .Include(d => d.Driver)
                .Where(predicate).AsEnumerable();

        public override Delivery Add(Delivery delivery)
        {
            var machine = MachineRepository.Single(delivery.MachineId);

            if (machine.OwnerCompanyId.HasValue)
            {
                var addedMachineOutgo = MapperUtil.Mapper.Map<MachineOutgo>(delivery);
                addedMachineOutgo.CompanyId = machine.OwnerCompanyId;

                MachineOutgoRepository.Add(addedMachineOutgo);

                delivery.RelatedTaxiMachineOutgoId = addedMachineOutgo.OutgoId;
            }

            base.Add(delivery);

            return delivery;
        }

        public override void RemoveRange(IEnumerable<Delivery> deliveries)
        {
            var relatedMachineOutgoesIds = deliveries
                .Where(d => d.RelatedTaxiMachineOutgoId.HasValue)
                .Select(d => d.RelatedTaxiMachineOutgoId.Value)
                .ToList();

            base.RemoveRange(deliveries);

            var relatedMachineOutgoes = _context.Set<MachineOutgo>().Where(mo => relatedMachineOutgoesIds.Contains(mo.OutgoId));
            MachineOutgoRepository.RemoveRange(relatedMachineOutgoes);
        }
    }
}
