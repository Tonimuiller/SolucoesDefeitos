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

    public ListViewModel<Model.Anomaly> ViewModel { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 10)
    {
        ViewModel = await _anomalyService.FilterAsync(cancellationToken, pageIndex, pageSize);
        return Page();
    }
}
