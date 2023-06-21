using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Service
{
    public interface IProductService: IService<Product, int>
    {
        Task<IEnumerable<Product>> SearchByTermAsync(CancellationToken cancellationToken, string term);

        Task<PagedData<Product>> FilterAsync(int page, int pageSize, CancellationToken cancellationToken);
    }
}
