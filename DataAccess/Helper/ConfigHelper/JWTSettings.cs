using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helper.ConfigHelper
{
    public static partial class ConfigHelper
    {
        public static class JWTSettings
        {
            public static string Secret => GetConfigByName("JWTSettings:Secret");
            public static double TokenValidityInMinutes => double.Parse(GetConfigByName("JWTSettings:TokenValidityInMinutes"));
            public static double RefreshTokenValidityInMinutes => double.Parse(GetConfigByName("JWTSettings:RefreshTokenValidityInMinutes"));
            public static double RefreshTokenValidityInDays => double.Parse(GetConfigByName("JWTSettings:RefreshTokenValidityInDays"));
            public static string ValidAudience => GetConfigByName("JWTSettings:ValidAudience");
            public static string ValidIssuer => GetConfigByName("JWTSettings:ValidIssuer");
        }
    }
}
