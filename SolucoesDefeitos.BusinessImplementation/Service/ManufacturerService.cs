using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Model;

namespace SolucoesDefeitos.BusinessImplementation.Service
{
    public class ManufacturerService : BaseService<Manufacturer>,
        IService<Manufacturer>,
        IManufacturerService
    {
        public ManufacturerService(IRepository<Manufacturer> repository) : base(repository)
        {
        }
    }
}
