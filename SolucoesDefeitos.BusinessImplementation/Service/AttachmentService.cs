using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Model;

namespace SolucoesDefeitos.BusinessImplementation.Service
{
    public class AttachmentService : BaseService<Attachment>,
        IService<Attachment>,
        IAttachmentService
    {
        public AttachmentService(IRepository<Attachment> repository) : base(repository)
        {
        }
    }
}
