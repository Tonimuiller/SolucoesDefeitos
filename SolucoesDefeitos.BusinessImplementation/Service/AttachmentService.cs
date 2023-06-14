using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessImplementation.Service
{
    public class AttachmentService : BaseService<Attachment, int>, IAttachmentService
    {
        private readonly IAttachmentRepository _attachmentRepository;
        public AttachmentService(
            IAttachmentRepository repository, 
            IAttachmentRepository attachmentRepository)
            : base(repository)
        {
            _attachmentRepository = attachmentRepository;
        }

        public async Task<IEnumerable<Attachment>> GetByAnomalyIdAsync(int anomalyId, CancellationToken cancellationToken)
        {
            return await _attachmentRepository.GetAttachmentsByAnomalyIdAsync(anomalyId, cancellationToken);
        }
    }
}
