using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Repository
{
    public interface IProductGroupRepository: IRepository<ProductGroup, int>
    {
        Task LoadSubgroupsAsync(ProductGroup productGroup, CancellationToken cancellationToken);

        Task<PagedData<ProductGroup>> FilterAsync(CancellationToken cancellationToken, string description = null, int page = 1, int pageSize = 20);

        Task<IEnumerable<ProductGroup>> GetAllEnabledDescriptionOrderedAsync(CancellationToken cancellationToken);
    }
}
