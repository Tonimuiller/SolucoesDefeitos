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
    public ListViewModel<Model.Manufacturer>? ViewModel { get; set; }

    public async Task OnGetAsync(string manufacturerNameFilter)
    {
        ManufacturerNameFilter = manufacturerNameFilter;
        ViewModel = new ListViewModel<Model.Manufacturer>();
        ViewModel.Data = await _manufacturerService.GetAllAsync();
    }
}
