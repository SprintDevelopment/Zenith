using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using Zenith.Assets.Utils;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class DeliveryRepository : Repository<Delivery>
    {
        MachineRepository MachineRepository = new MachineRepository();
        MachineOutgoRepository MachineOutgoRepository = new MachineOutgoRepository();
        ConfigurationRepository ConfigurationRepository = new ConfigurationRepository();

        //public override IEnumerable<Delivery> All() =>
        //    _context.Set<Delivery>()
        //        .Include(d => d.SaleItem).ThenInclude(si => si.Material)
        //        .Include(d => d.Site).ThenInclude(s => s.Company)
        //        .Include(d => d.Machine)
        //        .Include(d => d.Driver).AsEnumerable();

        public override async IAsyncEnumerable<Delivery> AllAsync()
        {
            var asyncEnumerable = _context.Set<Delivery>()
                .Include(d => d.SaleItem).ThenInclude(si => si.Material)
                .Include(d => d.Site).ThenInclude(s => s.Company)
                .Include(d => d.Machine)
                .Include(d => d.Driver)
                .OrderByDescending(s => s.DateTime)
                .AsSplitQuery()
                .AsAsyncEnumerable();

            await foreach (var item in asyncEnumerable)
            {
                yield return item;
            }
        }

        public override Delivery Single(dynamic id)
        {
            long longId = (long)id;
            return _context.Set<Delivery>()
                .Include(d => d.SaleItem).ThenInclude(si => si.Sale)
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

            if(delivery.AutoDeliveryNumberEnabled)
            {
                ConfigurationRepository.AddOrUpdateRange(new List<Configuration>
                    {
                        new Configuration
                        {
                            Key = $"{ConfigurationKeys.LastAutoDeliveryNumber}",
                            Value = $"{delivery.DeliveryNumber}"
                        }
                    });
            }

            base.Add(delivery);

            return delivery;
        }

        public override Delivery Update(Delivery delivery, dynamic deliveryId)
        {
            var relatedMachineOutgoes = _context.Set<MachineOutgo>().Where(mo => mo.OutgoId == delivery.RelatedTaxiMachineOutgoId).ToList();
            MachineOutgoRepository.RemoveRange(relatedMachineOutgoes);

            var machine = MachineRepository.Single(delivery.MachineId);

            if (machine.OwnerCompanyId.HasValue)
            {
                var addedMachineOutgo = MapperUtil.Mapper.Map<MachineOutgo>(delivery);
                addedMachineOutgo.CompanyId = machine.OwnerCompanyId;

                MachineOutgoRepository.Add(addedMachineOutgo);

                delivery.RelatedTaxiMachineOutgoId = addedMachineOutgo.OutgoId;
            }

            base.Update(delivery, delivery.DeliveryId);

            return delivery;
        }

        public override void RemoveRange(IEnumerable<Delivery> deliveries)
        {
            var relatedMachineOutgoesIds = deliveries
                .Where(d => d.RelatedTaxiMachineOutgoId.HasValue)
                .Select(d => d.RelatedTaxiMachineOutgoId.Value)
                .ToList();

            var relatedMachineOutgoes = _context.Set<MachineOutgo>().Where(mo => relatedMachineOutgoesIds.Contains(mo.OutgoId)).ToList();
            MachineOutgoRepository.RemoveRange(relatedMachineOutgoes);

            base.RemoveRange(deliveries);
        }
    }
}
