using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
                var storedProductSubgroup = await this.AddAsync(createdProductSubgroup);
                createdProductSubgroup.ProductGroupId = storedProductSubgroup.ProductGroupId;
                if (createdProductSubgroup.Subgroups != null)
                {
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
