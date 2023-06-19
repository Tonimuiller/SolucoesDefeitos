using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.BusinessImplementation.Service;

namespace SolucoesDefeitos.Pesentation.RazorPages.Pages.Anomaly;

public sealed class ViewModel : PageModel
{
    private readonly IAnomalyService _anomalyService;
    private readonly IAnomalyProductSpecificationService _anomalyProductSpecificationService;
    private readonly IAttachmentService _attachmentService;

    [BindProperty]
    public Model.Anomaly Anomaly { get; set; } = default!;

    public ViewModel(
        IAnomalyService anomalyService,
        IAnomalyProductSpecificationService anomalyProductSpecificationService,
        IAttachmentService attachmentService)
    {
        _anomalyService = anomalyService;
        _anomalyProductSpecificationService = anomalyProductSpecificationService;
        _attachmentService = attachmentService;
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
        Anomaly.Attachments = (await _attachmentService.GetByAnomalyIdAsync(anomalyId.Value, cancellationToken)).ToList();
        return Page();
    }
}
