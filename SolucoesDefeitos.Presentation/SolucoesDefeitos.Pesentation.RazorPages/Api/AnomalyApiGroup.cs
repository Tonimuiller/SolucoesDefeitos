using SolucoesDefeitos.BusinessDefinition.Service;

namespace SolucoesDefeitos.Pesentation.RazorPages.Api;

public sealed class AnomalyApiGroup : IApiGroup
{
    public void RegisterGroupMappings(WebApplication webApplication)
    {
        var group = webApplication.MapGroup("/api/anomaly");
        group.MapDelete("/{id:int}", DeleteAsync);
    }

    private async Task<IResult> DeleteAsync(
        CancellationToken cancellationToken,
        int id,
        IAnomalyService anomalyService)
    {
        await anomalyService.DeleteAsync(new Model.Anomaly { AnomalyId = id }, cancellationToken);
        return Results.Ok();
    }
}
