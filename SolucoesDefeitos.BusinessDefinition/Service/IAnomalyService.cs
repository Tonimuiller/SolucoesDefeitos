using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Dto.Anomaly.Request;
using SolucoesDefeitos.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Service
{
    public interface IAnomalyService: IService<Anomaly, int>
    {
        Task<IEnumerable<Anomaly>> GetAllEagerLoadAsync(CancellationToken cancellationToken);

        Task<PagedData<Anomaly>> FilterAsync(AnomalyFilterRequest request, CancellationToken cancellationToken);
    }
}
