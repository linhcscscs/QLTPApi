using DataAccess.Helper.StartupHelper;
using DataAccess.SQL.QLTP.Repository;
using DataAccess.SQL.QLTP.Repository.BaseRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            //services.AddTransient<IPhongGDRepository, PhongGDRepository>();
            //services.AddTransient<ITruongRepository, TruongRepository>();
            //services.AddTransient<ISoGDRepository, SoGDRepository>();
            //services.AddTransient<INguoiDungRepository, NguoiDungRepository>();
            //services.AddTransient<IGroupUserRepository, GroupUserRepository>();
            //services.AddTransient<IGroupUserMenuRepository, GroupUserMenuRepository>();
            //services.AddTransient<INhanSuRepository, NhanSuRepository>();
            #region QLTP Repository Register
            {
                var assemblies = Assembly.GetExecutingAssembly()
                                .GetReferencedAssemblies()
                                .Select(Assembly.Load).ToList(); ; // Lấy assembly hiện tại

                foreach (var assembly in assemblies)
                {
                    var repositoryTypes = assembly.GetTypes()
                    .Where(type => type.IsClass && !type.IsAbstract) // Lọc các class không phải abstract
                    .Where(type => type.GetInterfaces().Any(i =>
                        i.IsGenericType && i.GetGenericTypeDefinition() == typeof(QLTPRepository<>))) // Chỉ lấy class implement QLTPRepository<>
                    .ToList();

                    foreach (var implType in repositoryTypes)
                    {
                        var interfaceType = implType.GetInterfaces()
                            .FirstOrDefault(i => i != typeof(IQLTPRepository<>) && i.GetGenericTypeDefinition() == typeof(IQLTPRepository<>));

                        if (interfaceType != null)
                        {
                            services.AddTransient(interfaceType, implType);
                        }
                    }
                }
            }
            #endregion
            #endregion
        }
    }
}
