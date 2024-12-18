using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace trembonWoW.Core.Authorization;

public class ApiKeyAuthorizationFilter(IConfiguration configuration) : IAuthorizationFilter
{
    private const string ApiKeyHeaderName = "X-API-Key";

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        string? apiKey = context.HttpContext.Request.Headers[ApiKeyHeaderName];

        if (string.IsNullOrWhiteSpace(apiKey) || configuration["API:Key"] != apiKey)
        {
            context.Result = new UnauthorizedResult();
        }
    }
}