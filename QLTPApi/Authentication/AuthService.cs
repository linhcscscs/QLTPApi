using DataAccess.Helper.AuthHelper;
using DataAccess.Helper.ConfigHelper;
using DataAccess.Helper.ControllerHelper.Models;
using DataAccess.Helper.ControllerHelper.Values;
using DataAccess.SQL.QLTP.Repository;
using DataAccess.Values;
using QLTPApi.Authentication.Models;
using QLTPApi.Authentication.Values;
using System.Security.Claims;
using DataAccess.Helper.Extensions;
using LoginRequest = QLTPApi.Authentication.Models.LoginRequest;
using DataAccess.SQL.QLTP.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace QLTPApi.Authentication
{
    public interface IAuthService
    {
    }
    public class AuthService : IAuthService
    {
        #region Contructor
        public AuthService(IAuthContext authContext,
            INguoiDungRepository nguoiDungRepository,
            ITruongRepository truongRepository,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _authContext = authContext;
            _nguoiDungRepository = nguoiDungRepository;
            _truongRepository = truongRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }
        private IAuthContext _authContext;
        private INguoiDungRepository _nguoiDungRepository;
        private ITruongRepository _truongRepository;
        private IRefreshTokenRepository _refreshTokenRepository;
        #endregion
        #region Method
        public ReturnCode Login(LoginRequest model)
        {
            var ret = new ReturnCode();
            var hashedPassword = AuthHelper.MD5(model.PASSWORD);
            var nguoiDung = _nguoiDungRepository.getLoginTruong(_authContext.QLTPWorkingConnection, model.USERNAME, model.PASSWORD, model.MA_SO_GD, model.MA_TRUONG, SysCapDonVi.Truong);
            var truong = _truongRepository.getByMaBasic(_authContext.QLTPWorkingConnection, _authContext.Sys_Profile.SysTem_Nam_Hoc, model.MA_TRUONG);
            if (nguoiDung == null || truong == null)
            {
                ret = new ReturnCode(EReturnCode.Invalid_Credentials);
                return ret;
            }
            #region Generate token
            var token = GenerateToken(new PrepareTokenModel()
            {
                NGUOI_DUNG_ID = nguoiDung.ID,
                USER_VERSION = nguoiDung.VERSION,
                APP_VERSION = ConfigHelper.AppSettings.VERSION,
                MA_NAM_HOC = LocalApi.GetYearNow(),
                HOC_KY = LocalApi.GetKyNow(),
                MA_CAP_HOC = model.MA_CAP_HOC,
                MA_SO_GD = model.MA_SO_GD,
                ID_TRUONG = truong.ID,
                MA_TRUONG = truong.MA
            });
            #endregion
            return ret;
        }
        #endregion
        private TokenResponse GenerateToken(PrepareTokenModel model )
        {
            var tokenResponse = new TokenResponse()
            {
                NGUOI_DUNG_ID = model.NGUOI_DUNG_ID,
                APP_VERSION = model.APP_VERSION,
                HOC_KY = model.HOC_KY,
                ID_TRUONG = model.ID_TRUONG,
                MA_CAP_HOC= model.MA_CAP_HOC,
                MA_NAM_HOC= model.MA_NAM_HOC,
                MA_SO_GD = model.MA_SO_GD,
                MA_TRUONG= model.MA_TRUONG,
                USER_VERSION = model.USER_VERSION,
            };
            var claims = new ClaimsIdentity(
                [
                    new Claim(UserClaimKey.NGUOI_DUNG_ID, model.NGUOI_DUNG_ID.ToString()?? ""),
                    new Claim(UserClaimKey.USER_VERSION, model.USER_VERSION?.ToString() ?? ""),
                    new Claim(UserClaimKey.APP_VERSION, model.APP_VERSION),
                    new Claim(UserClaimKey.MA_NAM_HOC, model.MA_NAM_HOC.ToString()),
                    new Claim(UserClaimKey.HOC_KY, model.HOC_KY.ToString()),
                    new Claim(UserClaimKey.MA_CAP_HOC, model.MA_CAP_HOC),
                    new Claim(UserClaimKey.MA_SO_GD, model.MA_SO_GD),
                    new Claim(UserClaimKey.ID_TRUONG, model.ID_TRUONG.ToString()),
                    new Claim(UserClaimKey.MA_TRUONG, model.MA_TRUONG),
                ]);
            tokenResponse.ACCESS_TOKEN = new Token()
            {
                TOKEN = AuthHelper.GenerateAccessToken(claims, AuthHelper.GetExperiedDate()),
                EXPERIED_DATE = AuthHelper.GetExperiedDate()
            };
            if (!model.REMEMBER) return tokenResponse;
            tokenResponse.REFRESH_TOKEN = new Token()
            {
                TOKEN = GenerateRefreshToken(model),
                EXPERIED_DATE = AuthHelper.GetRefreshTokenExperiedDate()
            };
            return tokenResponse;
        }
        private string GenerateRefreshToken(PrepareTokenModel model)
        {
            var token = AuthHelper.GenerateRefreshToken();
            var expDate = AuthHelper.GetRefreshTokenExperiedDate();
            var refreshToken = new RefreshToken()
            {
                APP_VERSION = ConfigHelper.AppSettings.VERSION,
                EXPERIED_DATE = AuthHelper.GetRefreshTokenExperiedDate(),
                HOC_KY = model.HOC_KY,
                MA_CAP_HOC = model.MA_CAP_HOC,
                MA_NAM_HOC = model.MA_NAM_HOC,
                MA_SO_GD = model.MA_SO_GD,
                MA_TRUONG = model.MA_TRUONG,
                NGUOI_DUNG_ID = model.NGUOI_DUNG_ID
            };
            var res  = _refreshTokenRepository.Insert(_authContext.QLTPWorkingConnection, refreshToken);
            if (!res.Res)
            {
                throw new Exception();
            }
            return token;
        }
    }
}
