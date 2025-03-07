using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helper.CommonHelper
{
    public static class CommonHelper
    {
        public static T ConvertTo<T>(object? input, T instead) where T : struct
        {
            if (input == null || input is DBNull) return instead;
            try
            {
                if (input is T value) return value;
                return (T)Convert.ChangeType(input, typeof(T));
            }
            catch
            {
                return instead;
            }
        }

        public static T? ConvertTo<T>(object? input) where T : struct
        {
            if (input == null || input is DBNull) return null;
            try
            {
                if (input is T value) return value;
                return (T)Convert.ChangeType(input, typeof(T));
            }
            catch
            {
                return null;
            }
        }
        public static T IfNull<T>(T? value, T instead)
        {
            if (value is string str && string.IsNullOrEmpty(str))
            {
                return instead;
            }
            return value ?? instead;
        }
    }
}
