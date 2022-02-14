using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SolucoesDefeitos.DataAccess.EntityDml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolucoesDefeitos.DataAccess.Database
{
    public class SolucaoDefeitoMySqlDatabase : IDatabase
    {
        private readonly IConfiguration configuration;
        private static IDbConnection dbConnection;
        private static IDbTransaction dbTransaction;

        public SolucaoDefeitoMySqlDatabase(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IEnumerable<IEntityDml> EntitiesDmls
        {
            get
            {
                yield return new ProductEntityDml();
                yield return new ProductGroupEntityDml();
                yield return new ManufacturerEntityDml();
                yield return new AnomalyEntityDml();
                yield return new AnomalyProductSpecificationEntityDml();
            }
        }

        public DataTable GetSchema(string collectionName)
        {
            throw new System.NotImplementedException();
        }

        public void BeginTransaction()
        {
            if (dbTransaction != null)
            {
                return;
            }

            if (this.DbConnection.State != ConnectionState.Open)
            {
                this.DbConnection.Open();
            }

            dbTransaction = this.DbConnection.BeginTransaction();
        }

        public Task CommitAsync()
        {
            if (dbTransaction == null)
            {
                return Task.FromResult(0);
            }

            dbTransaction.Commit();
            ClearTransaction();
            return Task.FromResult(0);
        }

        public void Rollback()
        {
            if (dbTransaction == null)
            {
                return;
            }

            dbTransaction.Rollback();
            ClearTransaction();
        }

        public IDbConnection DbConnection
        {
            get
            {
                if (dbConnection != null && dbConnection.State == ConnectionState.Open)
                {
                    return dbConnection;
                }

                dbConnection = new MySqlConnection(configuration.GetConnectionString("database"));
                return dbConnection;
            }
        }

        public IDbTransaction DbTransaction => dbTransaction;

        public int ForeignKeyRelationshipViolationErrorCode => 1451;

        public virtual IEntityDml GetEntityDml<TModel>()
            where TModel : class
        {
            var modelType = typeof(TModel);
            var entityDml = this.EntitiesDmls
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
        
        public string GetEntityInsertSqlCommand<TModel>()
            where TModel : class
        {
            var entityDml = this.GetEntityDml<TModel>();
            var sqlBuilder = new StringBuilder(entityDml.Insert);
            if (sqlBuilder.ToString().Trim().Last() != ';')
            {
                sqlBuilder.Append(";");
            }

            sqlBuilder.Append(" SELECT LAST_INSERT_ID();");
            return sqlBuilder.ToString();
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
    }
}
