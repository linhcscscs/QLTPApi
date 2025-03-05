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
            public static string Secret => GetConfigByName("JWT:Secret");
            public static double TokenValidityInMinutes => double.Parse(GetConfigByName("JWT:TokenValidityInMinutes"));
            public static double RememberTokenValidityInMinute => double.Parse(GetConfigByName("JWT:RememberTokenValidityInMinute"));
            public static double RefreshTokenValidityInDays => double.Parse(GetConfigByName("JWT:RefreshTokenValidityInDays"));
            public static string ValidAudience => GetConfigByName("JWT:ValidAudience");
            public static string ValidIssuer => GetConfigByName("JWT:ValidIssuer");
        }
    }
}
