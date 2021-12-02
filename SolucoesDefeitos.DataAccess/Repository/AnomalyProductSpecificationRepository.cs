using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.DataAccess.Database;
using SolucoesDefeitos.DataAccess.UnitOfWork;
using SolucoesDefeitos.Model;

namespace SolucoesDefeitos.DataAccess.Repository
{
    public class AnomalyProductSpecificationRepository :
        BaseRepository<AnomalyProductSpecification, DapperUnitOfWork<SolucaoDefeitoMySqlDatabase>>,
        IRepository<AnomalyProductSpecification>,
        IAnomalyProductSpecificationRepository
    {
        public AnomalyProductSpecificationRepository(DapperUnitOfWork<SolucaoDefeitoMySqlDatabase> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
