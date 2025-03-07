using DataAccess.Helper.AuthHelper;
using DataAccess.Helper.ConfigHelper;
using DataAccess.Helper.ControllerHelper.Models;
using Microsoft.AspNetCore.Identity.Data;
using QLTPApi.Authentication.Values;
using System.Security.Claims;

namespace QLTPApi.Authentication
{
    public interface IAuthService
    {

    }
    public class AuthService : IAuthService
    {
        #region Contructor
        public AuthService(IAuthContext authContext)
        {
            _authContext = authContext;
        }
        private IAuthContext _authContext;
        #endregion
        #region Method
        public ReturnCode Login(LoginRequest model)
        {
            var ret = new ReturnCode();
            var tokenResponse = new TokenResponse();
            var baseConnection = new PhoCapGDBaseConnection(ConfigHelper.NamHoc, model.ma_tinh);
            var nguoiDung = _nguoiDungRepository.GetNguoiDungByUserAndPassword(baseConnection, model.ma_tinh, model.ma_huyen, model.ma_xa, model.username, model.password);
            if (nguoiDung == null)
            {
                ret = new ReturnCode()
                {
                    status = 0,
                    message = "Tài khoản hoặc mật khẩu không đúng",
                    status_code = 401
                };
                return ret;
            }

            #region Generate access token
            var claims = new ClaimsIdentity(
                [
                    new Claim(UserClaimKey.NguoiDungId, nguoiDung.ID.ToString()),
                    new Claim(UserClaimKey.Version, nguoiDung.VERSION?.ToString() ?? ""),
                    new Claim(UserClaimKey.MA_TINH, model.ma_tinh),
                    new Claim(UserClaimKey.MA_HUYEN, model.ma_huyen),
                    new Claim(UserClaimKey.MA_XA, model.ma_xa),
                    new Claim(UserClaimKey.MA_NAM_HOC, ConfigHelper.NamHoc.ToString()),
                    new Claim(UserClaimKey.App_Version, ConfigHelper.ConfigValue.Version)
                ]);

            var expDate = model.remember ? AuthHelper.GetRememberExperiedDate() : AuthHelper.GetExperiedDate();
            var accessToken = AuthHelper.GenerateAccessToken(claims, expDate);
            #endregion

            tokenResponse.experied_time = AuthHelper.GetExperiedDate();
            tokenResponse.version = nguoiDung.VERSION;

            tokenResponse.access_token = accessToken;
            tokenResponse.ma_nam_hoc = ConfigHelper.NamHoc;
            tokenResponse.ma_tinh = model.ma_tinh;
            tokenResponse.ma_huyen = model.ma_huyen;
            tokenResponse.ma_xa = model.ma_xa;
            tokenResponse.ten_hien_thi = nguoiDung.TEN_HIEN_THI ?? nguoiDung.TEN_DANG_NHAP;
            tokenResponse.is_root = nguoiDung.IS_ROOT == 1;
            tokenResponse.is_root_sys = nguoiDung.IS_ROOT_SYS == 1;

            // Log
            var httpContextAccessor = _authContext.HttpContextAccessor;
            _logSYSRepository.Save(new LogSYS()
            {
                MA_NAM_HOC = _authContext.Sys_Profile.MA_NAM_HOC,
                MA_TINH = model.ma_tinh,
                MA_HUYEN = model.ma_huyen,
                MA_XA = model.ma_xa,
                NGAY_TAO = DateTime.Now,
                HANH_DONG = LogType.LOG_IN,
                TEN_BANG = "",
                GHI_CHU = "Đăng nhập",
                NGUOI_TAO = model.username,
                IP = httpContextAccessor.GetIPAddress(),
                URL = httpContextAccessor.GetURL(),
                USER_AGENT = httpContextAccessor.GetUserAgent()
            });
            ret.data = tokenResponse;
            return ret;
        }
        #endregion
    }
}
