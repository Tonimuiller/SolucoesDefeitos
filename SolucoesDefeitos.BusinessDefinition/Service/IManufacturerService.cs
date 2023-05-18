using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Service
{
    public interface IManufacturerService: IService<Manufacturer>
    {
        Task<ListViewModel<Manufacturer>> FilterAsync(CancellationToken cancellationToken, string name = null, int page = 1, int pageSize = 10);
    }
}
