using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Model;
using SolucoesDefeitos.Pesentation.RazorPages.Pages.Anomaly.Components.Attachments;
using SolucoesDefeitos.Pesentation.RazorPages.Pages.Anomaly.Components.ProductTable;

namespace SolucoesDefeitos.Pesentation.RazorPages.Pages.Anomaly;

public sealed class FormModel : PageModel
{
    private readonly IAnomalyService _anomalyService;
    private readonly IAnomalyProductSpecificationService _anomalyProductSpecificationService;

    public FormModel(
        IAnomalyService anomalyService, 
        IAnomalyProductSpecificationService anomalyProductSpecificationService)
    {
        ArgumentNullException.ThrowIfNull(anomalyService);
        _anomalyService = anomalyService;
        _anomalyProductSpecificationService = anomalyProductSpecificationService;
    }

    [BindProperty]
    public Model.Anomaly Anomaly { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken, int? anomalyId = null)
    {
        if (anomalyId != null)
        {
            Anomaly = await _anomalyService.GetByIdAsync(anomalyId.Value, cancellationToken);
            if (Anomaly == null)
            {
                TempData["Error"] = "Não foi possível encontrar o registro.";
                return Redirect("./List");
            }

            Anomaly.ProductSpecifications = (await _anomalyProductSpecificationService.GetByAnomalyIdAsync(anomalyId.Value, cancellationToken)).ToList();
        }

        Anomaly = Anomaly ?? new Model.Anomaly
        {
            ProductSpecifications = new List<AnomalyProductSpecification>(),
            Attachments = new List<Attachment>()
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (Anomaly is null)
        {
            TempData["Error"] = "Não foram enviados os dados da Solução e Defeito";
            return Page();
        }

        if (string.IsNullOrEmpty(Anomaly.Summary) || string.IsNullOrWhiteSpace(Anomaly.Summary))
        {
            TempData["Error"] = "O Título/Resmo da Solução e Defeito é obrigatório.";
            return Page();
        }

        if (Anomaly.AnomalyId != 0)
        {
            try
            {
                Anomaly.ProductSpecifications ??= new List<AnomalyProductSpecification>();
                var response = await _anomalyService.UpdateAsync(Anomaly, cancellationToken);
                if (response.Success)
                {
                    TempData["Success"] = "Solução e Defeito atualizada com sucesso.";
                    return Redirect("./List");
                }

                TempData["Error"] = string.Join("\n", response.Errors);
                return Page();
            }
            catch
            {
                TempData["Error"] = "Ocorreu um erro ao atualizar a Solução e Defeito.";
                return Page();
            }
        }

        try
        {
            var response = await _anomalyService.AddAsync(Anomaly, cancellationToken);
            if (response.Success)
            {
                TempData["Success"] = "Solução e Defeito registrado com sucesso.";
                return Redirect("./List");
            }

            TempData["Error"] = string.Join("\n", response.Errors);
            return Page();
        }
        catch
        {
            TempData["Error"] = "Ocorreu um erro ao criar a Solução e Defeito.";
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAttachmentsChangeAsync(IEnumerable<Attachment> attachments, CancellationToken cancellationToken)
    {
        return await Task.FromResult<IActionResult>(ViewComponent(nameof(Attachments), new { attachments }));
    }
}
