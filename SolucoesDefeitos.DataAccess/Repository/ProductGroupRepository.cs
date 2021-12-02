using System.Linq;
using System.Threading.Tasks;
using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.DataAccess.Database;
using SolucoesDefeitos.DataAccess.EntityDml;
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

        public async Task LoadSubgroupsAsync(ProductGroup productGroup)
        {
            if (productGroup == null)
            {
                return;
            }

            var entityDml = new ProductGroupEntityDml();
            var subGroups = await this.UnitOfWork.GetAllAsync<ProductGroup>(entityDml.SelectByFatherProductGroupId, new { productGroup.ProductGroupId });
            productGroup.Subgroups = subGroups.ToList();
        }
    }
}
