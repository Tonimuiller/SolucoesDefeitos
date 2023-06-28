using SolucoesDefeitos.BusinessDefinition.Service;

namespace SolucoesDefeitos.Pesentation.RazorPages.Api;

public class ManufacturerApiGroup : IApiGroup
{
    public void RegisterGroupMappings(WebApplication webApplication)
    {
        var group = webApplication.MapGroup("/api/manufacturer")
            .RequireAuthorization();
        group.MapDelete("/{id:int}", DeleteAsync);
    }

    private async Task<IResult> DeleteAsync(
        CancellationToken cancellationToken,
        int id,
        IManufacturerService manufacturerService)
    {
        await manufacturerService.DeleteAsync(new Model.Manufacturer { ManufacturerId = id }, cancellationToken);
        return Results.Ok();
    }
}
