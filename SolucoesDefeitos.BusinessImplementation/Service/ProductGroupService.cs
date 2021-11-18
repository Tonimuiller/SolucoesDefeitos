using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Model;

namespace SolucoesDefeitos.BusinessImplementation.Service
{
    public class ProductGroupService : BaseService<ProductGroup>,
        IService<ProductGroup>,
        IProductGroupService
    {
        public ProductGroupService(IRepository<ProductGroup> repository) : base(repository)
        {
        }
    }
}
