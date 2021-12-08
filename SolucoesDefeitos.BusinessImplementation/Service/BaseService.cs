using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessImplementation.Service
{
    public abstract class BaseService<TModel> : IService<TModel>
        where TModel : class
    {
        private readonly IRepository<TModel> repository;

        public BaseService(IRepository<TModel> repository)
        {
            this.repository = repository;
        }

        public virtual async Task<ResponseDto<TModel>> AddAsync(TModel entity)
        {
            try
            {
                await this.repository.AddAsync(entity);
                return new ResponseDto<TModel>(true, entity);
            }
            catch(Exception ex)
            {
                return new ResponseDto<TModel>(ex.Message);
            }
        }

        public async Task BeginTransactionAsync()
        {
            await this.repository.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await this.repository.CommitTransactionAsync();
        }

        public virtual async Task DeleteAsync(TModel entity)
        {
            await this.repository.DeleteAsync(entity);
        }

        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            return await this.repository.GetAllAsync();
        }

        public async Task<TModel> GetByKeyAsync(object key)
        {
            return await this.repository.GetByKeyAsync(key);
        }

        public async Task RollbackTransactionAsync()
        {
            await this.repository.RoolbackTransactionAsync();
        }

        public virtual async Task UpdateAsync(TModel entity)
        {
            await this.repository.UpdateAsync(entity);
        }
    }
}
