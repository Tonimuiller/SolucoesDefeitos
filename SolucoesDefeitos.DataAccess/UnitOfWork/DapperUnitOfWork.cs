using Dapper;
using SolucoesDefeitos.BusinessDefinition;
using SolucoesDefeitos.DataAccess.Database;
using SolucoesDefeitos.DataAccess.EntityDml;
using SolucoesDefeitos.Model.Contracts;
using SolucoesDefeitos.Provider;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
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
            this.SetEntityCreationDate<T>(entity);
            var database = this.database.DbConnection;
            var insertSqlCommand = this.GetEntityInsertSqlCommand<T>();
            var entityId = await database.ExecuteScalarAsync<int>(insertSqlCommand, entity);
            this.AssignEntityKeyValue<T>(entity, entityId);

            return entity;
        }

        public virtual void BeginTransaction()
        {
            if (dbTransaction != null)
            {
                return;
            }

            if (database.DbConnection.State != ConnectionState.Open)
            {
                database.DbConnection.Open();
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

        public virtual async Task<IEnumerable<T>> GetAllAsync<T>(string sqlCommand, object parameters)
            where T: class
        {
            var connection = database.DbConnection;
            return await connection.QueryAsync<T>(sqlCommand, parameters, dbTransaction);
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
            this.SetEntityUpdateDate<T>(entity);
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

        private void SetEntityCreationDate<TModel>(TModel entity)
            where TModel: class
        {
            if (entity is ICreationDate)
            {
                (entity as ICreationDate).CreationDate = this.dateTimeProvider.CurrentDateTime;
            }
        }

        private string GetEntityInsertSqlCommand<TModel>()
            where TModel:class
        {
            var entityDml = GetEntityDml<TModel>();
            var sqlBuilder = new StringBuilder(entityDml.Insert);
            if (sqlBuilder.ToString().Trim().Last() != ';')
            {
                sqlBuilder.Append(";");
            }

            sqlBuilder.Append(" SELECT LAST_INSERT_ID();");
            return sqlBuilder.ToString();
        }

        private void SetEntityUpdateDate<TModel>(TModel entity)
            where TModel: class
        {
            if (entity is IUpdateDate)
            {
                (entity as IUpdateDate).UpdateDate = this.dateTimeProvider.CurrentDateTime;
            }
        }

        private void AssignEntityKeyValue<TModel>(TModel entity, int keyValue)
            where TModel: class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var entityType = entity.GetType();
            var properties = entityType.GetProperties();
            var keyProperties = properties.Where(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Any());
            if (keyProperties.Count() != 1)
            {
                throw new InvalidOperationException($"Invalid entity '{entityType.Name}' key configuration.");
            }

            var keyProperty = keyProperties.FirstOrDefault();
            keyProperty.SetValue(entity, keyValue);
        }
    }
}
