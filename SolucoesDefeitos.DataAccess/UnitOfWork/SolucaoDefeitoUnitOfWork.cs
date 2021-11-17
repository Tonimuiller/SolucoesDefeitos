using SolucoesDefeitos.BusinessDefinition;
using SolucoesDefeitos.DataAccess.Database;

namespace SolucoesDefeitos.DataAccess.UnitOfWork
{
    public class SolucaoDefeitoUnitOfWork : DapperUnitOfWork<SolucaoDefeitoMySqlDatabase>,
        IUnitOfWork
    {
        public SolucaoDefeitoUnitOfWork(SolucaoDefeitoMySqlDatabase database) : base(database)
        {
        }
    }
}
