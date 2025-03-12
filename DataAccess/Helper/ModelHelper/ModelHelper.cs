using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccess.Helper.LazyDI.ModelHelper
{
    public static class ModelHelper
    {
        public static bool ValidContainProps(this Type type, params string[] propNames)
        {
            foreach (var propName in propNames)
            {
                var prop = type.GetProperty(propName);
                if (prop == null)
                {
                    return false;
                }
            }
            return true;
        }
        public static string SnakeToPascalCase(this string snakeCase)
        {
            if (string.IsNullOrEmpty(snakeCase))
                return string.Empty;

            return string.Join(string.Empty,
                snakeCase.Split('_')
                         .Where(word => !string.IsNullOrEmpty(word))
                         .Select(word => char.ToUpper(word[0]) + word.Substring(1).ToLower()));
        }
        public static string SnakeToCamelCase(this string snakeCase)
        {
            var pascalCase = snakeCase.SnakeToPascalCase();
            return string.IsNullOrEmpty(pascalCase)
                ? string.Empty
                : char.ToLower(pascalCase[0]) + pascalCase.Substring(1);
        }
        public static string PascalToSnakeCase(this string pascalCase)
        {
            if (string.IsNullOrEmpty(pascalCase))
                return string.Empty;

            return string.Concat(pascalCase.Select((ch, index) =>
                char.IsUpper(ch) && index > 0
                    ? "_" + char.ToLower(ch)
                    : char.ToLower(ch).ToString()));
        }
        public static string CamelToSnakeCase(this string camelCase)
        {
            return camelCase.PascalToSnakeCase();
        }
        public static string PascalToCamelCase(this string pascalCase)
        {
            if (string.IsNullOrEmpty(pascalCase))
                return string.Empty;

            return char.ToLower(pascalCase[0]) + pascalCase.Substring(1);
        }
        public static string CamelToPascalCase(this string camelCase)
        {
            if (string.IsNullOrEmpty(camelCase))
                return string.Empty;

            return char.ToUpper(camelCase[0]) + camelCase.Substring(1);
        }
        public static string AllToLower(this string nameCase)
        {
            if (string.IsNullOrEmpty(nameCase))
                return string.Empty;

            return nameCase.Replace("_", "").ToLower();
        }
    }
}
