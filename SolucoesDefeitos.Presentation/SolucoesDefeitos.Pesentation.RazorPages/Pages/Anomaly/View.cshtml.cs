using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SolucoesDefeitos.BusinessDefinition.Service;

namespace SolucoesDefeitos.Pesentation.RazorPages.Pages.Anomaly;

public sealed class ViewModel : PageModel
{
    private readonly IAnomalyService _anomalyService;
    private readonly IAnomalyProductSpecificationService _anomalyProductSpecificationService;

    [BindProperty]
    public Model.Anomaly Anomaly { get; set; } = default!;

    public ViewModel(
        IAnomalyService anomalyService, 
        IAnomalyProductSpecificationService anomalyProductSpecificationService)
    {
        _anomalyService = anomalyService;
        _anomalyProductSpecificationService = anomalyProductSpecificationService;
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

        Anomaly.ProductSpecifications = (await _anomalyProductSpecificationService.GetByAnomalyIdAsync(anomalyId.Value, cancellationToken)).ToList();
        return Page();
    }
}
