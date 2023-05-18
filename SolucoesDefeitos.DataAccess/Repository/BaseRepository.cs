using SolucoesDefeitos.BusinessDefinition;
using SolucoesDefeitos.BusinessDefinition.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.DataAccess.Repository
{
    public abstract class BaseRepository<TModel, TUnitOfWork> : IRepository<TModel>
        where TModel : class
        where TUnitOfWork: IUnitOfWork
    {
        private readonly TUnitOfWork unitOfWork;

        public BaseRepository(TUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<TModel> AddAsync(TModel entity)
        {
            return await this.unitOfWork.AddAsync(entity);
        }

        public Task BeginTransactionAsync()
        {
            this.unitOfWork.BeginTransaction();
            return Task.FromResult(0);
        }

        public async Task CommitTransactionAsync()
        {
            await this.unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(TModel entity)
        {
            await this.unitOfWork.DeleteAsync(entity);
        }

        public async Task<TModel> GetByKeyAsync(object key)
        {
            return await this.unitOfWork.GetByKeyAsync<TModel>(key);
        }

        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            return await this.unitOfWork.GetAllAsync<TModel>();
        }

        public Task RoolbackTransactionAsync()
        {
            this.unitOfWork.RollbackTransaction();
            return Task.FromResult(0);
        }

        public async Task UpdateAsync(TModel entity)
        {
            await this.unitOfWork.UpdateAsync(entity);
        }

        protected async Task<IEnumerable<TModel>> QueryRawAsync(string query, object parameters, CancellationToken cancellationToken)
        {
            return await this.unitOfWork.QueryRawAsync<TModel>(query, parameters, cancellationToken);
        }

        protected async Task<TResult> QuerySingleRawAsync<TResult>(string query, object parameters, CancellationToken cancellationToken)
        {
            return await this.unitOfWork.QuerySingleRawAsync<TResult>(query, parameters, cancellationToken);
        }

        protected async Task<int> ExecuteRawAsync(string command, object entity)
        {
            return await this.unitOfWork.ExecuteRawAsync(command, entity);
        }
    }
}
