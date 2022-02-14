using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.DataAccess.Database;
using SolucoesDefeitos.DataAccess.EntityDml;
using SolucoesDefeitos.DataAccess.UnitOfWork;
using SolucoesDefeitos.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolucoesDefeitos.DataAccess.Repository
{
    public class AnomalyProductSpecificationRepository :
        BaseRepository<AnomalyProductSpecification, DapperUnitOfWork<SolucaoDefeitoMySqlDatabase>>,
        IRepository<AnomalyProductSpecification>,
        IAnomalyProductSpecificationRepository
    {
        private readonly AnomalyProductSpecificationEntityDml entityDml;

        public AnomalyProductSpecificationRepository(
            DapperUnitOfWork<SolucaoDefeitoMySqlDatabase> unitOfWork) 
            : base(unitOfWork)
        {
            this.entityDml = new AnomalyProductSpecificationEntityDml();
        }

        public async Task DeleteAsync(params int[] anomalyProductSpecificationIds)
        {
            await this.ExecuteRawAsync(this.entityDml.DeleteByAnomalyProductSpecificationIds, new { anomalyProductSpecificationIds });
        }

        public async Task<IEnumerable<int>> GetAnomalyProductSpecificationIdsByAnomalyIdAsync(int anomalyId)
        {
            var anomalyproductSpecifications = await this.QueryRawAsync(this.entityDml.SelectByAnomalyId, new { anomalyId });

            return anomalyproductSpecifications
                .Select(a => a.AnomalyProductSpecificationId);
        }
     }
}
