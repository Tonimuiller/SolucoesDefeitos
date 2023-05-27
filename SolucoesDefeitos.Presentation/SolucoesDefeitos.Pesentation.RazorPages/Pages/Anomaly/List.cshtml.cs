using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Dto;

namespace SolucoesDefeitos.Pesentation.RazorPages.Pages.Anomaly;

public sealed class ListModel : PageModel
{
    private readonly IAnomalyService _anomalyService;

    public ListModel(IAnomalyService anomalyService)
    {
        ArgumentNullException.ThrowIfNull(anomalyService);
        _anomalyService = anomalyService;
    }

    public PagedData<Model.Anomaly> PagedData { get; set; } = new PagedData<Model.Anomaly>();

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 10)
    {
        PagedData = await _anomalyService.FilterAsync(cancellationToken, pageIndex, pageSize);
        return Page();
    }
}
