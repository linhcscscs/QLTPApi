using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccess.Helper.Extensions
{
    public static class StringExtensions
    {
        public static string NullIfWhiteSpace(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) { return null; }
            return value;
        }
        public static string RemoveInjection(this string value)
        {
            if (!string.IsNullOrEmpty(value))
                return value.Trim().Normalize(NormalizationForm.FormKC).Replace("'", "").Replace("--", "");
            return value;
        }
        public static string ToNormalize(this string value)
        {
            if (!string.IsNullOrEmpty(value))
                return value.Trim().Normalize(NormalizationForm.FormKC);
            return value;
        }
        public static string ToNormalizeLower(this string value)
        {
            if (!string.IsNullOrEmpty(value))
                return value.Trim().ToLower().Normalize(NormalizationForm.FormKC);
            return value;
        }
        public static string ToNormalizeLowerRelace(this string value)
        {
            if (!string.IsNullOrEmpty(value))
                return value.Trim().ToLower().Normalize(NormalizationForm.FormKC).Replace(" ", "");
            return value;
        }
        public static string RemoveUnicode(this string text, string specialTextReplace = " ")
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder(text);
            for (int i = 32; i < 48; i++)
            {
                stringBuilder.Replace(((char)i).ToString(), specialTextReplace);
            }

            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = stringBuilder.ToString().Normalize(NormalizationForm.FormD);
            var returnValue = regex.Replace(strFormD, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').Replace(" ", "");
            if (returnValue != null)
                returnValue = returnValue.Replace(" ", null);
            return returnValue;
        }
        public static string GetExceptionMessages(this Exception e, string msgs = "")
        {
            if (e == null) return string.Empty;
            if (msgs == "") msgs = e.Message;
            if (e.InnerException != null)
                msgs += "\r\nInnerException: " + e.InnerException.GetExceptionMessages();
            return msgs;
        }

        public static string Compress(string text)
        {
            byte[] gzBuffer = null;
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                MemoryStream ms = new MemoryStream();
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    zip.Write(buffer, 0, buffer.Length);
                }

                ms.Position = 0;
                MemoryStream outStream = new MemoryStream();

                byte[] compressed = new byte[ms.Length];
                ms.Read(compressed, 0, compressed.Length);

                gzBuffer = new byte[compressed.Length + 4];
                Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
                Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Convert.ToBase64String(gzBuffer);
        }
        public static string Decompress(string compressedText)
        {
            byte[] gzBuffer = Convert.FromBase64String(compressedText);
            using (MemoryStream ms = new MemoryStream())
            {
                int msgLength = BitConverter.ToInt32(gzBuffer, 0);
                ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

                byte[] buffer = new byte[msgLength];

                ms.Position = 0;
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    zip.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

        public static object TrySetPropValueStatic(object src, string propName, object value)
        {
            if (src.GetType().GetProperty(propName) != null && src.GetType().GetProperty(propName).CanWrite)
            {
                src.GetType().GetProperty(propName).SetValue(src, value, null);
            }

            return src;
        }

        public static object TryGetPropValueStatic(object src, string propName)
        {
            if (src.GetType().GetProperty(propName) != null && src.GetType().GetProperty(propName).CanRead)
                return src.GetType().GetProperty(propName).GetValue(src, null);
            return null;
        }
        public static T CloneFullEntity<T>(this object source)
        {
            T result = Activator.CreateInstance<T>();

            var srcFields = source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
            foreach (var property in srcFields)
            {
                var value = TryGetPropValueStatic(source, property.Name);
                TrySetPropValueStatic(result, property.Name, value);
            }

            return result;
        }
    }
}
