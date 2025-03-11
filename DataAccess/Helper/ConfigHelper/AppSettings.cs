using static DataAccess.Helper.Common.CommonHelper;

namespace DataAccess.Helper.ConfigHelper
{
    public static partial class ConfigHelper
    {
        public static class AppSettings
        {
            public static int NAM_HOC => ConvertTo(GetConfigByName("AppSettings:NAM_HOC"), 2022);
            public static string ENCRYPT_SECRET => GetConfigByName("AppSettings:ENCRYPT_SECRET");
            public static string MONGODB_NAME => GetConfigByName("AppSettings:MONGODB_NAME");
            public static string VERSION => GetConfigByName("AppSettings:VERSION");
            public static string CORS_NAME => GetConfigByName("AppSettings:CORS_NAME");
            public static int ThangDauKy2 => ConvertTo(GetConfigByName("AppSettings:ThangDauKy2"), 1);
            public static int ThangCuoiKy2 => ConvertTo(GetConfigByName("AppSettings:ThangDauKy2"), 8);
            public static string?[] CORS => Configuration.GetSection("AppSettings:CORS").GetChildren().Select(
            s => s.Value?.ToString()).ToArray() ?? new string[0];
        }
    }
}
