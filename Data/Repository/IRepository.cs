using System;
using System.Linq;
using System.Linq.Expressions;
using Data;

namespace Data.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class, IEntity
    {
        int Add(TEntity entity);

        bool Update(TEntity entity);

        void Remove(int id);

        void Remove(TEntity entity);

        void RemoveAll();


        TEntity Get(int id);

        IQueryable<TEntity> Get();

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

        bool Has(int id);

        bool Has(Expression<Func<TEntity, bool>> predicate);
    }
}
