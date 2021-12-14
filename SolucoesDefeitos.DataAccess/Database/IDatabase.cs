using SolucoesDefeitos.DataAccess.EntityDml;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SolucoesDefeitos.DataAccess.Database
{
    public interface IDatabase
    {
        IEnumerable<IEntityDml> EntitiesDmls { get; }

        DataTable GetSchema(string collectionName);

        void BeginTransaction();

        Task CommitAsync();

        void Rollback();
        IEntityDml GetEntityDml<TModel>() where TModel : class;
        string GetEntityInsertSqlCommand<TModel>() where TModel : class;

        public IDbConnection DbConnection { get; }

        public IDbTransaction DbTransaction { get; }

        public int ForeignKeyRelationshipViolationErrorCode { get; }
    }
}
