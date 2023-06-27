using Microsoft.AspNetCore.Mvc;
using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;

namespace SolucoesDefeitos.Pesentation.RazorPages.Api;

public sealed class ProductGroupApiGroup : IApiGroup
{
    public void RegisterGroupMappings(WebApplication webApplication)
    {
        var group = webApplication.MapGroup("/api/product-group");
        group.MapDelete("/{id:int}", DeleteAsync);
        group.MapGet("/anomaly-filter-options", AnomalyFilterOptionsAsync);
    }

    private async Task<IResult> DeleteAsync(
        int id,
        IProductGroupService productGroupService,
        CancellationToken cancellationToken)
    {
        await productGroupService.DeleteAsync(new Model.ProductGroup { ProductGroupId = id }, cancellationToken);
        return Results.Ok();
    }

    private async Task<IResult> AnomalyFilterOptionsAsync(
        [FromQuery] int[] manufacturerIds,
        IProductGroupRepository productGroupRepository,
        CancellationToken cancellationToken)
    {
        var productGroups = await productGroupRepository.GetAllEnabledByManufacturerIdsDescriptionOrderedAsync(manufacturerIds, cancellationToken);
        return Results.Ok(productGroups);
    }
}
