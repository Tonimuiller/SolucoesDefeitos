using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Repository
{
    public interface IProductGroupRepository: IRepository<ProductGroup>
    {
        Task LoadSubgroupsAsync(ProductGroup productGroup);

        Task<PagedData<ProductGroup>> FilterAsync(CancellationToken cancellationToken, string description = null, int page = 1, int pageSize = 20);
    }
}
