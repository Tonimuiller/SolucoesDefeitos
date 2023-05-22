using Microsoft.AspNetCore.Mvc.RazorPages;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Dto;

namespace SolucoesDefeitos.Pesentation.RazorPages.Pages.ProductGroup;

public class ListModel : PageModel
{
    private readonly IProductGroupService _productGroupService;

    public ListModel(IProductGroupService productGroupService)
    {
        ArgumentNullException.ThrowIfNull(productGroupService);

        _productGroupService = productGroupService;
    }

    public string? ProductGroupDescriptionFilter { get; set; }
    public ListViewModel<Model.ProductGroup>? ViewModel { get; set; }

    public async Task OnGetAsync(CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 10, string? productGroupDescriptionFilter = null)
    {
        ProductGroupDescriptionFilter = productGroupDescriptionFilter;
        ViewModel = await _productGroupService.FilterAsync(cancellationToken, ProductGroupDescriptionFilter, pageIndex, pageSize);
    }
}
