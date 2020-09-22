using System;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleCarCatalogue.Exceptions;

namespace SimpleCarCatalogue.Gui.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : Controller
    {
        public void Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;

            var statusCode = HttpStatusCode.InternalServerError;
            if (exception is ObjectNotExistsException)
                statusCode = HttpStatusCode.NotFound;
            else if (exception is ArgumentException)
                statusCode = HttpStatusCode.BadRequest;

            Response.StatusCode = (int)statusCode;
            Response.WriteAsync(exception.Message);
        }
    }
}
