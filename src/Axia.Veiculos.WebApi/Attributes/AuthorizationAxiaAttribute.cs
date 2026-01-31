using Axia.Veiculos.Application.Common;
using Axia.Veiculos.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Axia.Veiculos.WebApi.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class AuthorizationAxiaAttribute : Attribute, IAuthorizationFilter
{
    private readonly Role[] _roles;

    public AuthorizationAxiaAttribute(params Role[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            var result = Result.Unauthorized();
            context.Result = new JsonResult(new { result.IsSuccess, result.Message, result.StatusCode })
            {
                StatusCode = result.StatusCode
            };
            return;
        }

        var roleClaim = user.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(roleClaim) || !Enum.TryParse<Role>(roleClaim, out var userRole))
        {
            var result = Result.Forbidden();
            context.Result = new JsonResult(new { result.IsSuccess, result.Message, result.StatusCode })
            {
                StatusCode = result.StatusCode
            };
            return;
        }

        if (_roles.Length > 0 && !_roles.Contains(userRole))
        {
            var result = Result.Forbidden();
            context.Result = new JsonResult(new { result.IsSuccess, result.Message, result.StatusCode })
            {
                StatusCode = result.StatusCode
            };
        }
    }
}
