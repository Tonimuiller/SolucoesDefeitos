using Microsoft.AspNetCore.Mvc;
using SolucoesDefeitos.BusinessDefinition.Service;

namespace SolucoesDefeitos.Pesentation.RazorPages.Api;

public class ProductApiGroup : IApiGroup
{
    public void RegisterGroupMappings(WebApplication webApplication)
    {
        var group = webApplication.MapGroup("/api/product");
        group.MapGet("/by-term", ByTermAsync);
        group.MapDelete("/{id:int}", DeleteAsync);
    }

    private async Task<IResult> ByTermAsync(CancellationToken cancellationToken, 
        [FromQuery] string term, 
        IProductService productService)
    {
        if (string.IsNullOrEmpty(term) || string.IsNullOrWhiteSpace(term))
            return Results.BadRequest();

        return Results.Ok(await productService.SearchByTermAsync(cancellationToken, term));
    }

    private async Task<IResult> DeleteAsync(int id, 
        CancellationToken cancellationToken,
        IProductService productService)
    {
        var result = await productService.DeleteAsync(new Model.Product { ProductId = id }, cancellationToken);
        if (!result.Success)
        {
            return Results.Conflict(result);
        }

        return Results.Ok();
    }
}
