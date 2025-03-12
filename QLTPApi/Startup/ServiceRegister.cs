using DataAccess.Helper.StartupHelper;
using QLTPApi.Authentication;
using QLTPApi.Service;

namespace QLTPApi.Startup
{
    public class ServiceRegister : IBaseServiceStartup
    {
        public void Configure(IServiceCollection services)
        {
            services.AddScoped<ITestService, TestSerivce>();

            services.AddScoped<IAuthService, AuthService>();
        }

        public void Configure(IApplicationBuilder app)
        {
        }
    }
}
