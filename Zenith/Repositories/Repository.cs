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
using Zenith.Data;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class Repository<T> where T : Model
    {
        protected static DbContext _context;
        public Repository()
        {
            if (_context == null)
                _context = new DbContextFactory().CreateDbContext(null);
        }

        public IEnumerable<T> All() => _context.Set<T>().AsEnumerable();
        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => _context.Set<T>().Where(predicate).AsEnumerable();
        public T Single(dynamic id) => _context.Set<T>().Find(id);

        public T Add(T entity) => _context.Set<T>().Add(entity).Entity;
        public T Update(T entity, dynamic entityId)
        {
            var old = Single(entityId);
            _context.Entry(old).State = EntityState.Detached;
            _context.Entry(entity).State = EntityState.Modified;

            return entity;
        }
        public void Remove(T entity) => _context.Set<T>().Remove(entity);

        public void AddRange(IEnumerable<T> entities) => _context.Set<T>().AddRange(entities);
        public void RemoveRange(IEnumerable<T> entities) => _context.Set<T>().RemoveRange(entities);

        public int SaveChanges() => _context.SaveChanges();
    }
}
