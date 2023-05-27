using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Dto.Anomaly;
using SolucoesDefeitos.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Service
{
    public interface IAnomalyService: IService<Anomaly>
    {
        Task<IEnumerable<Anomaly>> GetAllEagerLoadAsync();
        new Task<UpdateAnomalyResponseDto> UpdateAsync(Anomaly updatedAnomaly);

        Task<PagedData<Anomaly>> FilterAsync(CancellationToken cancellationToken, int page = 1, int pageSize = 10);
    }
}
