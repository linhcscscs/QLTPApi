
using DataAccess.Helper.StartupHelper;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Reflection;
using QLTPApi.Authentication;
using DataAccess.Helper.LazyDI;
using DataAccess.Helper.AuthHelper;

namespace QLTPApi.Startup
{
    public class Startup : IBaseServiceStartup, IBaseAppStartup, IBaseConfigStartup
    {

        public void Configure(IConfiguration configuration)
        {
        }
        public void Configure(IServiceCollection services)
        {
            #region HttpContextAccessor
            services.AddHttpContextAccessor();
            #endregion

            #region Core Configure
            #region Compression
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            });
            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.SmallestSize;
            });
            #endregion

            #region JWT Authen
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = AuthHelper.GetTokenValidationParameters();
            });
            #endregion

            #region Swagger + OpenApi
            // Swagger JWT config
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            });

            services.AddOpenApi();
            #endregion

            #region Lazy DI
            services.AddTransient(typeof(Lazy<>), typeof(LazyServiceProvider<>));
            #endregion

            #region Auth Services
            services.AddSingleton<IAuthContext, AuthContext>();
            #endregion

            #region Cấu hình return API giữ nguyên tên field
            services.AddControllers().AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    });
            #endregion

            #endregion
        }

        public void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }
    }
}
