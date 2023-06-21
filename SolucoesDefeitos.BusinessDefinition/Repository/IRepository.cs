using SolucoesDefeitos.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Repository
{
    public interface IRepository<TModel, TKey>
        where TModel : class
    {
        Task<TModel> AddAsync(TModel entity, CancellationToken cancellationToken);

        Task UpdateAsync(TModel entity, CancellationToken cancellationToken);

        Task<ResponseDto> DeleteAsync(TModel entity, CancellationToken cancellationToken);

        Task<TModel> GetByIdAsync(TKey keyValue, CancellationToken cancellationToken);

        Task<IEnumerable<TModel>> GetAllAsync(CancellationToken cancellationToken);
    }
}
