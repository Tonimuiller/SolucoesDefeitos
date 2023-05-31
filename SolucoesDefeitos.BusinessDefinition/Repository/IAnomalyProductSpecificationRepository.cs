using SolucoesDefeitos.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Repository
{
    public interface IAnomalyProductSpecificationRepository: IRepository<AnomalyProductSpecification, int>
    {
        Task<IEnumerable<AnomalyProductSpecification>> GetProductsSpecificationsByAnomalyIdAsync(int anomalyId, CancellationToken cancellationToken);
        Task<IEnumerable<int>> GetAnomalyProductSpecificationIdsByAnomalyIdAsync(int anomalyId, CancellationToken cancellationToken);
        Task DeleteAsync(CancellationToken cancellationToken, params int[] anomalyProductSpecificationIds);
    }
}
