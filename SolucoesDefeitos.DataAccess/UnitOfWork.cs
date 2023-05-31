using SolucoesDefeitos.BusinessDefinition;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabase _database;

        public UnitOfWork(IDatabase database)
        {
            _database = database;
        }


        public async Task BeginTransactionAsync(CancellationToken cancellationToken)
        {
            if (_database.DbTransaction != null)
            {
                return;
            }

            if (_database.DbConnection.State != ConnectionState.Open)
            {
                _database.DbConnection.Open();
            }

            _database.DbTransaction = await _database.DbConnection.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            if (_database.DbTransaction == null)
            {
                return;
            }

            await _database.DbTransaction.CommitAsync(cancellationToken);
            await ClearTransactionAsync();            
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
        {
            if (_database.DbTransaction == null)
            {
                return;
            }

            await _database.DbTransaction.RollbackAsync(cancellationToken);
            await ClearTransactionAsync();
        }

        protected async Task ClearTransactionAsync()
        {
            if (_database.DbTransaction == null)
            {
                return;
            }

            await _database.DbTransaction.DisposeAsync();
            _database.DbTransaction = null;
        }
    }
}
