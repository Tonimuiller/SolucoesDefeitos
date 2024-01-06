using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SolucoesDefeitos.BusinessDefinition.Service;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace SolucoesDefeitos.Pesentation.RazorPages.Pages.Identity.Account;

[AllowAnonymous]
public class LoginModel : PageModel
{
    private readonly IUserService _userService;

    public LoginModel(IUserService userService)
    {
        ArgumentNullException.ThrowIfNull(userService);
        _userService = userService;
    }

    [BindProperty]
    public string Login { get; set; } = default!;

    [BindProperty, DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(Login?.Trim()))
        {
            TempData["Error"] = "Informe o login.";
            return Page();
        }

        if (string.IsNullOrEmpty(Password?.Trim()))
        {
            TempData["Error"] = "Informe a senha.";
            return Page();
        }

        var user = await _userService.GetByCredentialsAsync(Login, Password, cancellationToken);
        if (user != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return RedirectToPage("/Index");
        }

        TempData["Error"] = "Login ou senha inválido(s).";
        return Page();
    }
}
