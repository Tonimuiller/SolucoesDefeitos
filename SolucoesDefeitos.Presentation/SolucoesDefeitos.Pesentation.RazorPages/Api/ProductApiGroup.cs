using Microsoft.AspNetCore.Mvc;
using SolucoesDefeitos.BusinessDefinition.Service;

namespace SolucoesDefeitos.Pesentation.RazorPages.Api;

public class ProductApiGroup : IApiGroup
{
    public void RegisterGroupMappings(WebApplication webApplication)
    {
        var group = webApplication.MapGroup("/api/product");
        group.MapGet("/by-term", ByTermAsync);
    }

    private async Task<IResult> ByTermAsync(CancellationToken cancellationToken, 
        [FromQuery] string term, 
        IProductService productService)
    {
        if (string.IsNullOrEmpty(term) || string.IsNullOrWhiteSpace(term))
            return Results.BadRequest();

        return Results.Ok(await productService.SearchByTermAsync(cancellationToken, term));
    }
}
