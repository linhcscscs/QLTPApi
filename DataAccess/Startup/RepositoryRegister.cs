using DataAccess.Helper.ConfigHelper;
using DataAccess.Helper.StartupHelper;
using DataAccess.SQL.QLTP.Context;
using DataAccess.SQL.QLTP.Models;
using DataAccess.SQL.QLTP.Repository;
using DataAccess.SQL.QLTP.Repository.BaseRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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
    public class RepositoryRegister : IBaseServiceStartup
    {
        public void Configure(IServiceCollection services)
        {
            #region DbContext
            services.AddDbContext<QUAN_LY_THU_PHI_2025Context>(options => options.UseSqlServer(ConfigHelper.GetConnectionStringByName("QUAN_LY_THU_PHI_2025Context")));
            #endregion
            #region Context Factory
            services.AddScoped<IQLTPContextFactory, QLTPContextFactory>();
            #endregion
            #region Repository
            #region QLTP Repository Register
            {
                #region Get Assembly
                var referencedAssemblies = Assembly.GetExecutingAssembly()
                .GetReferencedAssemblies()
                .Select(Assembly.Load);
                var currentAssemblies = Assembly.GetExecutingAssembly();
                var assemblies = new List<Assembly>();
                assemblies.AddRange(currentAssemblies);
                assemblies.AddRange(referencedAssemblies);
                #endregion

                foreach (var assembly in assemblies)
                {
                    var repositoryTypes = assembly.GetTypes()
                    .Where(type => type.IsClass && !type.IsAbstract) // Lọc các class không phải abstract
                    .Where(type => type.GetInterfaces().Any(i =>
                        i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQLTPRepository<>))) // Chỉ lấy class implement QLTPRepository<>
                    .ToList();

                    foreach (var implType in repositoryTypes)
                    {
                        var interfaceType = implType.GetInterfaces()
                            .FirstOrDefault(i =>
                            i != typeof(IQLTPRepository<>)
                            && !i.IsGenericType);

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
