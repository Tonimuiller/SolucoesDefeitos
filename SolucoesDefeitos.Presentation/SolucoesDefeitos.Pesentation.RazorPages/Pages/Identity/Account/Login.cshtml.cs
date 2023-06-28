using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace SolucoesDefeitos.Pesentation.RazorPages.Pages.Identity.Account;

[AllowAnonymous]
public class LoginModel : PageModel
{
    [BindProperty]
    public string Login { get; set; }

    [BindProperty, DataType(DataType.Password)]
    public string Password { get; set; }

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

        if (Login == "admin" && Password == "admin12345")
        {
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "User")
                    };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return RedirectToPage("/Index");
        }

        TempData["Error"] = "Login ou senha inválido(s).";
        return Page();
    }
}
