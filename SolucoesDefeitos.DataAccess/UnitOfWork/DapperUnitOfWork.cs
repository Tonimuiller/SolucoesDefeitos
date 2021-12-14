using Dapper;
using MySql.Data.MySqlClient;
using SolucoesDefeitos.BusinessDefinition;
using SolucoesDefeitos.DataAccess.Database;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model.Contracts;
using SolucoesDefeitos.Provider;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            var insertSqlCommand = this.database.GetEntityInsertSqlCommand<T>();
            var entityId = await database.ExecuteScalarAsync<int>(insertSqlCommand, entity);
            this.AssignEntityKeyValue<T>(entity, entityId);

            return entity;
        }

        public virtual void BeginTransaction() => this.database.BeginTransaction();

        public virtual async Task CommitAsync() => await this.database.CommitAsync();

        public virtual async Task<ResponseDto> DeleteAsync<T>(T entity) where T : class
        {
            var entityDml = this.database.GetEntityDml<T>();
            var _ = await ExecuteAsync(entityDml.Delete, entity);
            return new ResponseDto(true);
        }

        public virtual async Task<T> GetByKeyAsync<T>(object key)
            where T : class
        {
            var connection = database.DbConnection;
            var entityDml = this.database.GetEntityDml<T>();
            return await connection.QueryFirstOrDefaultAsync<T>(entityDml.SelectByKey, key, database.DbTransaction);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync<T>()
            where T : class
        {
            var connection = database.DbConnection;
            var entityDml = this.database.GetEntityDml<T>();
            return await connection.QueryAsync<T>(entityDml.Select, transaction: database.DbTransaction);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync<T>(string sqlCommand, object parameters)
            where T: class
        {
            var connection = database.DbConnection;
            return await connection.QueryAsync<T>(sqlCommand, parameters, database.DbTransaction);
        }

        public virtual void RollbackTransaction() => this.database.Rollback();

        public virtual async Task UpdateAsync<T>(T entity) where T : class
        {
            var entityDml = this.database.GetEntityDml<T>();
            this.SetEntityUpdateDate<T>(entity);
            await ExecuteAsync(entityDml.Update, entity);
        }

        protected TDatabase Database { get => this.database; }

        protected virtual async Task<int> ExecuteAsync(string command, object entity)
        {
            var connection = database.DbConnection;
            return await connection.ExecuteAsync(command, entity, database.DbTransaction);
        }

        private void SetEntityCreationDate<TModel>(TModel entity)
            where TModel: class
        {
            if (entity is ICreationDate)
            {
                (entity as ICreationDate).CreationDate = this.dateTimeProvider.CurrentDateTime;
            }
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
