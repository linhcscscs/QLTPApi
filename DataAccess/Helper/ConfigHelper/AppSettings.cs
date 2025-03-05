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
            public static int NAM_HOC
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
            public static string ENCRYPT_SECRET => GetConfigByName("AppSettings:ENCRYPT_SECRET");
            public static string MONGODB_NAME => GetConfigByName("AppSettings:MONGODB_NAME");
            public static string VERSION => GetConfigByName("AppSettings:VERSION");
        }
    }
}
