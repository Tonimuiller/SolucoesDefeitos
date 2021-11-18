using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.DataAccess.Database;
using SolucoesDefeitos.DataAccess.UnitOfWork;
using SolucoesDefeitos.Model;

namespace SolucoesDefeitos.DataAccess.Repository
{
    public class AttachmentRepository :
        BaseRepository<Attachment, DapperUnitOfWork<SolucaoDefeitoMySqlDatabase>>,
        IRepository<Attachment>,
        IAttachmentRepository
    {
        public AttachmentRepository(DapperUnitOfWork<SolucaoDefeitoMySqlDatabase> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
