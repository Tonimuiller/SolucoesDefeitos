using SolucoesDefeitos.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Service
{
    public interface IAttachmentService: IService<Attachment, int>
    {
        Task<IEnumerable<Attachment>> GetByAnomalyIdAsync(int anomalyId, CancellationToken cancellationToken);
    }
}
