using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SolucoesDefeitos.Dto;

namespace SolucoesDefeitos.BusinessDefinition.Service
{
    public interface IService<TModel, TKey>
        where TModel: class
    {

        Task<ResponseDto<TModel>> AddAsync(TModel entity, CancellationToken cancellationToken);

        Task<ResponseDto> UpdateAsync(TModel entity, CancellationToken cancellationToken);

        Task DeleteAsync(TModel entity, CancellationToken cancellationToken);

        Task<TModel> GetByIdAsync(TKey id, CancellationToken cancellationToken);

        Task<IEnumerable<TModel>> GetAllAsync(CancellationToken cancellationToken);
    }
}
