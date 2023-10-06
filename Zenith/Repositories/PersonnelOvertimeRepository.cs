using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class PersonnelOvertimeRepository : Repository<PersonnelOvertime>
    {
        public override PersonnelOvertime Single(dynamic id)
        {
            int intId = (int)id;
            return _context.Set<PersonnelOvertime>()
                .Include(pa => pa.Person)
                .SingleOrDefault(s => s.PersonnelOvertimeId == intId);
        }
    }
}
