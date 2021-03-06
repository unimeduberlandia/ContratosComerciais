using Dommel;
using Painel.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace Painel.Repositories
{
    public abstract class SqlRepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : BaseEntity
    {
        protected readonly string ConnectionString;

        protected SqlRepositoryBase(string conn)
        {
            ConnectionString = ConfigurationManager.ConnectionStrings[conn].ConnectionString;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                return db.GetAll<TEntity>();
            }
        }

        public virtual TEntity GetById(long id)
        {
            var tabela = typeof(TEntity).Name;

            using (var db = new SqlConnection(ConnectionString))
            {
                return db.Get<TEntity>(id);
            }
        }

        public virtual void Insert(ref TEntity entity)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var id = (int)db.Insert(entity);
                entity = GetById(id);
            }
        }

        public virtual bool Update(TEntity entity)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                return db.Update(entity);
            }
        }

        public virtual bool Delete(long id)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var entity = GetById(id);
                if (entity == null) throw new Exception("Registro não encontrado");
                return db.Delete(entity);
            }
        }

        public virtual IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                return db.Select(predicate);
            }
        }

        public void InsertRange(ref IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange(ref IEnumerable<long> ids)
        {
            throw new NotImplementedException();
        }
    }        
}
