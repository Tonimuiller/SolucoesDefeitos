using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Dto.Anomaly;
using SolucoesDefeitos.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessImplementation.Service
{
    public class AnomalyService : BaseService<Anomaly>,
        IService<Anomaly>,
        IAnomalyService
    {
        private readonly IAnomalyProductSpecificationService anomalyProductSpecificationService;

        public AnomalyService(
            IRepository<Anomaly> repository,
            IAnomalyProductSpecificationService anomalyProductSpecificationService) : base(repository)
        {
            this.anomalyProductSpecificationService = anomalyProductSpecificationService;
        }

        public override async Task<ResponseDto<Anomaly>> AddAsync(Anomaly entity)
        {
            try
            {
                await this.BeginTransactionAsync();
                var addAnomalyResponse = await base.AddAsync(entity);
                if (!addAnomalyResponse.Success)
                {
                    return addAnomalyResponse;
                }

                entity.AnomalyId = addAnomalyResponse.Content.AnomalyId;
                await this.SaveNewAnomalyProductSpecifications(entity);
                await this.CommitAsync();
                return new ResponseDto<Anomaly>(true, entity);
            }
            catch
            {
                await this.RollbackTransactionAsync();
                throw;
            }
        }

        new public async Task<UpdateAnomalyResponseDto> UpdateAsync(Anomaly updatedAnomaly)
        {
            try
            {
                var anomaly = await this.GetByKeyAsync(updatedAnomaly);
                if (anomaly == null)
                {
                    return new UpdateAnomalyResponseDto(false, true, "Anomaly not found");
                }

                anomaly.Description = updatedAnomaly.Description;
                anomaly.RepairSteps = updatedAnomaly.RepairSteps;
                await this.BeginTransactionAsync();
                await base.UpdateAsync(anomaly);
                await this.anomalyProductSpecificationService.SaveAnomalyProductSpecifiationsAsync(updatedAnomaly.AnomalyId, updatedAnomaly.ProductSpecifications);
                await this.CommitAsync();
                return new UpdateAnomalyResponseDto(true);
            }
            catch
            {
                await this.RollbackTransactionAsync();
                throw;
            }
        }

        private async Task SaveNewAnomalyProductSpecifications(Anomaly anomaly)
        {
            var newAnomalyProductSpecifications = anomaly
                .ProductSpecifications?
                .Where(p => p.AnomalyProductSpecificationId == 0)?
                .ToList();

            if (!(newAnomalyProductSpecifications?.Any() ?? false))
            {
                return;
            }

            var anomalyProductsSpecifications = this.SetAnomalyIdInProductSpecifications(anomaly.AnomalyId, newAnomalyProductSpecifications);
            await this.anomalyProductSpecificationService.AddCollectionAsync(anomalyProductsSpecifications);
        }

        private ICollection<AnomalyProductSpecification> SetAnomalyIdInProductSpecifications(int anomalyId, ICollection<AnomalyProductSpecification> productSpecifications)
        {
            foreach(var productSpecification in productSpecifications)
            {
                productSpecification.AnomalyId = anomalyId;
            }

            return productSpecifications;
        }
    }
}
