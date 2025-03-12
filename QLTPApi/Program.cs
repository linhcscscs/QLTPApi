using DataAccess.Helper.ConfigHelper;
using DataAccess.Helper.StartupHelper;
using Microsoft.Identity.Client;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add builder.Services to the container.

builder.Services.AddControllers();


#region Create Configuration
IConfiguration config = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", false, false)
                          .AddEnvironmentVariables()
                          .Build();
#endregion
#region Get Assembly
var referencedAssemblies = Assembly.GetExecutingAssembly()
.GetReferencedAssemblies()
.Select(Assembly.Load);
var currentAssemblies = Assembly.GetExecutingAssembly();
var assemblies = new List<Assembly>();
assemblies.AddRange(currentAssemblies);
assemblies.AddRange(referencedAssemblies);
#endregion
#region Set up service + config
{
    #region Get Base Startup + Base Config
    var startupAssemblies = new List<Assembly>();
    var configtartupAssemblies = new List<Assembly>();
    foreach (var assembly in assemblies)
    {
        var hasStartup = assembly.GetTypes()
            .Any(type => typeof(IBaseServiceStartup).IsAssignableFrom(type)
                         && !type.IsInterface
                         && !type.IsAbstract);

        var hasConfigStartup = assembly.GetTypes()
            .Any(type => typeof(IBaseConfigStartup).IsAssignableFrom(type)
                         && !type.IsInterface
                         && !type.IsAbstract);

        if (hasStartup)
        {
            startupAssemblies.Add(assembly);
        }

        if (hasConfigStartup)
        {
            configtartupAssemblies.Add(assembly);
        }
    }
    var configStartupClasses = configtartupAssemblies
    .SelectMany(assembly => assembly.GetTypes())
    .Where(type => typeof(IBaseConfigStartup).IsAssignableFrom(type)
                   && !type.IsInterface
                   && !type.IsAbstract)
    .ToList();

    var startupClasses = startupAssemblies
        .SelectMany(assembly => assembly.GetTypes())
        .Where(type => typeof(IBaseServiceStartup).IsAssignableFrom(type)
                       && !type.IsInterface
                       && !type.IsAbstract)
        .ToList();
    #endregion
    #region Run IBaseConfigStartup
    {
        foreach (var configStartupClass in configStartupClasses)
        {
            try
            {
                var instance = (IBaseConfigStartup)Activator.CreateInstance(configStartupClass)!;
                instance.Configure(config);
            }
            catch { }
        }
    }
    #endregion
    #region Run IBaseServiceStartup
    {
        foreach (var startupClass in startupClasses)
        {
            try
            {
                var instance = (IBaseServiceStartup)Activator.CreateInstance(startupClass)!;
                instance.Configure(builder.Services);
            }
            catch { }
        }
    }
    #endregion
}
#endregion
#region CORS
var lstCors = ConfigHelper.AppSettings.CORS;
if (lstCors.Count() > 0)
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: ConfigHelper.AppSettings.CORS_NAME,
                          policy =>
                          {
                              policy.WithOrigins(lstCors!)
                              .AllowAnyMethod()
                              .AllowCredentials()
                              .AllowAnyHeader();
                          });
    });
}
else
{
    if (builder.Environment.IsDevelopment())
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: ConfigHelper.AppSettings.CORS_NAME,
                              policy =>
                              {
                                  policy.AllowAnyHeader();
                                  policy.AllowAnyMethod();
                                  policy.AllowAnyOrigin();
                                  policy.AllowCredentials();
                              });
        });
    }
    else
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: ConfigHelper.AppSettings.CORS_NAME,
                              policy =>
                              {
                                  policy.AllowAnyHeader();
                                  policy.AllowAnyMethod();
                                  policy.AllowAnyOrigin();
                                  policy.AllowCredentials();
                              });
        });
    }
}
#endregion

var app = builder.Build();

#region Get Base App Startup
{
    var startupAssemblies = new List<Assembly>();
    foreach (var assembly in assemblies)
    {
        var hasStartup = assembly.GetTypes()
            .Any(type => typeof(IBaseAppStartup).IsAssignableFrom(type)
                         && !type.IsInterface
                         && !type.IsAbstract);

        if (hasStartup)
        {
            startupAssemblies.Add(assembly);
        }
    }
    var startupClasses = startupAssemblies
        .SelectMany(assembly => assembly.GetTypes())
        .Where(type => typeof(IBaseAppStartup).IsAssignableFrom(type)
                       && !type.IsInterface
                       && !type.IsAbstract)
        .ToList();
    foreach (var startupClass in startupClasses)
    {
        try
        {
            var instance = (IBaseAppStartup)Activator.CreateInstance(startupClass)!;
            instance.Configure(app);
        }
        catch { }
    }
}
#endregion

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
