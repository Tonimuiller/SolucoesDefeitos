using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.DataAccess.Database;
using SolucoesDefeitos.DataAccess.EntityDml;
using SolucoesDefeitos.DataAccess.UnitOfWork;
using SolucoesDefeitos.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.DataAccess.Repository
{
    public class AttachmentRepository :
        BaseRepository<Attachment, DapperUnitOfWork<SolucaoDefeitoMySqlDatabase>>,
        IRepository<Attachment>,
        IAttachmentRepository
    {
        private readonly AttachmentEntityDml entityDml;

        public AttachmentRepository(DapperUnitOfWork<SolucaoDefeitoMySqlDatabase> unitOfWork) : base(unitOfWork)
        {
            this.entityDml = new AttachmentEntityDml();
        }

        public async Task<IEnumerable<Attachment>> GetAttachmentsByAnomalyId(int anomalyId)
        {
            return await this.QueryRawAsync(this.entityDml.SelectByAnomalyId, new { anomalyId }, CancellationToken.None);
        }
    }
}
