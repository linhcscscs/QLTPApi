using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DataAccess.Helper.StartupHelper
{
    public interface IBaseStartup
    {
        void Configure(IServiceCollection services);
        void Configure(IApplicationBuilder app);
    }
}
