using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SolucoesDefeitos.BusinessDefinition.Service;

namespace SolucoesDefeitos.Pesentation.RazorPages.Pages.Anomaly;

public sealed class ViewModel : PageModel
{
    private readonly IAnomalyService _anomalyService;

    [BindProperty]
    public Model.Anomaly Anomaly { get; set; } = default!;

    public ViewModel(IAnomalyService anomalyService)
    {
        _anomalyService = anomalyService;
    }

    public async Task<IActionResult> OnGetAsync(int? anomalyId, CancellationToken cancellationToken)
    {
        if (anomalyId == null)
        {
            return Redirect("/List");
        }

        Anomaly = await _anomalyService.GetByIdAsync(anomalyId.Value, cancellationToken);
        if (Anomaly == null)
        {
            TempData["Error"] = "Solução e Defeito não encontrado.";
            return Redirect("/List");
        }

        return Page();
    }
}
