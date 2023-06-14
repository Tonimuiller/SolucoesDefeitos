using SolucoesDefeitos.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Repository
{
    public interface IAttachmentRepository: IRepository<Attachment, int>
    {
        Task<IEnumerable<Attachment>> GetAttachmentsByAnomalyIdAsync(int anomalyId, CancellationToken cancellationToken);

        Task DeleteAsync(int anomalyId, int[] attachmentIds, CancellationToken cancellationToken);
    }
}
