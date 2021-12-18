using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SolucoesDefeitos.Controllers.Extension
{
    public static class InternalServerErrorResponse
    {
        public static IActionResult InternalServerError(this ControllerBase controllerBase, object body)
        {
            return controllerBase.StatusCode((int)HttpStatusCode.InternalServerError, body);
        }
    }
}
