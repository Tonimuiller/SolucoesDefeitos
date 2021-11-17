using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.DataAccess.Database;
using SolucoesDefeitos.DataAccess.UnitOfWork;
using SolucoesDefeitos.Model;

namespace SolucoesDefeitos.DataAccess.Repository
{
    public class ProductGroupRepository : 
        BaseRepository<ProductGroup, DapperUnitOfWork<SolucaoDefeitoMySqlDatabase>>,
        IRepository<ProductGroup>,
        IProductGroupRepository
    {
        public ProductGroupRepository(DapperUnitOfWork<SolucaoDefeitoMySqlDatabase> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
