using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Repository
{
    public interface IAnomalyRepository: IRepository<Anomaly>
    {
        Task<ListViewModel<Anomaly>> FilterAsync(CancellationToken cancellationToken, int page = 1, int pageSize = 20);
    }
}
