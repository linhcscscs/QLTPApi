using DataAccess.Helper.StartupHelper;
using DataAccess.SQL.QLTP.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Startup
{
    public class RepositoryRegister : IBaseStartup
    {
        public void Configure(IApplicationBuilder app)
        {
        }

        public void Configure(IServiceCollection services)
        {
            #region Repository
            services.AddTransient<IPhongGDRepository, PhongGDRepository>();
            services.AddTransient<ITruongRepository, TruongRepository>();
            services.AddTransient<ISoGDRepository, SoGDRepository>();
            services.AddTransient<INguoiDungRepository, NguoiDungRepository>();
            services.AddTransient<IGroupUserRepository, GroupUserRepository>();
            services.AddTransient<IGroupUserMenuRepository, GroupUserMenuRepository>();
            #endregion
        }
    }
}
