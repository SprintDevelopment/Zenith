using Microsoft.EntityFrameworkCore;
using System.Linq;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class PersonRepository : Repository<Person>
    {
        public override Person Single(dynamic id)
        {
            int intId = (int)id;
            return _context.Set<Person>()
                .Include(s => s.PersonnelAbsences)
                .SingleOrDefault(s => s.PersonId == intId);
        }
    }
}
