using SolucoesDefeitos.BusinessDefinition;
using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Dto.Anomaly;
using SolucoesDefeitos.Dto.Anomaly.Request;
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
                await SaveNewAnomalyAttachments(entity, cancellationToken);
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
                anomaly.Summary = updatedAnomaly.Summary;
                await _unitOfWork.BeginTransactionAsync(cancellationToken);
                await base.UpdateAsync(anomaly, cancellationToken);
                await _anomalyProductSpecificationService.SaveAnomalyProductSpecifiationsAsync(updatedAnomaly.AnomalyId, updatedAnomaly.ProductSpecifications, cancellationToken);
                await SaveUpdatedAnomalyAttachmentsAsync(updatedAnomaly, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);
                return new UpdateAnomalyResponseDto(true);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return new ResponseDto(false, ex.Message);
            }
        }

        public override async Task<ResponseDto> DeleteAsync(Anomaly entity, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);
                await _anomalyProductSpecificationRepository.DeleteByAnomalyIdAsync(entity.AnomalyId, cancellationToken);
                await _attachmentRepository.DeleteAnomalyAttachmentsAsync(entity.AnomalyId, cancellationToken);
                var deleteResponse = await base.DeleteAsync(entity, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);
                return deleteResponse;
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

        public async Task<PagedData<Anomaly>> FilterAsync(AnomalyFilterRequest request,CancellationToken cancellationToken)
        {
            return await _anomalyRepository.FilterAsync(request, cancellationToken);
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

        private async Task SaveNewAnomalyAttachments(Anomaly anomaly, CancellationToken cancellationToken)
        {
            var newAnomalyAttachments = anomaly
                .Attachments?
                .Where(a => a.AttachmentId == default)
                .ToList();

            if (!(newAnomalyAttachments?.Any() ?? false))
            {
                return;
            }

            var attachments = SetAttachmentsAnomalyId(anomaly.AnomalyId, newAnomalyAttachments);
            foreach(var attachment in attachments)
            {
                await _attachmentRepository.AddAsync(attachment, cancellationToken);
            }
        }

        private ICollection<Attachment> SetAttachmentsAnomalyId(int anomalyId, ICollection<Attachment> attachments)
        {
            foreach(var attachment in attachments)
            {
                attachment.AnomalyId = anomalyId;
            }

            return attachments;
        }

        private async Task SaveUpdatedAnomalyAttachmentsAsync(Anomaly anomaly, CancellationToken cancellationToken)
        {
            await SaveDeletedAttachmentsAsync(anomaly, cancellationToken);
            await SaveNewAnomalyAttachments(anomaly, cancellationToken);
            await SaveUpdatedAttachmentsAsync(anomaly, cancellationToken);
        }

        private async Task SaveUpdatedAttachmentsAsync(Anomaly anomaly, CancellationToken cancellationToken)
        {
            var updatedAttachments = anomaly
                .Attachments?
                .Where(a => a.AttachmentId > 0)
                .ToList();
            if (!(updatedAttachments?.Any() ?? false))
            {
                return;
            }

            foreach (var attachment in updatedAttachments)
            {
                await _attachmentRepository.UpdateAsync(attachment, cancellationToken);
            }
        }

        private async Task SaveDeletedAttachmentsAsync(Anomaly anomaly, CancellationToken cancellationToken)
        {
            var attachmentIds = anomaly
                .Attachments?
                .Select(a => a.AttachmentId)
                .ToArray();
            await _attachmentRepository.DeleteAsync(anomaly.AnomalyId, attachmentIds, cancellationToken);
        }
    }
}
