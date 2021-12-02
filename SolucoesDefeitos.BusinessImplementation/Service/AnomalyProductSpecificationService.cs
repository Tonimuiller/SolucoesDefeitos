using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Model;

namespace SolucoesDefeitos.BusinessImplementation.Service
{
    public class AnomalyProductSpecificationService : BaseService<AnomalyProductSpecification>,
        IService<AnomalyProductSpecification>,
        IAnomalyProductSpecificationService
    {
        public AnomalyProductSpecificationService(IRepository<AnomalyProductSpecification> repository) : base(repository)
        {
        }
    }
}
