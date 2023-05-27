using Microsoft.AspNetCore.Mvc.RazorPages;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Dto;

namespace SolucoesDefeitos.Pesentation.RazorPages.Pages.Manufacturer;

public class ListModel : PageModel
{
    private readonly IManufacturerService _manufacturerService;

    public ListModel(
        IManufacturerService manufacturerService
        )
    {
        ArgumentNullException.ThrowIfNull(manufacturerService);

        _manufacturerService = manufacturerService;
    }

    public string? ManufacturerNameFilter { get; set; }
    public PagedData<Model.Manufacturer>? PagedData { get; set; }

    public async Task OnGetAsync(CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 10, string? manufacturerNameFilter = null)
    {
        ManufacturerNameFilter = manufacturerNameFilter;
        PagedData =  await _manufacturerService.FilterAsync(cancellationToken, manufacturerNameFilter, pageIndex, pageSize);
    }
}
