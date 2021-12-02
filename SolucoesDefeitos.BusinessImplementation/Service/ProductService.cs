using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Model;

namespace SolucoesDefeitos.BusinessImplementation.Service
{
    public class ProductService : BaseService<Product>,
        IService<Product>,
        IProductService
    {
        public ProductService(IRepository<Product> repository) : base(repository)
        {
        }
    }
}
