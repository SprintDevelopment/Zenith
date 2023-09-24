using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Dtos;
using Zenith.Data;
using Zenith.Models.ReportModels;
using Zenith.Models.ReportModels.ReportSearchModels;

namespace Zenith.Repositories.ReportRepositories
{
    public class ReportRepository<T> where T : ReportModel, new()
    {
        protected static ApplicationDbContext _context;
        public ReportRepository()
        {
            if (_context == null)
            {
                _context = new DbContextFactory().CreateDbContext(null);
            }
        }

        public virtual IEnumerable<T> Find(BaseDto searchModel) => Enumerable.Empty<T>();
    }
}
