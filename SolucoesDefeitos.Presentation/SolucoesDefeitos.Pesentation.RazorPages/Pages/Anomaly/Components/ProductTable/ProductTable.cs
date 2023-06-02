using Microsoft.AspNetCore.Mvc;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Model;

namespace SolucoesDefeitos.Pesentation.RazorPages.Pages.Anomaly.Components.ProductTable;

public class ProductTable: ViewComponent
{
    private readonly IAnomalyProductSpecificationService _anomalyProductSpecificationService;

    public ProductTable(IAnomalyProductSpecificationService anomalyProductSpecificationService)
    {
        _anomalyProductSpecificationService = anomalyProductSpecificationService;
    }

    public async Task<IViewComponentResult> InvokeAsync(IEnumerable<AnomalyProductSpecification> products, CancellationToken cancellationToken)
    {
        await _anomalyProductSpecificationService.LoadProductsAsync(products, cancellationToken);
        return View("~/Pages/Anomaly/Components/ProductTable/Default.cshtml", products);
    }
}
