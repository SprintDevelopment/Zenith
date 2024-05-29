using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zenith.Assets.Extensions;
using Zenith.Assets.Utils;
using Zenith.Data;
using Zenith.Migrations;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class Repository<T> where T : Model, new()
    {
        public ApplicationDbContext _context;
        public Repository()
        {
            //if (App.Context is null)
                //App.Context = new DbContextFactory().CreateDbContext(null);

            _context = new DbContextFactory().CreateDbContext(null);
        }

        public virtual IEnumerable<T> All() => _context.Set<T>().AsEnumerable();
        public virtual async IAsyncEnumerable<T> AllAsync()
        {
            await foreach (var item in _context.Set<T>().AsAsyncEnumerable())
            {
                yield return item;
            }  
        }
        public virtual IEnumerable<T> AllForSearch()
        {
            var all = _context.Set<T>().AsEnumerable().ToList();
            all.Insert(0, new T());

            return all;
        }
        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => _context.Set<T>().Where(predicate).AsEnumerable();
        public virtual IEnumerable<T> FindForSearch(Expression<Func<T, bool>> predicate)
        {
            var foundItems = _context.Set<T>().Where(predicate).AsEnumerable().ToList();
            foundItems.Insert(0, new T());

            return foundItems;
        }
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
            var old = this.Single(entityId);
            _context.Entry(old).State = EntityState.Detached;
            _context.Entry(entity.LightClone()).State = EntityState.Modified;

            _context.SaveChanges();
            return entity;
        }

        //public virtual void Remove(T entity) { _context.Set<T>().Remove(entity); _context.SaveChanges(); }
        public virtual void AddRange(IEnumerable<T> entities) { _context.Set<T>().AddRange(entities.Select(e => e.LightClone())); _context.SaveChanges(); }
        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            entities.Select(e =>
            {
                var inDbEntity = Single(e.GetKeyPropertyValue());
                _context.Entry(inDbEntity).State = EntityState.Detached;
                _context.Remove(inDbEntity);

                return e;
            }).ToList();

            _context.SaveChanges(); 
        }
    }
}
