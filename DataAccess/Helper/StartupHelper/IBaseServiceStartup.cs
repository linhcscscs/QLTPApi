using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DataAccess.Helper.StartupHelper
{
    public interface IBaseServiceStartup
    {
        void Configure(IServiceCollection services);
    }
}
