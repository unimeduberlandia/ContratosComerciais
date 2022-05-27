using Painel.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Painel.Repositories
{
    public interface IRepositoryBase<TEntity> where TEntity : BaseEntity
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(long id);
        void Insert(ref TEntity entity);
        void InsertRange(ref IEnumerable<TEntity> entities);
        bool Update(TEntity entity);
        bool Delete(long id);
        void DeleteRange(ref IEnumerable<long> ids);
        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate);
    }
}
