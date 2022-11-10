using SolucoesDefeitos.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Repository
{
    public interface IAttachmentRepository: IRepository<Attachment>
    {
        Task<IEnumerable<Attachment>> GetAttachmentsByAnomalyId(int anomalyId);
    }
}
