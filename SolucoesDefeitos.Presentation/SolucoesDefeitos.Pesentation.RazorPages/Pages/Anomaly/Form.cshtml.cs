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
                TempData["Error"] = "N�o foi poss�vel encontrar o registro.";
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
            TempData["Error"] = "N�o foram enviados os dados da Solu��o e Defeito";
            return Page();
        }

        if (string.IsNullOrEmpty(Anomaly.Summary) || string.IsNullOrWhiteSpace(Anomaly.Summary))
        {
            TempData["Error"] = "O T�tulo/Resmo da Solu��o e Defeito � obrigat�rio.";
            return Page();
        }

        if (Anomaly.AnomalyId != 0)
        {
            var storedAnomaly = await _anomalyService.GetByKeyAsync(new { Anomaly.AnomalyId });
            if (storedAnomaly is null)
            {
                TempData["Error"] = "N�o foi poss�vel recuperar os dados da Solu��o e Defeito.";
                return Page();
            }

            storedAnomaly.UpdateDate = DateTime.Now;
            storedAnomaly.Description = Anomaly.Description;
            storedAnomaly.RepairSteps = Anomaly.RepairSteps;
            storedAnomaly.Summary = Anomaly.Summary;

            try
            {
                await _anomalyService.UpdateAsync(storedAnomaly);
                TempData["Success"] = "Solu��o e Defeito atualizada com sucesso.";
                return Redirect("./List");
            }
            catch
            {
                TempData["Error"] = "Ocorreu um erro ao atualizar a Solu��o e Defeito.";
                return Page();
            }
        }

        try
        {
            await _anomalyService.AddAsync(Anomaly);
            TempData["Success"] = "Solu��o e Defeito registrado com sucesso.";
            return Redirect("./List");
        }
        catch
        {
            TempData["Error"] = "Ocorreu um erro ao criar a Solu��o e Defeito.";
        }

        return Page();
    }
}
