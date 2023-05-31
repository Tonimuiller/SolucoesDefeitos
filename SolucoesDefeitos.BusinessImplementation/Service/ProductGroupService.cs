using SolucoesDefeitos.BusinessDefinition;
using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessImplementation.Service
{
    public class ProductGroupService : BaseService<ProductGroup, int>,
        IProductGroupService
    {
        private readonly IProductGroupRepository _productGroupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductGroupService(
            IProductGroupRepository productGroupRepository, 
            IUnitOfWork unitOfWork)
            : base(productGroupRepository)
        {
            _productGroupRepository = productGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task<ResponseDto<ProductGroup>> AddAsync(ProductGroup newProductGroup, CancellationToken cancellationToken)
        {
            if (newProductGroup == null)
            {
                throw new ArgumentNullException(nameof(newProductGroup));
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);
                await base.AddAsync(newProductGroup, cancellationToken);
                if (newProductGroup.Subgroups != null && newProductGroup.Subgroups.Any())
                {
                    FillSubGroupsFatherProductGroupId(newProductGroup.ProductGroupId, newProductGroup.Subgroups);
                    await ProcessProductSubgroupCreationAsync(newProductGroup.Subgroups, cancellationToken);
                }

                await _unitOfWork.CommitAsync(cancellationToken);
                return new ResponseDto<ProductGroup>(true, newProductGroup);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return new ResponseDto<ProductGroup>(ex.Message);
            }
        }

        public override async Task<ResponseDto> UpdateAsync(ProductGroup updatedProductGroup, CancellationToken cancellationToken)
        {
            if (updatedProductGroup == null)
            {
                throw new ArgumentNullException(nameof(updatedProductGroup));
            }

            var storedProductGroup = await GetByIdAsync(updatedProductGroup.ProductGroupId, cancellationToken);
            if (storedProductGroup == null)
            {
                throw new ArgumentException("Não foi possível encontrar o grupo de produto");
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);
                await UpdateProductGroupPropertiesAsync(storedProductGroup, updatedProductGroup, cancellationToken);
                if (updatedProductGroup.Subgroups != null)
                {
                    await UpdateProductGroupSubgroupsAsync(storedProductGroup, updatedProductGroup, cancellationToken);
                }

                await _unitOfWork.CommitAsync(cancellationToken);
                return new ResponseDto(true);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return new ResponseDto(false, $"Ocorreu um erro ao atualizar o grupo de produtos: {ex.Message}");
            }
        }

        public async Task<PagedData<ProductGroup>> FilterAsync(CancellationToken cancellationToken, string description = null, int page = 1, int pageSize = 10)
        {
            return await _productGroupRepository.FilterAsync(cancellationToken, description, page, pageSize);
        }

        private void FillSubGroupsFatherProductGroupId(int FatherProductGroupId, ICollection<ProductGroup> subGroups)
        {
            foreach(var subGroup in subGroups)
            {
                subGroup.FatherProductGroupId = FatherProductGroupId;
            }
        }

        private async Task UpdateProductGroupPropertiesAsync(
            ProductGroup storedProductGroup,
            ProductGroup updatedProductGroup,
            CancellationToken cancellationToken)
        {
            storedProductGroup.Description = updatedProductGroup.Description;
            storedProductGroup.Enabled = updatedProductGroup.Enabled;
            storedProductGroup.FatherProductGroupId = updatedProductGroup.FatherProductGroupId;
            await base.UpdateAsync(storedProductGroup, cancellationToken);
        }

        private async Task UpdateProductGroupSubgroupsAsync(
            ProductGroup storedProductGroup,
            ProductGroup updatedProductGroup,
            CancellationToken cancellationToken)
        {
            await _productGroupRepository.LoadSubgroupsAsync(storedProductGroup, cancellationToken);
            await ProcessProductSubgroupUpdatesAsync(
                storedProductGroup.Subgroups,
                updatedProductGroup.Subgroups.Where(s => s.ProductGroupId > 0),
                cancellationToken);

            var currentProductSubgroupIds = updatedProductGroup.Subgroups
                .Select(s => s.ProductGroupId);
            var deletedProductSubgroups = storedProductGroup.Subgroups
                .Where(s => !currentProductSubgroupIds.Contains(s.ProductGroupId));
            await ProcessProductSubgroupDeletionsAsync(deletedProductSubgroups, cancellationToken);

            await ProcessProductSubgroupCreationAsync(updatedProductGroup.Subgroups.Where(s => s.ProductGroupId == 0), cancellationToken);
        }

        private async Task ProcessProductSubgroupCreationAsync(
            IEnumerable<ProductGroup> createdProductSubgroups, 
            CancellationToken cancellationToken)
        {
            foreach(var createdProductSubgroup in createdProductSubgroups)
            {
                await base.AddAsync(createdProductSubgroup, cancellationToken);
                if (createdProductSubgroup.Subgroups != null)
                {
                    FillSubGroupsFatherProductGroupId(createdProductSubgroup.ProductGroupId, createdProductSubgroup.Subgroups);
                    await ProcessProductSubgroupCreationAsync(createdProductSubgroup.Subgroups, cancellationToken);
                }
            }
        }

        private async Task ProcessProductSubgroupUpdatesAsync(
            ICollection<ProductGroup> storedProductSubgroups,
            IEnumerable<ProductGroup> updatedProductSubgroups,
            CancellationToken cancellationToken)
        {
            foreach(var updatedSubgroup in updatedProductSubgroups)
            {
                var storedSubgroup = storedProductSubgroups.FirstOrDefault(s => s.ProductGroupId == updatedSubgroup.ProductGroupId);
                if (storedSubgroup != null)
                {
                    await UpdateProductGroupPropertiesAsync(storedSubgroup, updatedSubgroup, cancellationToken);
                    if (updatedSubgroup.Subgroups != null)
                    {
                        await UpdateProductGroupSubgroupsAsync(storedSubgroup, updatedSubgroup, cancellationToken);
                    }
                }
            }
        }

        private async Task ProcessProductSubgroupDeletionsAsync(
            IEnumerable<ProductGroup> deletedProductSubgroups,
            CancellationToken cancellationToken)
        {
            foreach(var deletedProductSubGroup in deletedProductSubgroups)
            {
                if (deletedProductSubGroup.Subgroups != null)
                {
                    await ProcessProductSubgroupDeletionsAsync(deletedProductSubGroup.Subgroups, cancellationToken);
                }

                await DeleteAsync(deletedProductSubGroup, cancellationToken);
            }
        }
    }
}
