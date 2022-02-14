using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessImplementation.Service
{
    public class AnomalyProductSpecificationService : BaseService<AnomalyProductSpecification>,
        IService<AnomalyProductSpecification>,
        IAnomalyProductSpecificationService
    {
        private const int NewRecordId = 0;
        private readonly IAnomalyProductSpecificationRepository anomalyProductSpecificationRepository;

        public AnomalyProductSpecificationService(IAnomalyProductSpecificationRepository anomalyProductSpecificationRepository) 
            : base(anomalyProductSpecificationRepository)
        {
            this.anomalyProductSpecificationRepository = anomalyProductSpecificationRepository;
        }

        public async Task AddCollectionAsync(ICollection<AnomalyProductSpecification> anomalyProductsSpecifications)
        {
            if (anomalyProductsSpecifications == null)
            {
                throw new ArgumentNullException(nameof(anomalyProductsSpecifications));
            }

            foreach(var anomalyProductSpecification in anomalyProductsSpecifications)
            {
                await this.AddAsync(anomalyProductSpecification);
            }
        }

        public async Task SaveAnomalyProductSpecifiationsAsync(int parentAnomalyId, ICollection<AnomalyProductSpecification> anomalyProductSpecifications)
        {
            var validAnomalyIds = new int[] { NewRecordId, parentAnomalyId };
            if (anomalyProductSpecifications == null 
                || (anomalyProductSpecifications.Any() 
                && anomalyProductSpecifications.Any(s => !validAnomalyIds.Contains(s.AnomalyId))))
            {
                throw new ArgumentException(nameof(anomalyProductSpecifications));
            }

            var updatedAnomalyProductsSpecifications = anomalyProductSpecifications.Where(p => p.AnomalyProductSpecificationId > 0);
            var newAnomalyProductsSpecifications = anomalyProductSpecifications.Where(p => p.AnomalyProductSpecificationId == NewRecordId);
            var existentAnomalyProductsSpecificationIds = await this.anomalyProductSpecificationRepository.GetAnomalyProductSpecificationIdsByAnomalyIdAsync(parentAnomalyId);
            foreach(var updatedAnomalyProductSpecification in updatedAnomalyProductsSpecifications)
            {
                var storedAnomalyProductSpecification = await this.GetByKeyAsync(updatedAnomalyProductSpecification);
                if (storedAnomalyProductSpecification == null)
                {
                    throw new InvalidOperationException("Anomaly product specification not available for update.");
                }

                storedAnomalyProductSpecification.ManufactureYear = updatedAnomalyProductSpecification.ManufactureYear;
                storedAnomalyProductSpecification.ProductId = updatedAnomalyProductSpecification.ProductId;
                await this.UpdateAsync(storedAnomalyProductSpecification);
            }

            foreach(var newAnomalyProductSpecification in newAnomalyProductsSpecifications)
            {
                newAnomalyProductSpecification.AnomalyId = parentAnomalyId;
            }

            await this.AddCollectionAsync(newAnomalyProductsSpecifications.ToList());

            var anomalyProductSpecificationIds = anomalyProductSpecifications
                .Select(anomalyProductSpecification => anomalyProductSpecification.AnomalyProductSpecificationId);

            var deletedAnomalyProductsSpecificationIds = existentAnomalyProductsSpecificationIds
                .Where(anomalyProductSpecificationId => !anomalyProductSpecificationIds.Contains(anomalyProductSpecificationId));
            await this.anomalyProductSpecificationRepository.DeleteAsync(deletedAnomalyProductsSpecificationIds.ToArray());
        }
    }
}
