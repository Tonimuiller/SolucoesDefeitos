using SolucoesDefeitos.Dto.Anomaly;
using SolucoesDefeitos.Model;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Service
{
    public interface IAnomalyService: IService<Anomaly>
    {
        new Task<UpdateAnomalyResponseDto> UpdateAsync(Anomaly updatedAnomaly);
    }
}
