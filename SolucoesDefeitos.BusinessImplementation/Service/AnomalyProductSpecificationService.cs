﻿using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessImplementation.Service
{
    public class AnomalyProductSpecificationService : BaseService<AnomalyProductSpecification, int>,
        IAnomalyProductSpecificationService
    {
        private const int NewRecordId = 0;
        private readonly IAnomalyProductSpecificationRepository _anomalyProductSpecificationRepository;
        private readonly IProductRepository _productRepository;

        public AnomalyProductSpecificationService(
            IAnomalyProductSpecificationRepository anomalyProductSpecificationRepository, 
            IProductRepository productRepository)
            : base(anomalyProductSpecificationRepository)
        {
            _anomalyProductSpecificationRepository = anomalyProductSpecificationRepository;
            _productRepository = productRepository;
        }

        public async Task AddCollectionAsync(ICollection<AnomalyProductSpecification> anomalyProductsSpecifications, CancellationToken cancellationToken)
        {
            if (anomalyProductsSpecifications == null)
            {
                throw new ArgumentNullException(nameof(anomalyProductsSpecifications));
            }

            foreach(var anomalyProductSpecification in anomalyProductsSpecifications)
            {
                await AddAsync(anomalyProductSpecification, cancellationToken);
            }
        }

        public async Task SaveAnomalyProductSpecifiationsAsync(int parentAnomalyId, ICollection<AnomalyProductSpecification> anomalyProductSpecifications, CancellationToken cancellationToken)
        {
            var validAnomalyIds = new int[] { NewRecordId, parentAnomalyId };
            if (anomalyProductSpecifications == null 
                || (anomalyProductSpecifications.Any() 
                && anomalyProductSpecifications.Any(s => !validAnomalyIds.Contains(s.AnomalyId))))
            {
                throw new ArgumentException(nameof(anomalyProductSpecifications));
            }

            await UpdateAnomalyProductSpecificationsInAnomaly(anomalyProductSpecifications, cancellationToken);
            await AddNewAnomalyProductSpecificationsInAnomaly(parentAnomalyId, anomalyProductSpecifications, cancellationToken);
            await DeleteAnomalyProductSpecificationsInAnomaly(parentAnomalyId, anomalyProductSpecifications, cancellationToken);
        }
        
        public async Task LoadProductsAsync(IEnumerable<AnomalyProductSpecification> anomalyProductSpecifications, CancellationToken cancellationToken)
        {
            if (anomalyProductSpecifications is null)
            {
                return;
            }

            var productIds = anomalyProductSpecifications.Select(p => p.ProductId);
            var products = await _productRepository.EagerLoadByIdsAsync(productIds, cancellationToken);
            foreach (var anomalyProductSpecification in anomalyProductSpecifications)
            {
                anomalyProductSpecification.Product = products.FirstOrDefault(p => p.ProductId == anomalyProductSpecification.ProductId);
            }
        }

        public async Task<IEnumerable<AnomalyProductSpecification>> GetByAnomalyIdAsync(int anomalyId, CancellationToken cancellationToken)
        {
            return await _anomalyProductSpecificationRepository.GetProductsSpecificationsByAnomalyIdAsync(anomalyId, cancellationToken);
        }

        private async Task UpdateAnomalyProductSpecificationsInAnomaly(ICollection<AnomalyProductSpecification> anomalyProductSpecifications, CancellationToken cancellationToken)
        {
            var updatedAnomalyProductsSpecifications = anomalyProductSpecifications.Where(p => p.AnomalyProductSpecificationId > 0);
            foreach (var updatedAnomalyProductSpecification in updatedAnomalyProductsSpecifications)
            {
                var storedAnomalyProductSpecification = await GetByIdAsync(updatedAnomalyProductSpecification.AnomalyProductSpecificationId, cancellationToken);
                if (storedAnomalyProductSpecification == null)
                {
                    throw new InvalidOperationException("Anomaly product specification not available for update.");
                }

                storedAnomalyProductSpecification.ManufactureYear = updatedAnomalyProductSpecification.ManufactureYear;
                storedAnomalyProductSpecification.ProductId = updatedAnomalyProductSpecification.ProductId;
                await UpdateAsync(storedAnomalyProductSpecification, cancellationToken);
            }
        }

        private async Task AddNewAnomalyProductSpecificationsInAnomaly(int parentAnomalyId, ICollection<AnomalyProductSpecification> anomalyProductSpecifications, CancellationToken cancellationToken)
        {
            var newAnomalyProductsSpecifications = anomalyProductSpecifications
                .Where(p => p.AnomalyProductSpecificationId == NewRecordId)
                .ToList();
            newAnomalyProductsSpecifications.ForEach(newAnomalyProductSpecification => newAnomalyProductSpecification.AnomalyId = parentAnomalyId);
            await AddCollectionAsync(newAnomalyProductsSpecifications, cancellationToken);
        }

        private async Task DeleteAnomalyProductSpecificationsInAnomaly(int parentAnomalyId, ICollection<AnomalyProductSpecification> anomalyProductSpecifications, CancellationToken cancellationToken)
        {
            var anomalyProductSpecificationIds = anomalyProductSpecifications
                .Select(anomalyProductSpecification => anomalyProductSpecification.AnomalyProductSpecificationId);
            var existentAnomalyProductsSpecificationIds = await _anomalyProductSpecificationRepository.GetAnomalyProductSpecificationIdsByAnomalyIdAsync(parentAnomalyId, cancellationToken);
            var deletedAnomalyProductsSpecificationIds = existentAnomalyProductsSpecificationIds
                .Where(anomalyProductSpecificationId => !anomalyProductSpecificationIds.Contains(anomalyProductSpecificationId));
            await _anomalyProductSpecificationRepository.DeleteAsync(cancellationToken, deletedAnomalyProductsSpecificationIds.ToArray());
        }
    }
}
