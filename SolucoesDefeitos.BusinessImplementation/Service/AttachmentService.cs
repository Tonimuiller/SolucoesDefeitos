using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Model;

namespace SolucoesDefeitos.BusinessImplementation.Service
{
    public class AttachmentService : BaseService<Attachment, int>, IAttachmentService
    {
        public AttachmentService(IAttachmentRepository repository) : base(repository)
        {
        }
    }
}
