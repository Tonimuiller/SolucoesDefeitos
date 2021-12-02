using SolucoesDefeitos.Model;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Repository
{
    public interface IProductGroupRepository: IRepository<ProductGroup>
    {
        Task LoadSubgroupsAsync(ProductGroup productGroup);
    }
}
