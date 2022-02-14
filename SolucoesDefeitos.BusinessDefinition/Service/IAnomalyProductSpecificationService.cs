using SolucoesDefeitos.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Service
{
    public interface IAnomalyProductSpecificationService : IService<AnomalyProductSpecification>
    {
        Task AddCollectionAsync(ICollection<AnomalyProductSpecification> anomalyProductsSpecifications);
        
        Task SaveAnomalyProductSpecifiationsAsync(int parentAnomalyId, ICollection<AnomalyProductSpecification> anomalyProductSpecifications);
    }
}
