using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SolucoesDefeitos.BusinessDefinition.Service;

namespace SolucoesDefeitos.Pesentation.RazorPages.Pages.ProductGroup;

public sealed class FormModel : PageModel
{
    private readonly IProductGroupService _productGroupService;

    public FormModel(IProductGroupService productGroupService)
    {
        ArgumentNullException.ThrowIfNull(productGroupService);
        _productGroupService = productGroupService;
    }

    [BindProperty]
    public Model.ProductGroup? ProductGroup { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken, int? productGroupId = null)
    {
        if (productGroupId is null)
        {
            ProductGroup = new Model.ProductGroup();
            ProductGroup.Enabled = true;
        }
        else
        {
            ProductGroup = await _productGroupService.GetByKeyAsync(new { productGroupId = productGroupId });
            if (ProductGroup is null)
            {
                return Redirect("./List");
            }
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (ProductGroup is null)
        {
            TempData["Error"] = "Não foram enviados dados do Grupo de Produto.";
            return Page();
        }

        if (string.IsNullOrEmpty(ProductGroup.Description))
        {
            TempData["Error"] = "A descrição do Grupo de Produtos é obrigatória.";
            return Page();
        }

        if (ProductGroup!.ProductGroupId != 0)
        {
            var storedProductGroup = await _productGroupService.GetByKeyAsync(new { productGroupId = ProductGroup!.ProductGroupId });
            if (storedProductGroup is null)
            {
                TempData["Error"] = "Não foi possível recuperar os dados do Grupo de Produtos.";
                return Page();
            }

            storedProductGroup.UpdateDate = DateTime.Now;
            storedProductGroup.Enabled = ProductGroup!.Enabled;
            storedProductGroup.Description = ProductGroup!.Description;
            try
            {
                await _productGroupService.UpdateAsync(storedProductGroup);
                TempData["Success"] = "Grupo de Produtos atualizado com sucesso.";
                return Redirect("./List");
            }
            catch
            {
                TempData["Error"] = "Ocorreu um erro ao atualizar o Grupo de Produtos";
                return Page();
            }
        }

        try
        {
            await _productGroupService.AddAsync(ProductGroup);
            TempData["Success"] = "Grupo de Produtos criado com sucesso.";
            return Redirect("./List");
        }
        catch
        {
            TempData["Error"] = "Ocorreu um erro ao criar o Grupo de Produtos.";
        }

        return Page();
    }
}
