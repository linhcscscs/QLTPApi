using DataAccess.Caching.Interface;
using DataAccess.Caching.MemCache;
using DataAccess.Caching.Redis;
using DataAccess.Helper.ConfigHelper;
using DataAccess.Helper.StartupHelper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StackExchange.Redis;

namespace DataAccess.Startup
{
    public class DataAccessStartup : IBaseStartup, IBaseConfigStartup
    {
        public void Configure(IConfiguration configuration)
        {
            #region Config Helper
            ConfigHelper.Configuration = configuration;
            #endregion
        }

        public void Configure(IServiceCollection services)
        {
            #region Core Config
            #region Caching
            if (ConfigHelper.CacheSettings.CACHING_TYPE == "1")
            {
                var redisConnectionString = ConfigHelper.GetConnectionStringByName("RedisDefaultConnection");
                services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(redisConnectionString));
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisConnectionString;
                    options.InstanceName =  ConfigHelper.CacheSettings.REDIS_CACHE_NAME;
                });
                services.AddSingleton<ICacheProvider, RedisCacheProvider>();
            }
            else
                services.AddSingleton<ICacheProvider, MemCacheProvider>();
            #endregion
            #endregion
        }

        public void Configure(IApplicationBuilder app)
        {
        }
    }
}
