using SolucoesDefeitos.Dto.Anomaly;
using SolucoesDefeitos.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Service
{
    public interface IAnomalyService: IService<Anomaly>
    {
        Task<IEnumerable<Anomaly>> GetAllEagerLoadAsync();
        new Task<UpdateAnomalyResponseDto> UpdateAsync(Anomaly updatedAnomaly);
    }
}
