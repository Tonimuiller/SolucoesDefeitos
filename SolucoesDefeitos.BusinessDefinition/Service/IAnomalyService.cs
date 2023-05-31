using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Service
{
    public interface IAnomalyService: IService<Anomaly, int>
    {
        Task<IEnumerable<Anomaly>> GetAllEagerLoadAsync(CancellationToken cancellationToken);

        Task<PagedData<Anomaly>> FilterAsync(CancellationToken cancellationToken, int page = 1, int pageSize = 10);
    }
}
