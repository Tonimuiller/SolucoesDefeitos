using Microsoft.AspNetCore.Mvc.RazorPages;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Pesentation.RazorPages.Pages.Shared;

namespace SolucoesDefeitos.Pesentation.RazorPages.Pages.Product;

public sealed class ListModel : BasePageModel
{
    public readonly IProductService _productService;

    public ListModel(IProductService productService)
    {
        _productService = productService;
    }

    public PagedData<Model.Product> PagedData { get; private set; } = new PagedData<Model.Product>();

    public async Task OnGetAsync(int? page, int? pageSize, CancellationToken cancellationToken)
    {
        PagedData = await _productService.FilterAsync(page ?? 1, pageSize ?? 20, cancellationToken);
    }
}
