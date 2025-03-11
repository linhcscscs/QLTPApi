using DataAccess.Helper.AuthHelper;
using DataAccess.Helper.Common;
using DataAccess.Helper.ConfigHelper;
using Microsoft.Extensions.Configuration.UserSecrets;
using QLTPApi.Authentication.Models;
using QLTPApi.Authentication.Values;

namespace QLTPApi.Authentication.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthenticationMiddleware(RequestDelegate next) => _next = next;
        public async Task Invoke(HttpContext httpContext, IAuthContext authContext)
        {
            await _next(httpContext);
        }
    }
}
