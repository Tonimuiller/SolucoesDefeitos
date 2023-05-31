using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SolucoesDefeitos.Pesentation.RazorPages.Pages.Product.Component.ProductForm;

public class DefaultModel : PageModel
{

    [BindProperty(SupportsGet = true)]
    public bool DisplayAsModal { get; set; }

    [BindProperty]
    public Model.Product Product { get; set; } = new Model.Product { Enabled = true};

    public Task OnGetAsync()
    {
        return Task.CompletedTask;
    }
}
