using Microsoft.AspNetCore.Mvc;
using SolucoesDefeitos.Pesentation.RazorPages.Components.Pager.Model;

namespace SolucoesDefeitos.Pesentation.RazorPages.Pages.Product.Component.ProductForm;

public sealed class ProductForm: ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync(PagerModel pagerModel)
    {
        return Task.FromResult<IViewComponentResult>(View("~/Pages/Product/Component/ProductForm/Default.cshtml"));
    }
}
