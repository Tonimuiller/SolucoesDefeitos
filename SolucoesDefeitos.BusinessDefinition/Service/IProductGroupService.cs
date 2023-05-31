using System.Threading;
using System.Threading.Tasks;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;

namespace SolucoesDefeitos.BusinessDefinition.Service
{
    public interface IProductGroupService : IService<ProductGroup, int>
    {
        Task<PagedData<ProductGroup>> FilterAsync(CancellationToken cancellationToken, string description = null, int page = 1, int pageSize = 10);
    }
}
