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
    public class ProductGroupService : BaseService<ProductGroup>,
        IService<ProductGroup>,
        IProductGroupService
    {
        private readonly IProductGroupRepository productGroupRepository;

        public ProductGroupService(IProductGroupRepository productGroupRepository) 
            : base(productGroupRepository)
        {
            this.productGroupRepository = productGroupRepository;
        }

        new public async Task<ResponseDto<ProductGroup>> AddAsync(ProductGroup newProductGroup)
        {
            if (newProductGroup == null)
            {
                throw new ArgumentNullException(nameof(newProductGroup));
            }

            try
            {
                await this.BeginTransactionAsync();
                await base.AddAsync(newProductGroup);
                if (newProductGroup.Subgroups != null && newProductGroup.Subgroups.Any())
                {
                    this.FillSubGroupsFatherProductGroupId(newProductGroup.ProductGroupId, newProductGroup.Subgroups);
                    await this.ProcessProductSubgroupCreationAsync(newProductGroup.Subgroups);
                }

                await this.CommitAsync();
                return new ResponseDto<ProductGroup>(true, newProductGroup);
            }
            catch (Exception ex)
            {
                await this.RollbackTransactionAsync();
                return new ResponseDto<ProductGroup>(ex.Message);
            }
        }

        new public async Task<ResponseDto> UpdateAsync(ProductGroup updatedProductGroup)
        {
            if (updatedProductGroup == null)
            {
                throw new ArgumentNullException(nameof(updatedProductGroup));
            }

            var storedProductGroup = await this.GetByKeyAsync(new { updatedProductGroup.ProductGroupId });
            if (storedProductGroup == null)
            {
                throw new ArgumentException("Não foi possível encontrar o grupo de produto");
            }

            try
            {
                await this.BeginTransactionAsync();
                await this.UpdateProductGroupPropertiesAsync(storedProductGroup, updatedProductGroup);
                if (updatedProductGroup.Subgroups != null)
                {
                    await this.UpdateProductGroupSubgroupsAsync(storedProductGroup, updatedProductGroup);
                }

                await this.CommitAsync();
                return new ResponseDto(true);
            }
            catch (Exception ex)
            {
                await this.RollbackTransactionAsync();
                return new ResponseDto(false, $"Ocorreu um erro ao atualizar o grupo de produtos: {ex.Message}");
            }
        }

        public async Task<PagedData<ProductGroup>> FilterAsync(CancellationToken cancellationToken, string description = null, int page = 1, int pageSize = 10)
        {
            return await productGroupRepository.FilterAsync(cancellationToken, description, page, pageSize);
        }

        private void FillSubGroupsFatherProductGroupId(int FatherProductGroupId, ICollection<ProductGroup> subGroups)
        {
            foreach(var subGroup in subGroups)
            {
                subGroup.FatherProductGroupId = FatherProductGroupId;
            }
        }

        private async Task UpdateProductGroupPropertiesAsync(ProductGroup storedProductGroup,
            ProductGroup updatedProductGroup)
        {
            storedProductGroup.Description = updatedProductGroup.Description;
            storedProductGroup.Enabled = updatedProductGroup.Enabled;
            storedProductGroup.FatherProductGroupId = updatedProductGroup.FatherProductGroupId;
            await base.UpdateAsync(storedProductGroup);
        }

        private async Task UpdateProductGroupSubgroupsAsync(ProductGroup storedProductGroup,
            ProductGroup updatedProductGroup)
        {
            await this.productGroupRepository.LoadSubgroupsAsync(storedProductGroup);
            await this.ProcessProductSubgroupUpdatesAsync(
                storedProductGroup.Subgroups,
                updatedProductGroup.Subgroups.Where(s => s.ProductGroupId > 0));

            var currentProductSubgroupIds = updatedProductGroup.Subgroups
                .Select(s => s.ProductGroupId);
            var deletedProductSubgroups = storedProductGroup.Subgroups
                .Where(s => !currentProductSubgroupIds.Contains(s.ProductGroupId));
            await this.ProcessProductSubgroupDeletionsAsync(deletedProductSubgroups);

            await this.ProcessProductSubgroupCreationAsync(updatedProductGroup.Subgroups.Where(s => s.ProductGroupId == 0));
        }

        private async Task ProcessProductSubgroupCreationAsync(IEnumerable<ProductGroup> createdProductSubgroups)
        {
            foreach(var createdProductSubgroup in createdProductSubgroups)
            {
                await base.AddAsync(createdProductSubgroup);
                if (createdProductSubgroup.Subgroups != null)
                {
                    this.FillSubGroupsFatherProductGroupId(createdProductSubgroup.ProductGroupId, createdProductSubgroup.Subgroups);
                    await this.ProcessProductSubgroupCreationAsync(createdProductSubgroup.Subgroups);
                }
            }
        }

        private async Task ProcessProductSubgroupUpdatesAsync(ICollection<ProductGroup> storedProductSubgroups,
            IEnumerable<ProductGroup> updatedProductSubgroups)
        {
            foreach(var updatedSubgroup in updatedProductSubgroups)
            {
                var storedSubgroup = storedProductSubgroups.FirstOrDefault(s => s.ProductGroupId == updatedSubgroup.ProductGroupId);
                if (storedSubgroup != null)
                {
                    await this.UpdateProductGroupPropertiesAsync(storedSubgroup, updatedSubgroup);
                    if (updatedSubgroup.Subgroups != null)
                    {
                        await this.UpdateProductGroupSubgroupsAsync(storedSubgroup, updatedSubgroup);
                    }
                }
            }
        }

        private async Task ProcessProductSubgroupDeletionsAsync(IEnumerable<ProductGroup> deletedProductSubgroups)
        {
            foreach(var deletedProductSubGroup in deletedProductSubgroups)
            {
                if (deletedProductSubGroup.Subgroups != null)
                {
                    await this.ProcessProductSubgroupDeletionsAsync(deletedProductSubGroup.Subgroups);
                }

                await this.DeleteAsync(deletedProductSubGroup);
            }
        }
    }
}
