using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using System.Threading;

namespace SolucoesDefeitos.Pesentation.RazorPages.Pages.Product;

public sealed class FormModel : PageModel
{
    private readonly IProductService _productService;
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IProductGroupRepository _productGroupRepository;

    public FormModel(
        IProductService productService,
        IManufacturerRepository manufacturerRepository,
        IProductGroupRepository productGroupRepository)
    {
        _productService = productService;
        _manufacturerRepository = manufacturerRepository;
        _productGroupRepository = productGroupRepository;        
    }

    [BindProperty]
    public Model.Product Product { get; set; }

    public IEnumerable<Model.Manufacturer> Manufacturers { get; private set; }

    public IEnumerable<Model.ProductGroup> ProductGroups { get; private set; }
     
    public async Task<IActionResult> OnGetAsync(int? productId, CancellationToken cancellationToken)
    {
        await InitializeModelAvailableReferencesAsync(cancellationToken);
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

        await InitializeModelAvailableReferencesAsync(cancellationToken);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        await InitializeModelAvailableReferencesAsync(cancellationToken);
        if (string.IsNullOrEmpty(Product?.Name))
        {
            TempData["Error"] = "O Nome do Produto é obrigatório.";
            return Page();
        }

        if (Product?.ProductId != 0)
        {
            var storedProduct = await _productService.GetByIdAsync(Product!.ProductId, cancellationToken);
            if (storedProduct is null)
            {
                TempData["Error"] = "Não foi possível recuperar os dados do Produto.";
                return Page();
            }

            storedProduct.UpdateDate = DateTime.Now;
            storedProduct.Enabled = Product!.Enabled;
            storedProduct.Name = Product!.Name;
            storedProduct.ProductGroupId = Product!.ProductGroupId;
            storedProduct.ManufacturerId = Product!.ManufacturerId;
            storedProduct.Code = Product!.Code;

            try
            {
                var response = await _productService.UpdateAsync(storedProduct, cancellationToken);
                if (response.Success)
                {
                    TempData["Success"] = "Produto atualizado com sucesso.";
                    return Redirect("./List");
                }

                TempData["Error"] = string.Join("\n", response.Errors);
                return Page();
            }
            catch
            {
                TempData["Error"] = "Ocorreu um erro ao atualizar o Produto";
                return Page();
            }
        }

        try
        {
            var response = await _productService.AddAsync(Product, cancellationToken);
            if (response.Success)
            {
                TempData["Success"] = "Produto criado com sucesso.";
                return Redirect("./List");
            }

            TempData["Error"] = string.Join("\n", response.Errors);
            return Page();
        }
        catch
        {
            TempData["Error"] = "Ocorreu um erro ao criar o Produto.";
        }

        return Page();
    }

    private async Task InitializeModelAvailableReferencesAsync(CancellationToken cancellationToken)
    {
        Manufacturers = await _manufacturerRepository.GetAllEnabledNameOrderedAsync(cancellationToken);
        ProductGroups = await _productGroupRepository.GetAllEnabledDescriptionOrderedAsync(cancellationToken);
    }
}
