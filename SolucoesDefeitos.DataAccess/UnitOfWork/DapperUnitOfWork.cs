using Dapper;
using SolucoesDefeitos.BusinessDefinition;
using SolucoesDefeitos.DataAccess.Database;
using SolucoesDefeitos.DataAccess.EntityDml;
using SolucoesDefeitos.Model.Contracts;
using SolucoesDefeitos.Provider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SolucoesDefeitos.DataAccess.UnitOfWork
{
    public abstract class DapperUnitOfWork<TDatabase> : IUnitOfWork
        where TDatabase : IDatabase
    {
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly TDatabase database;
        private IDbTransaction dbTransaction;

        public DapperUnitOfWork(
            IDateTimeProvider dateTimeProvider,
            TDatabase database
            )
        {
            this.dateTimeProvider = dateTimeProvider;
            this.database = database;
        }

        public virtual async Task<T> AddAsync<T>(T entity) where T : class
        {
            var entityDml = GetEntityDml<T>();
            if (entity is ICreationDate)
            {
                (entity as ICreationDate).CreationDate = this.dateTimeProvider.CurrentDateTime;
            }

            var _ = await ExecuteAsync(entityDml.Insert, entity);
            return entity;
        }

        public virtual void BeginTransaction()
        {
            if (dbTransaction != null)
            {
                return;
            }

            dbTransaction = database.DbConnection.BeginTransaction();
        }

        public virtual Task CommitAsync()
        {
            if (dbTransaction == null)
            {
                return Task.FromResult(0);
            }

            dbTransaction.Commit();
            ClearTransaction();
            return Task.FromResult(0);
        }

        public virtual async Task DeleteAsync<T>(T entity) where T : class
        {
            var entityDml = GetEntityDml<T>();
            var _ = await ExecuteAsync(entityDml.Delete, entity);
        }

        public virtual async Task<T> GetByKeyAsync<T>(object key)
            where T : class
        {
            var connection = database.DbConnection;
            var entityDml = this.GetEntityDml<T>();
            return await connection.QueryFirstOrDefaultAsync<T>(entityDml.SelectByKey, key, dbTransaction);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync<T>()
            where T : class
        {
            var connection = database.DbConnection;
            var entityDml = this.GetEntityDml<T>();
            return await connection.QueryAsync<T>(entityDml.Select, transaction: dbTransaction);
        }

        public virtual void RollbackTransaction()
        {
            if (dbTransaction == null)
            {
                return;
            }

            dbTransaction.Rollback();
            ClearTransaction();
        }

        public virtual async Task UpdateAsync<T>(T entity) where T : class
        {
            var entityDml = GetEntityDml<T>();
            if (entity is IUpdateDate)
            {
                (entity as IUpdateDate).UpdateDate = this.dateTimeProvider.CurrentDateTime;
            }

            await ExecuteAsync(entityDml.Update, entity);
        }

        protected virtual void ClearTransaction()
        {
            if (dbTransaction == null)
            {
                return;
            }

            dbTransaction.Dispose();
            dbTransaction = null;
        }

        protected virtual async Task<int> ExecuteAsync(string command, object entity)
        {
            var connection = database.DbConnection;
            return await connection.ExecuteAsync(command, entity, dbTransaction);
        }

        protected virtual IEntityDml GetEntityDml<TModel>()
            where TModel : class
        {
            var modelType = typeof(TModel);
            var entityDml = this.database.EntitiesDmls
                .Where(e => e.Type == modelType);
            if (!entityDml.Any())
            {
                throw new InvalidOperationException($"Couldn't find entity dml definition for model {modelType.Name}");
            }

            if (entityDml.Count() > 1)
            {
                throw new InvalidOperationException($"More than one entity dml definition for model {modelType.Name}");
            }

            return entityDml.FirstOrDefault();
        }
    }
}
