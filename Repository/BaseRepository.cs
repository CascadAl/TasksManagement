using Domain;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Repository
{
    public abstract class BaseRepository<TEntity>: IDisposable, IRepository<TEntity> where TEntity: class, IEntity
    {
        private ApplicationDbContext _context = null;
        private IDbSet<TEntity> _entities = null;

        public BaseRepository(ApplicationDbContext context)
        {
            this._context = context;
            this._entities = _context.Set<TEntity>();
        }


        public int Add(TEntity entity)
        {
            if (entity==null) throw new ArgumentNullException(NameOf.nameof(()=> entity));

            _entities.Add(entity);
            _context.SaveChanges();

            return entity.Id; 
        }

        public IQueryable<TEntity> Get()
        {
            return _entities;
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.Where(predicate).AsQueryable();
        }

        public TEntity Get(int id)
        {
            if (id < 1) throw new ArgumentOutOfRangeException(NameOf.nameof(() => id));

            return _entities.FirstOrDefault(e=>e.Id==id);
        }

        public bool Has(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.Any(predicate);
        }

        public bool Has(int id)
        {
            if (id < 1) throw new ArgumentOutOfRangeException(NameOf.nameof(() => id));

            return _entities.Any(e => e.Id == id);
        }

        public void Remove(TEntity entity)
        {
            if (entity == null) return;

            _entities.Remove(entity);
            _context.SaveChanges();
        }

        public void Remove(int id)
        {
            if (id < 1) throw new ArgumentOutOfRangeException(NameOf.nameof(()=>id));

            var entity = this.Get(id);
            if (entity == null) return;

            _entities.Remove(entity);
            _context.SaveChanges();
        }

        public void RemoveAll()
        {
            // нужно проверить
            _context.Set<TEntity>().RemoveRange(_entities);
            _context.SaveChanges();
        }

        public bool Update(TEntity entity)
        {
            if (entity == null) return false;

            // проверить id!=0
            if (entity.Id == 0) return false;
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }

        public virtual void Dispose()
        {
            _context.Dispose();
        }
    }

    public class NameOf
    {
        public static String nameof<T>(Expression<Func<T>> name)
        {
            MemberExpression expressionBody = (MemberExpression)name.Body;
            return expressionBody.Member.Name;
        }
    }
    
}
