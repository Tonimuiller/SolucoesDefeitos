using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.DataAccess.Database;
using SolucoesDefeitos.DataAccess.EntityDml;
using SolucoesDefeitos.DataAccess.UnitOfWork;
using SolucoesDefeitos.Model;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.DataAccess.Repository
{
    public class ProductGroupRepository : 
        BaseRepository<ProductGroup, DapperUnitOfWork<SolucaoDefeitoMySqlDatabase>>,
        IRepository<ProductGroup>,
        IProductGroupRepository
    {
        private readonly ProductGroupEntityDml entityDml;

        public ProductGroupRepository(DapperUnitOfWork<SolucaoDefeitoMySqlDatabase> unitOfWork) : base(unitOfWork)
        {
            this.entityDml = new ProductGroupEntityDml();
        }

        public async Task LoadSubgroupsAsync(ProductGroup productGroup)
        {
            if (productGroup == null)
            {
                return;
            }

            var subGroups = await this.QueryRawAsync(this.entityDml.SelectByFatherProductGroupId, new { productGroup.ProductGroupId }, CancellationToken.None);
            productGroup.Subgroups = subGroups.ToList();
        }
    }
}
