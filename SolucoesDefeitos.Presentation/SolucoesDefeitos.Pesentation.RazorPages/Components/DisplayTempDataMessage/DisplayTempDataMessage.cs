using Microsoft.AspNetCore.Mvc;

namespace SolucoesDefeitos.Pesentation.RazorPages.Components.DisplayTempDataMessage;

public sealed class DisplayTempDataMessage : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync()
    {
        return Task.FromResult<IViewComponentResult>(View("~/Components/DisplayTempDataMessage/Default.cshtml"));
    }
}
