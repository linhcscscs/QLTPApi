using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helper.ConfigHelper
{
    public static partial class ConfigHelper
    {
        public static class AppSettings
        {
            public static string ENCRYPT_SECRET => GetConfigByName("AppSettings:ENCRYPT_SECRET");
            public static string MONGODB_NAME => GetConfigByName("AppSettings:MONGODB_NAME");
            public static string VERSION => GetConfigByName("AppSettings:VERSION");
        }
    }
}
