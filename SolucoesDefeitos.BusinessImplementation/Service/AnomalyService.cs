using SolucoesDefeitos.BusinessDefinition;
using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Dto.Anomaly;
using SolucoesDefeitos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessImplementation.Service
{
    public class AnomalyService : BaseService<Anomaly, int>,
        IAnomalyService
    {
        private readonly IAnomalyProductSpecificationService _anomalyProductSpecificationService;
        private readonly IAnomalyProductSpecificationRepository _anomalyProductSpecificationRepository;
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IAnomalyRepository _anomalyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AnomalyService(
            IAnomalyProductSpecificationService anomalyProductSpecificationService,
            IAnomalyProductSpecificationRepository anomalyProductSpecificationRepository,
            IAttachmentRepository attachmentRepository,
            IAnomalyRepository anomalyRepository,
            IUnitOfWork unitOfWork)
            : base(anomalyRepository)
        {
            _anomalyProductSpecificationService = anomalyProductSpecificationService;
            _anomalyProductSpecificationRepository = anomalyProductSpecificationRepository;
            _attachmentRepository = attachmentRepository;
            _anomalyRepository = anomalyRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task<ResponseDto<Anomaly>> AddAsync(Anomaly entity, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);
                var addAnomalyResponse = await base.AddAsync(entity, cancellationToken);
                if (!addAnomalyResponse.Success)
                {
                    return addAnomalyResponse;
                }

                entity.AnomalyId = addAnomalyResponse.Content.AnomalyId;
                await SaveNewAnomalyProductSpecifications(entity, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);
                return new ResponseDto<Anomaly>(true, entity);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return new ResponseDto<Anomaly>(false, null, ex.Message);
            }
        }

        public override async Task<ResponseDto> UpdateAsync(Anomaly updatedAnomaly, CancellationToken cancellationToken)
        {
            try
            {
                var anomaly = await GetByIdAsync(updatedAnomaly.AnomalyId, cancellationToken);
                if (anomaly == null)
                {
                    return new ResponseDto(false, "Anomaly not found");
                }

                anomaly.Description = updatedAnomaly.Description;
                anomaly.RepairSteps = updatedAnomaly.RepairSteps;
                await _unitOfWork.BeginTransactionAsync(cancellationToken);
                await base.UpdateAsync(anomaly, cancellationToken);
                await _anomalyProductSpecificationService.SaveAnomalyProductSpecifiationsAsync(updatedAnomaly.AnomalyId, updatedAnomaly.ProductSpecifications, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);
                return new UpdateAnomalyResponseDto(true);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return new ResponseDto(false, ex.Message);
            }
        }

        public async Task<IEnumerable<Anomaly>> GetAllEagerLoadAsync(CancellationToken cancellationToken)
        {
            var anomalies = await GetAllAsync(cancellationToken);
            foreach(var anomaly in anomalies)
            {
                anomaly.ProductSpecifications = (await _anomalyProductSpecificationRepository.GetProductsSpecificationsByAnomalyIdAsync(anomaly.AnomalyId, cancellationToken)).ToList();
                anomaly.Attachments = (await _attachmentRepository.GetAttachmentsByAnomalyIdAsync(anomaly.AnomalyId, cancellationToken)).ToList();
            }

            return anomalies;
        }

        public async Task<PagedData<Anomaly>> FilterAsync(CancellationToken cancellationToken, int page = 1, int pageSize = 10)
        {
            return await _anomalyRepository.FilterAsync(cancellationToken, page, pageSize);
        }

        private async Task SaveNewAnomalyProductSpecifications(Anomaly anomaly, CancellationToken cancellationToken)
        {
            var newAnomalyProductSpecifications = anomaly
                .ProductSpecifications?
                .Where(p => p.AnomalyProductSpecificationId == 0)?
                .ToList();

            if (!(newAnomalyProductSpecifications?.Any() ?? false))
            {
                return;
            }

            var anomalyProductsSpecifications = SetAnomalyIdInProductSpecifications(anomaly.AnomalyId, newAnomalyProductSpecifications);
            await _anomalyProductSpecificationService.AddCollectionAsync(anomalyProductsSpecifications, cancellationToken);
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
