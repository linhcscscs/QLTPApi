using DataAccess.Helper.StartupHelper;
using Microsoft.AspNetCore.Authentication;

namespace QLTPApi.Startup
{
    public class MiddlewareRegister : IBaseStartup
    {
        public void Configure(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app)
        {
            #region JWT & Authen
            //app.UseMiddleware<AuthenticationMiddleware>();
            #endregion
        }
    }
}
