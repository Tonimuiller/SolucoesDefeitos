using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SolucoesDefeitos.Pesentation.RazorPages.Components.DisplayTempDataMessage;

namespace SolucoesDefeitos.Pesentation.RazorPages.Pages.Shared;

public abstract class BasePageModel: PageModel
{
    public async Task<IActionResult> OnPostTempDataErrorAsync(string errorMessage, CancellationToken cancellationToken)
    {
        TempData["Error"] = errorMessage;
        return await Task.FromResult<IActionResult>(ViewComponent(nameof(DisplayTempDataMessage)));
    }
}
