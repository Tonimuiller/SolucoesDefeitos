using Microsoft.AspNetCore.Mvc;

namespace SolucoesDefeitos.Pesentation.RazorPages.Pages.Anomaly.Components.Attachments;

public class Attachments: ViewComponent
{
    public Attachments()
    {
        
    }

    public Task<IViewComponentResult> InvokeAsync(IEnumerable<Model.Attachment> attachments, CancellationToken cancellationToken)
    {
        attachments ??= new List<Model.Attachment>();
        return Task.FromResult<IViewComponentResult>(View("~/Pages/Anomaly/Components/Attachment/Default.cshtml", attachments));
    }
}
