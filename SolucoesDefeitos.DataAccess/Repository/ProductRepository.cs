using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.DataAccess.Database;
using SolucoesDefeitos.DataAccess.UnitOfWork;
using SolucoesDefeitos.Model;

namespace SolucoesDefeitos.DataAccess.Repository
{
    public class ProductRepository : 
        BaseRepository<Product, DapperUnitOfWork<SolucaoDefeitoMySqlDatabase>>,
        IRepository<Product>,
        IProductRepository
    {
        public ProductRepository(DapperUnitOfWork<SolucaoDefeitoMySqlDatabase> unitOfWork) 
            : base(unitOfWork)
        {
        }
    }
}
