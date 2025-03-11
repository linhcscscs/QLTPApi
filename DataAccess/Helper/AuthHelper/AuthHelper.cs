using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static DataAccess.Helper.ConfigHelper.ConfigHelper;

namespace DataAccess.Helper.AuthHelper
{
    public static class AuthHelper
    {
        public static string MD5(string value)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(value);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] result = md5.ComputeHash(bytes);
                for (int i = 0; i < result.Length; i++)
                {
                    sb.Append(result[i].ToString("x2"));
                }
            }
            catch
            { return value; }
            return sb.ToString();
        }
        public static string GetByClaim(this ClaimsPrincipal principal, string claimName)
        {
            return principal.FindFirst(claimName)?.Value ?? "";
        }
        private static byte[] GetKey(string keyString)
        {
            byte[] key = new byte[16];
            for (int i = 0; i < 16; i += 2)
            {
                byte[] unicodeBytes = BitConverter.GetBytes(keyString[i % keyString.Length]);
                Array.Copy(unicodeBytes, 0, key, i, 2);
            }
            return key;
        }
        public static string EncryptString(string text, string keyString)
        {
            var key = GetKey(keyString);
            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }
        public static string DecryptString(string cipherText, string keyString)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = GetKey(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }
        public static string? getCookie(this IHttpContextAccessor httpContextAccessor, string cookieName, string secret)
        {
            string? result = null;
            if (string.IsNullOrEmpty(httpContextAccessor?.HttpContext?.Request.Cookies[cookieName]))
            {
                return result;
            }
            try
            {
                result = DecryptString(httpContextAccessor?.HttpContext?.Request.Cookies[cookieName] ?? "", secret);
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;
        }
        public static void setCookie(this IHttpContextAccessor httpContextAccessor, string cookieName, string setValue, string secret, double expAfterMinute)
        {
            var encryptedValue = EncryptString(setValue, secret);

            var cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.UtcNow.AddMinutes(expAfterMinute);
            cookieOptions.Path = "/";
            cookieOptions.Secure = true;
            cookieOptions.HttpOnly = true;

            httpContextAccessor?.HttpContext?.Response.Cookies.Append(cookieName, encryptedValue, cookieOptions);
        }
        public static void clearCookie(this IHttpContextAccessor httpContextAccessor)
        {
            foreach (var cookie in httpContextAccessor?.HttpContext?.Request?.Cookies?.Keys ?? new List<string>())
            {
                httpContextAccessor?.HttpContext?.Response.Cookies.Delete(cookie);
            }
        }
        public static DateTime GetExperiedDate() => DateTime.UtcNow.AddMinutes(JWTSettings.TokenValidityInMinutes);
        public static DateTime GetRefreshTokenExperiedDate() => DateTime.UtcNow.AddMinutes(JWTSettings.RefreshTokenValidityInMinutes);
        public static string GenerateAccessToken(ClaimsIdentity claims, DateTime? expired_date = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JWTSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = JWTSettings.ValidIssuer,
                Audience = JWTSettings.ValidAudience,
                Subject = claims,
                Expires = expired_date ?? GetExperiedDate(),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var refreshToken = Convert.ToBase64String(randomNumber);
            return refreshToken;
        }
        public static string? GetAccessTokenFromHeader(this HttpContext httpContext)
        {
            var authorizationHeader = httpContext.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                return authorizationHeader["Bearer ".Length..]; // Cắt bỏ "Bearer " để lấy token
            }
            return null;
        }
        public static Dictionary<string, string> DecodeJwt(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
        }
        public static bool ValidJwt(string token, out ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = new ClaimsPrincipal();
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = GetTokenValidationParameters();
                claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,

                ValidAudience = JWTSettings.ValidAudience,
                ValidIssuer = JWTSettings.ValidIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSettings.Secret!))
            };
        }
    }
}
