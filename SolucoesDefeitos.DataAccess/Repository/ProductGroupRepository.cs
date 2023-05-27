using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.DataAccess.Database;
using SolucoesDefeitos.DataAccess.EntityDml;
using SolucoesDefeitos.DataAccess.UnitOfWork;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;
using System.Linq;
using System.Text;
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

        public async Task<PagedData<ProductGroup>> FilterAsync(CancellationToken cancellationToken, string description = null, int page = 1, int pageSize = 20)
        {
            var listViewModel = new PagedData<ProductGroup>();
            listViewModel.CurrentPage = page;
            listViewModel.PageSize = pageSize;
            var skip = (page - 1) * pageSize;
            var totalItemsQueryBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tCOUNT(productgroupid) AS TotalItems")
                .AppendLine("FROM")
            .AppendLine("\tproductGroup");

            if (!string.IsNullOrEmpty(description))
            {
                totalItemsQueryBuilder.AppendLine("WHERE")
                    .AppendLine("\tdescription LIKE @description");
                description = $"{description}%";
            }

            var totalItemsParameters = new
            {
                description
            };

            listViewModel.TotalRecords = await this.QuerySingleRawAsync<int>(totalItemsQueryBuilder.ToString(), totalItemsParameters, cancellationToken);
            while (listViewModel.CurrentPage > 1 && skip >= listViewModel.TotalRecords)
            {
                listViewModel.CurrentPage--;
                skip = (listViewModel.CurrentPage - 1) * pageSize;
            }

            var queryBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tproductgroupid,")
                .AppendLine("\tcreationdate,")
                .AppendLine("\tupdatedate,")
                .AppendLine("\tenabled,")
                .AppendLine("\tdescription")
                .AppendLine("FROM")
            .AppendLine("\tproductgroup");

            if (!string.IsNullOrEmpty(description))
            {
                queryBuilder.AppendLine("WHERE")
                    .AppendLine("\tdescription LIKE @description");
                description = $"{description}%";
            }

            queryBuilder.AppendLine("ORDER BY description");

            queryBuilder.AppendLine("LIMIT @pageSize OFFSET @skip");

            var parameters = new
            {
                description,
                skip,
                pageSize
            };

            listViewModel.Data = await this.QueryRawAsync(queryBuilder.ToString(), parameters, cancellationToken);
            return listViewModel;
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
