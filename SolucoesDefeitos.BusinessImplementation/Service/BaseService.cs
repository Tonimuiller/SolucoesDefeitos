using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Dto;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessImplementation.Service
{
    public abstract class BaseService<TModel, keyType> : IService<TModel, keyType>
        where TModel : class
    {
        private readonly IRepository<TModel, keyType> _repository;

        public BaseService(IRepository<TModel, keyType> repository)
        {
            this._repository = repository;
        }

        public virtual async Task<ResponseDto<TModel>> AddAsync(TModel entity, CancellationToken cancellationToken)
        {
            try
            {
                await _repository.AddAsync(entity, cancellationToken);
                return new ResponseDto<TModel>(true, entity);
            }
            catch(Exception ex)
            {
                return new ResponseDto<TModel>(ex.Message);
            }
        }

        public virtual async Task<ResponseDto> DeleteAsync(TModel entity, CancellationToken cancellationToken)
        {
            return await _repository.DeleteAsync(entity, cancellationToken);
        }

        public async Task<IEnumerable<TModel>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync(cancellationToken);
        }

        public async Task<TModel> GetByIdAsync(keyType id, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(id, cancellationToken);
        }

        public virtual async Task<ResponseDto> UpdateAsync(TModel entity, CancellationToken cancellationToken)
        {
            await _repository.UpdateAsync(entity, cancellationToken);
            return new ResponseDto(true);
        }
    }
}
