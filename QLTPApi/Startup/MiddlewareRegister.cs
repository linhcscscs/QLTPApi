using DataAccess.Helper.StartupHelper;
using Microsoft.AspNetCore.Authentication;
using QLTPApi.Controllers.BaseController.ExceptionHandle;

namespace QLTPApi.Startup
{
    public class MiddlewareRegister : IBaseAppStartup
    {
        public void Configure(WebApplication app)
        {
            #region Exception Handle
            app.UseMiddleware<ExceptionHandleMiddleware>();
            #endregion
        }
    }
}
