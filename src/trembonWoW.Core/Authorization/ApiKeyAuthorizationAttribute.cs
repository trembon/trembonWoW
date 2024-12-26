using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trembonWoW.Core.Authorization;

public class ApiKeyAuthorizationAttribute : ServiceFilterAttribute
{
    public ApiKeyAuthorizationAttribute()
        : base(typeof(ApiKeyAuthorizationFilter))
    {
    }
}
