using SolucoesDefeitos.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Repository
{
    public interface IProductRepository: IRepository<Product, int>
    {
        Task<IEnumerable<Product>> SearchByTermAsync(CancellationToken cancellationToken, string term);

        Task<IEnumerable<Product>> EagerLoadByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken);
    }
}
