using DataAccess.Helper.StartupHelper;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
        var instance = (IBaseConfigStartup)Activator.CreateInstance(configStartupClass)!;
        instance.Configure(config);
    }
}
#endregion
#region Run IBaseStartup
{
    foreach (var startupClass in startupClasses)
    {
        var instance = (IBaseStartup)Activator.CreateInstance(startupClass)!;
        instance.Configure(builder.Services);
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
    var instance = (IBaseStartup)Activator.CreateInstance(startupClass)!;
    instance.Configure(app);
}
#endregion

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
