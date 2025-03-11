using DataAccess.Helper.ConfigHelper;
using DataAccess.Helper.StartupHelper;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add builder.Services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

#region Create Configuration
IConfiguration config = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", false, false)
                          .AddEnvironmentVariables()
                          .Build();
#endregion
#region Get Base Startup + Base Config
var referencedAssemblies = Assembly.GetExecutingAssembly()
    .GetReferencedAssemblies()
    .Select(Assembly.Load);
var startupAssemblies = new List<Assembly>();
var configtartupAssemblies = new List<Assembly>();
foreach (var assembly in referencedAssemblies)
{
    var hasStartup = assembly.GetTypes()
        .Any(type => typeof(IBaseStartup).IsAssignableFrom(type)
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
    .Where(type => typeof(IBaseStartup).IsAssignableFrom(type)
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
#region Run IBaseStartup
{
    foreach (var startupClass in startupClasses)
    {
        try
        {
            var instance = (IBaseStartup)Activator.CreateInstance(startupClass)!;
            instance.Configure(builder.Services);
        }
        catch { }
    }
}
#endregion

#region Logger
if(ConfigHelper.LogSettings.LOGGING_TYPE == "1")
{
    Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()  // Log ra console
    .WriteTo.File($"Logs/log{DateTime.Now:ddMMyyyy}.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
    builder.Host.UseSerilog();
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
#region Run IBaseStartup
foreach (var startupClass in startupClasses)
{
    try
    {
        var instance = (IBaseStartup)Activator.CreateInstance(startupClass)!;
        instance.Configure(app);
    }
    catch { }
}
#endregion

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
