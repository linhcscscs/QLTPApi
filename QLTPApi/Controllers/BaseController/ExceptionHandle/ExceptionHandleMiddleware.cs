using DataAccess.Helper.ControllerHelper.Values;
using DataAccess.Helper.ControllerHelper;
using System.Net;
using System.Text.Json;
using Azure;

namespace QLTPApi.Controllers.BaseController.ExceptionHandle
{
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandleMiddleware> _logger;

        public ExceptionHandleMiddleware(RequestDelegate next, ILogger<ExceptionHandleMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                HandleExceptionAsync(context, ex);
                var response = ControllerHelper.ReturnCode(ErrorCode.Internal_Error_Exception);
                var jsonResponse = JsonSerializer.Serialize(response.Value);
                await context.Response.WriteAsJsonAsync(jsonResponse);
            }
        }

        private static void HandleExceptionAsync(HttpContext context, Exception exception)
        {
        }
    }
}
