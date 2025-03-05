using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helper.ConfigHelper
{
    public static partial class ConfigHelper
    {
        public static class CacheSettings
        {
            public static string CACHING_TYPE => GetConfigByName("CachingSettings:CACHING_TYPE");
            public static string REDIS_CACHE_NAME => GetConfigByName("CachingSettings:REDIS_CACHE_NAME");
        }
    }
}
