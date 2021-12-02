using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Service
{
    public interface IService<TModel>
        where TModel: class
    {
        Task BeginTransactionAsync();

        Task RollbackTransactionAsync();

        Task CommitAsync();

        Task<TModel> AddAsync(TModel entity);

        Task UpdateAsync(TModel entity);

        Task DeleteAsync(TModel entity);

        Task<TModel> GetByKeyAsync(object key);

        Task<IEnumerable<TModel>> GetAllAsync();
    }
}
