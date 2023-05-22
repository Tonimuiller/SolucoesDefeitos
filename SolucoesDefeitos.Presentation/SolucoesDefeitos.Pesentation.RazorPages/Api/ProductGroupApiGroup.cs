using SolucoesDefeitos.BusinessDefinition.Service;

namespace SolucoesDefeitos.Pesentation.RazorPages.Api;

public sealed class ProductGroupApiGroup : IApiGroup
{
    public void RegisterGroupMappings(WebApplication webApplication)
    {
        var group = webApplication.MapGroup("/api/product-group");
        group.MapDelete("/{id:int}", DeleteAsync);
    }

    private async Task<IResult> DeleteAsync(
        CancellationToken cancellationToken,
        int id,
        IProductGroupService productGroupService)
    {
        await productGroupService.DeleteAsync(new Model.ProductGroup { ProductGroupId = id });
        return Results.Ok();
    }
}
