using Azure.Core;
using DataAccess.Helper.ControllerHelper;
using DataAccess.Helper.ControllerHelper.Values;
using DataAccess.Helper.Extensions.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using QLTPApi.Authentication;
using QLTPApi.Service;

namespace QLTPApi.Controllers.BaseController.AttributeCollection
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AllowedIpAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var service = context.HttpContext.RequestServices.GetService<ITestService>();
            var ip = context.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            }
            if (string.IsNullOrEmpty(ip) || service.IpTesting().Contains(ip))
            {
                context.Result = ControllerHelper.ReturnCode(ErrorCode.Forbidden);
                return;
            }
        }
    }
}
