using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SolucoesDefeitos.BusinessDefinition.Service;

namespace SolucoesDefeitos.Pesentation.RazorPages.Pages.Product;

public sealed class FormModel : PageModel
{
    private readonly IProductService _productService;

    public FormModel(IProductService productService)
    {
        _productService = productService;
    }

    public Model.Product Product { get; set; }

    public async Task<IActionResult> OnGetAsync(int? productId, CancellationToken cancellationToken)
    {
        if (productId == null) 
        {
            Product = new Model.Product { Enabled = true };
            return Page();
        }

        Product = await _productService.GetByIdAsync(productId.Value, cancellationToken);
        if (Product == null)
        {
            return Redirect("./List");
        }

        return Page();
    }
}
