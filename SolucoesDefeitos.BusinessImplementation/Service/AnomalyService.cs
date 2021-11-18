using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Model;

namespace SolucoesDefeitos.BusinessImplementation.Service
{
    public class AnomalyService : BaseService<Anomaly>,
        IService<Anomaly>,
        IAnomalyService
    {
        public AnomalyService(IRepository<Anomaly> repository) : base(repository)
        {
        }
    }
}
