using Microsoft.AspNetCore.Mvc;
using SolucoesDefeitos.Pesentation.RazorPages.Components.Pager.Model;

namespace SolucoesDefeitos.Pesentation.RazorPages.Components.Pager;

public sealed class Pager: ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync(PagerModel pagerModel)
    {
        return Task.FromResult<IViewComponentResult>(View("~/Components/Pager/Default.cshtml", pagerModel));
    }
}
