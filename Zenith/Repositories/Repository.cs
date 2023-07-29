using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Extensions;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class Repository<T> where T : Model
    {
        public IEnumerable<T> Get(T selector = null)
        {
            return Enumerable.Empty<T>();
        }

        public int GetCount(string searchCriteria)
        {
            return 1;
        }

        public virtual T GetSingle(dynamic key, T selector = null)
        {
            return null;
        }

        public IEnumerable<T> Find(string searchCriteria, T selector = null)
        {
            return Enumerable.Empty<T>();
        }

        public virtual bool Insert(T modelToInsert, bool isKeyPropertyCare = false)
        {
            return false;
        }

        public virtual bool Update(T modelToUpdate)
        {
            return false;
        }

        public int Delete(IEnumerable<T> itemsToDelete)
        {
            return 1;
        }
    }
}
