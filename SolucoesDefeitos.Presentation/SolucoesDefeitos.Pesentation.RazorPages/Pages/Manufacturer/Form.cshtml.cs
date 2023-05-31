using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SolucoesDefeitos.BusinessDefinition.Service;

namespace SolucoesDefeitos.Pesentation.RazorPages.Pages.Manufacturer;

public class FormModel : PageModel
{
    private readonly IManufacturerService _manufacturerService;

    public FormModel(IManufacturerService manufacturerService)
    {
        _manufacturerService = manufacturerService;
    }

    [BindProperty]
    public Model.Manufacturer Manufacturer { get; set; } = new Model.Manufacturer { Enabled = true };

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken, int? manufacturerId = null)
    {
        if (manufacturerId.HasValue)
        {
            Manufacturer = await _manufacturerService.GetByIdAsync(manufacturerId.Value, cancellationToken);
            if (Manufacturer is null)
            {
                return Redirect("./List");
            }
        }
        else
        {
            Manufacturer = new Model.Manufacturer { Enabled = true };
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(Manufacturer.Name))
        {
            TempData["Error"] = "O Nome do Fabricante é obrigatório.";
            return Page();
        }

        if (Manufacturer?.ManufacturerId != 0)
        {
            var storedManufacturer = await _manufacturerService.GetByIdAsync(Manufacturer!.ManufacturerId, cancellationToken);
            if (storedManufacturer is null)
            {
                TempData["Error"] = "Não foi possível recuperar os dados do Fabricante.";
                return Page();
            }

            storedManufacturer.UpdateDate = DateTime.Now;
            storedManufacturer.Enabled = Manufacturer!.Enabled;
            storedManufacturer.Name = Manufacturer!.Name;
            try
            {
                var response = await _manufacturerService.UpdateAsync(storedManufacturer, cancellationToken);
                if (response.Success)
                {
                    TempData["Success"] = "Fabricante atualizado com sucesso.";
                    return Redirect("./List");
                }

                TempData["Error"] = string.Join("\n", response.Errors);
                return Page();
            }
            catch 
            {
                TempData["Error"] = "Ocorreu um erro ao atualizar o Fabricante";
                return Page();
            }
        }

        try
        {
            var response = await _manufacturerService.AddAsync(Manufacturer, cancellationToken);
            if (response.Success)
            {
                TempData["Success"] = "Fabricante criado com sucesso.";
                return Redirect("./List");
            }

            TempData["Error"] = string.Join("\n", response.Errors);
            return Page();
        }
        catch
        {
            TempData["Error"] = "Ocorreu um erro ao criar o Fabricante.";
        }

        return Page();
    }
}
