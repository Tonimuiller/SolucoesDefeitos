using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Dto.Anomaly.Request;

namespace SolucoesDefeitos.Pesentation.RazorPages.Pages.Anomaly;

public sealed class ListModel : PageModel
{
    private readonly IAnomalyService _anomalyService;
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IProductGroupRepository _productGroupRepository;
    private readonly IProductRepository _productRepository;

    public ListModel(
        IAnomalyService anomalyService,
        IManufacturerRepository manufacturerRepository,
        IProductGroupRepository productGroupRepository,
        IProductRepository productRepository)
    {
        ArgumentNullException.ThrowIfNull(anomalyService);
        _anomalyService = anomalyService;
        _manufacturerRepository = manufacturerRepository;
        _productGroupRepository = productGroupRepository;
        _productRepository = productRepository;
    }

    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public int[] ManufacturerIds { get; set; }

    [BindProperty(SupportsGet = true)]
    public int[] ProductGroupIds { get; set; }

    [BindProperty(SupportsGet = true)]
    public int[] ProductIds { get; set; }

    [BindProperty(SupportsGet = true)]
    public int PageIndex { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    public int PageSize { get; set; } = 10;

    public IEnumerable<Model.Manufacturer> Manufacturers { get; set; } = Enumerable.Empty<Model.Manufacturer>();
    public IEnumerable<Model.ProductGroup> ProductGroups { get; set; } = Enumerable.Empty<Model.ProductGroup>();
    public IEnumerable<Model.Product> Products { get; set; } = Enumerable.Empty<Model.Product>();
    public PagedData<Model.Anomaly> PagedData { get; set; } = new PagedData<Model.Anomaly>();

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        await LoadFilterOptionsAsync(cancellationToken);
        PagedData = await _anomalyService.FilterAsync(
            new AnomalyFilterRequest
            {
                Page = PageIndex,
                PageSize = PageSize,
                SearchTerm = SearchTerm,
                ManufacturerIds = ManufacturerIds,
                ProductGroupIds = ProductGroupIds,
                ProductIds = ProductIds,
            }, cancellationToken);
        return Page();
    }

    private async Task LoadFilterOptionsAsync(CancellationToken cancellationToken)
    {
        Manufacturers = await _manufacturerRepository.GetAllEnabledNameOrderedAsync(cancellationToken);
        if (ManufacturerIds?.Any() ?? false)
        {
            ProductGroups = await _productGroupRepository.GetAllEnabledByManufacturerIdsDescriptionOrderedAsync(ManufacturerIds, cancellationToken);
            if (ProductGroupIds?.Any() ?? false)
            {
                Products = await _productRepository.GetAllEnabledByProductGroupIdsAsync(ProductGroupIds, cancellationToken);
            }
        }
    }
}
