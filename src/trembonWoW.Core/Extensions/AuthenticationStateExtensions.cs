using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using trembonWoW.Core.Services;

namespace trembonWoW.Core.Extensions;

public static class AuthenticationStateExtensions
{
    public static async Task<T?> GetClaimsValue<T>(this Task<AuthenticationState>? authenticationState, string claimsName, T? defaultValue = default) where T : IConvertible
    {
        if (authenticationState is null)
            return defaultValue;

        var authState = await authenticationState;
        if (authState is null)
            return defaultValue;

        var user = authState?.User;
        if (user?.Identity is not null && user.Identity.IsAuthenticated)
        {
            var claim = user.FindFirst(claimsName);
            if (claim is not null)
                return (T)Convert.ChangeType(claim.Value, typeof(T));
        }

        return defaultValue;
    }
}