using SolucoesDefeitos.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Repository
{
    public interface IAnomalyProductSpecificationRepository: IRepository<AnomalyProductSpecification>
    {
        Task<IEnumerable<int>> GetAnomalyProductSpecificationIdsByAnomalyIdAsync(int anomalyId);

        Task DeleteAsync(params int[] anomalyProductSpecificationIds);
    }
}
