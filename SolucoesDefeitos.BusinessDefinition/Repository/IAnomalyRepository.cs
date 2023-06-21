using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Dto.Anomaly.Request;
using SolucoesDefeitos.Model;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Repository
{
    public interface IAnomalyRepository: IRepository<Anomaly, int>
    {
        Task<PagedData<Anomaly>> FilterAsync(AnomalyFilterRequest request, CancellationToken cancellationToken);
    }
}
