using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessImplementation.Service
{
    public class ProductService : BaseService<Product, int>,
        IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository) 
            : base(repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> SearchByTermAsync(CancellationToken cancellationToken, string term)
        {
            return await _repository.SearchByTermAsync(cancellationToken, term);
        }
    }
}
