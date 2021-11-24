using SolucoesDefeitos.BusinessDefinition;
using SolucoesDefeitos.DataAccess.Database;
using SolucoesDefeitos.Provider;

namespace SolucoesDefeitos.DataAccess.UnitOfWork
{
    public class SolucaoDefeitoUnitOfWork : DapperUnitOfWork<SolucaoDefeitoMySqlDatabase>,
        IUnitOfWork
    {
        public SolucaoDefeitoUnitOfWork(
            IDateTimeProvider dateTimeProvider,
            SolucaoDefeitoMySqlDatabase database
            ) : base(dateTimeProvider, database)
        {
        }
    }
}
