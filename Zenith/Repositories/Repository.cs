using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zenith.Assets.Extensions;
using Zenith.Assets.Utils;
using Zenith.Data;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class Repository<T> where T : Model, new()
    {
        protected static ApplicationDbContext _context;
        public Repository()
        {
            if (_context == null)
                _context = new DbContextFactory().CreateDbContext(null);
        }

        public virtual IEnumerable<T> All() => _context.Set<T>().AsEnumerable();
        public virtual IEnumerable<T> AllForSearch()
        {
            var all = _context.Set<T>().AsEnumerable().ToList();
            all.Insert(0, new T());

            return all;
        }
        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => _context.Set<T>().Where(predicate).AsEnumerable();
        public virtual T Single(dynamic id) => _context.Set<T>().Find(id);

        public virtual T Add(T entity)
        {
            var lightClone = entity.LightClone();

            _context.Set<T>().Add(lightClone);
            _context.SaveChanges();

            var keyProperty = entity.GetKeyProperty();
            keyProperty.SetValue(entity, keyProperty.GetValue(lightClone));

            return entity;
        }

        public virtual T Update(T entity, dynamic entityId)
        {
            var old = Single(entityId);
            _context.Entry(old).State = EntityState.Detached;
            _context.Entry(entity.LightClone()).State = EntityState.Modified;

            _context.SaveChanges();
            return entity;
        }

        public virtual void Remove(T entity) { _context.Set<T>().Remove(entity); _context.SaveChanges(); }
        public virtual void AddRange(IEnumerable<T> entities) { _context.Set<T>().AddRange(entities.Select(e => e.LightClone())); _context.SaveChanges(); }
        public virtual void RemoveRange(IEnumerable<T> entities) { _context.Set<T>().RemoveRange(entities); _context.SaveChanges(); }
    }
}
