using Axia.Veiculos.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace Axia.Veiculos.WebApi.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected IActionResult HandleResult(Result result)
    {
        return StatusCode(result.StatusCode, new
        {
            result.IsSuccess,
            result.Message,
            result.StatusCode,
            result.Errors
        });
    }

    protected IActionResult HandleResult<T>(Result<T> result)
    {
        return StatusCode(result.StatusCode, new
        {
            result.IsSuccess,
            result.Message,
            result.StatusCode,
            result.Data,
            result.Errors
        });
    }
}
