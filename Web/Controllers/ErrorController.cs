using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers;

public class ErrorController : Framework.Guama.Core.Presentation.Web.Common.Controllers.BaseWebController
{
    public ErrorController(Microsoft.Extensions.Localization.IStringLocalizer<ErrorController> localizer, ILogger<ErrorController> logger) : base(localizer, logger)
    {
    }

    public IActionResult Exception()
    {
        var feature = HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();

        if (feature != null)
        {
            logger.LogError(feature.Error, "Unexpected error handled!");
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [Route("error/403")]
    public IActionResult Forbidden()
    {
        Response.Clear();

        Response.StatusCode = StatusCodes.Status403Forbidden;

        return View();
    }

    [Route("error/404")]
    public IActionResult NotPageFound()
    {
        Response.Clear();

        Response.StatusCode = StatusCodes.Status404NotFound;

        return View();
    }

    [Route("error/500")]
    public IActionResult InternalFailure()
    {
        Response.Clear();

        Response.StatusCode = StatusCodes.Status500InternalServerError;

        return View();
    }

    [Route("error/{code}")]
    public IActionResult Index(int code)
    {
        Response.Clear();
        Response.StatusCode = code;

        return View();
    }
}