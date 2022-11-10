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
        private readonly IAnomalyProductSpecificationRepository anomalyProductSpecificationRepository;
        private readonly IAttachmentRepository attachmentRepository;

        public AnomalyService(
            IRepository<Anomaly> repository,
            IAnomalyProductSpecificationService anomalyProductSpecificationService,
            IAnomalyProductSpecificationRepository anomalyProductSpecificationRepository,
            IAttachmentRepository attachmentRepository
            ) : base(repository)
        {
            this.anomalyProductSpecificationService = anomalyProductSpecificationService;
            this.anomalyProductSpecificationRepository = anomalyProductSpecificationRepository;
            this.attachmentRepository = attachmentRepository;
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

        public async Task<IEnumerable<Anomaly>> GetAllEagerLoadAsync()
        {
            var anomalies = await this.GetAllAsync();
            foreach(var anomaly in anomalies)
            {
                anomaly.ProductSpecifications = (await this.anomalyProductSpecificationRepository.GetProductsSpecificationsByAnomalyId(anomaly.AnomalyId)).ToList();
                anomaly.Attachments = (await this.attachmentRepository.GetAttachmentsByAnomalyId(anomaly.AnomalyId)).ToList();
            }

            return anomalies;
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
