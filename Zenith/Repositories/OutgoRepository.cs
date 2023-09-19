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

            CashRepository.Add(MapperUtil.Mapper.Map<Cash>(outgo));

            return outgo;
        }

        public override Outgo Update(Outgo outgo, dynamic outgoId)
        {
            base.Update(outgo, outgo.OutgoId);

            var relatedCash = CashRepository.Find(c => c.MoneyTransactionType == MoneyTransactionTypes.Outgo && c.RelatedEntityId == outgo.OutgoId)
                .Select(c => MapperUtil.Mapper.Map<Cash>(c))
                .FirstOrDefault();

            if (relatedCash is not null)
            {
                MapperUtil.Mapper.Map(outgo, relatedCash);
                CashRepository.Update(relatedCash, relatedCash.CashId);
            }

            return outgo;
        }

        public override void RemoveRange(IEnumerable<Outgo> outgoes)
        {
            var outgoesIds = outgoes.Select(b => b.OutgoId).ToList();

            base.RemoveRange(outgoes);

            var relatedCashes = CashRepository.Find(c => c.MoneyTransactionType == MoneyTransactionTypes.Outgo && outgoesIds.Contains(c.RelatedEntityId));
            CashRepository.RemoveRange(relatedCashes);
        }
    }
}
