using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace DataAccess.Helper.ConfigHelper
{
    public static partial class ConfigHelper
    {
        public static IConfiguration Configuration;
        public static string GetConnectionStringByName(string connectionStringName, string instead = "") => Configuration.GetConnectionString(connectionStringName) ?? Configuration.GetConnectionString(instead) ?? "";
        public static string GetConfigByName(string name) => Configuration[name] ?? "";
        public static string GetConfigByName(this IConfiguration configuration, string name) => configuration[name] ?? "";
        public static int NamHoc
        {
            get
            {
                var ma_nam_hoc = 2022;
                if (int.TryParse(GetConfigByName("AppSettings:NAM_HOC"), out var ma_nam_hoc_temp))
                {
                    ma_nam_hoc = ma_nam_hoc_temp;
                }
                return ma_nam_hoc;
            }
        }
    }
}
