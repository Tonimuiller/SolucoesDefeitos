using SolucoesDefeitos.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Service
{
    public interface IAnomalyProductSpecificationService : IService<AnomalyProductSpecification, int>
    {
        Task AddCollectionAsync(ICollection<AnomalyProductSpecification> anomalyProductsSpecifications, CancellationToken cancellationToken);
        
        Task SaveAnomalyProductSpecifiationsAsync(int parentAnomalyId, ICollection<AnomalyProductSpecification> anomalyProductSpecifications, CancellationToken cancellationToken);

        Task LoadProductsAsync(IEnumerable<AnomalyProductSpecification> anomalyProductSpecifications, CancellationToken cancellationToken);
    }
}
