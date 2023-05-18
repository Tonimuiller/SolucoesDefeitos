using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessImplementation.Service
{
    public class ManufacturerService : BaseService<Manufacturer>,
        IService<Manufacturer>,
        IManufacturerService
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        public ManufacturerService(
            IManufacturerRepository manufacturerRepository
            ) 
            : base(manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<ListViewModel<Manufacturer>> FilterAsync(CancellationToken cancellationToken, string name = null, int page = 1, int pageSize = 10)
        {
            return await _manufacturerRepository.FilterAsync(cancellationToken, name, page, pageSize);
        }
    }
}
