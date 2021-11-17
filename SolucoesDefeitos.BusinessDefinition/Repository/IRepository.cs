using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Repository
{
    public interface IRepository<TModel>
        where TModel : class
    {
        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RoolbackTransactionAsync();

        Task<TModel> AddAsync(TModel entity);

        Task UpdateAsync(TModel entity);

        Task DeleteAsync(TModel entity);

        Task<TModel> GetByKeyAsync(object key);

        Task<IEnumerable<TModel>> GetAllAsync();
    }
}
