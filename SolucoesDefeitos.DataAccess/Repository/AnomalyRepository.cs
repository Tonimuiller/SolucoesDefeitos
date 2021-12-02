using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.DataAccess.Database;
using SolucoesDefeitos.DataAccess.UnitOfWork;
using SolucoesDefeitos.Model;

namespace SolucoesDefeitos.DataAccess.Repository
{
    public class AnomalyRepository :
        BaseRepository<Anomaly, DapperUnitOfWork<SolucaoDefeitoMySqlDatabase>>,
        IRepository<Anomaly>,
        IAnomalyRepository
    {
        public AnomalyRepository(DapperUnitOfWork<SolucaoDefeitoMySqlDatabase> unitOfWork) 
            : base(unitOfWork)
        {
        }
    }
}
