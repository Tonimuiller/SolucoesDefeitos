﻿using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Repository
{
    public interface IManufacturerRepository: IRepository<Manufacturer, int>
    {
        Task<PagedData<Manufacturer>> FilterAsync(CancellationToken cancellationToken, string name = null, int page = 1, int pageSize = 20);

        Task<IEnumerable<Manufacturer>> GetAllEnabledNameOrderedAsync(CancellationToken cancellationToken);
    }
}
