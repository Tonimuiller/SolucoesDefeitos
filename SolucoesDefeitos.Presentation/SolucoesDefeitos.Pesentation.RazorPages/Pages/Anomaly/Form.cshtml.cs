using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SolucoesDefeitos.BusinessDefinition.Service;

namespace SolucoesDefeitos.Pesentation.RazorPages.Pages.Anomaly;

public sealed class FormModel : PageModel
{
    private readonly IAnomalyService _anomalyService;

    public FormModel(IAnomalyService anomalyService)
    {
        ArgumentNullException.ThrowIfNull(anomalyService);
        _anomalyService = anomalyService;
    }

    [BindProperty]
    public Model.Anomaly? Anomaly { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken, int? anomalyId = null)
    {
        if (anomalyId != null)
        {
            Anomaly = await _anomalyService.GetByKeyAsync(new { anomalyId });
            if (Anomaly == null)
            {
                TempData["Error"] = "Não foi possível encontrar o registro.";
                return Redirect("./List");
            }
        }

        Anomaly = Anomaly ?? new Model.Anomaly();

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
            var storedAnomaly = await _anomalyService.GetByKeyAsync(new { Anomaly.AnomalyId });
            if (storedAnomaly is null)
            {
                TempData["Error"] = "Não foi possível recuperar os dados da Solução e Defeito.";
                return Page();
            }

            storedAnomaly.UpdateDate = DateTime.Now;
            storedAnomaly.Description = Anomaly.Description;
            storedAnomaly.RepairSteps = Anomaly.RepairSteps;
            storedAnomaly.Summary = Anomaly.Summary;

            try
            {
                await _anomalyService.UpdateAsync(storedAnomaly);
                TempData["Success"] = "Solução e Defeito atualizada com sucesso.";
                return Redirect("./List");
            }
            catch
            {
                TempData["Error"] = "Ocorreu um erro ao atualizar a Solução e Defeito.";
                return Page();
            }
        }

        try
        {
            await _anomalyService.AddAsync(Anomaly);
            TempData["Success"] = "Solução e Defeito registrado com sucesso.";
            return Redirect("./List");
        }
        catch
        {
            TempData["Error"] = "Ocorreu um erro ao criar a Solução e Defeito.";
        }

        return Page();
    }
}
