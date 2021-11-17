﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        
        void RollbackTransaction();

        Task CommitAsync();

        Task<T> AddAsync<T>(T entity) where T : class;

        Task UpdateAsync<T>(T entity) where T : class;

        Task DeleteAsync<T>(T entity) where T : class;

        Task<T> GetByKeyAsync<T>(object key)
            where T : class;

        Task<IEnumerable<T>> GetAllAsync<T>()
            where T : class;
    }
}