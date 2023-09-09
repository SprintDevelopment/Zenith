using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class PersonnelAbsenceRepository : Repository<PersonnelAbsence>
    {
        public override PersonnelAbsence Single(dynamic id)
        {
            int intId = (int)id;
            return _context.Set<PersonnelAbsence>()
                .Include(pa => pa.Person)
                .SingleOrDefault(s => s.PersonnelAbsenceId == intId);
        }
    }
}
