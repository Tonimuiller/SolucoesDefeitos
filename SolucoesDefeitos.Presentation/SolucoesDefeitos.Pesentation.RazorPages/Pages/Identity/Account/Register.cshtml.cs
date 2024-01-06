using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SolucoesDefeitos.BusinessDefinition.Service;
using System.ComponentModel.DataAnnotations;

namespace SolucoesDefeitos.Pesentation.RazorPages.Pages.Identity.Account;

public sealed class RegisterModel : PageModel
{
    private readonly IUserService _userService;

    public RegisterModel(IUserService userService)
    {
        ArgumentNullException.ThrowIfNull(userService);
        _userService = userService;
    }

    [BindProperty]
    public string Name { get; set; } = default!;

    [BindProperty]
    public string Login { get; set; } = default!;

    [BindProperty]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    [BindProperty]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = default!;

    public void OnGet()
    {
    }

}
