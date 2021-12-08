using System.Threading.Tasks;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;

namespace SolucoesDefeitos.BusinessDefinition.Service
{
    public interface IProductGroupService : IService<ProductGroup>
    {
        new Task<ResponseDto<ProductGroup>> AddAsync(ProductGroup newProductGroup);
        new Task<ResponseDto> UpdateAsync(ProductGroup updatedProductGroup);
    }
}
