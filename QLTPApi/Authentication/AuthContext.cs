using DataAccess.Helper.AuthHelper;
using DataAccess.Helper.CommonHelper;
using DataAccess.Helper.ConfigHelper;
using DataAccess.SQL.QLTP.Context;
using DataAccess.SQL.QLTP.Models;
using DataAccess.SQL.QLTP.Repository;
using QLTPApi.Authentication.Models;
using QLTPApi.Authentication.Values;

namespace QLTPApi.Authentication
{
    public interface IAuthContext
    {
        bool IsAuthenticated { get; }
        GroupUserMenu? Sys_Group_User_Menu { get; }
        GroupUser? Sys_GroupUser { get; }
        Menu? Sys_Menu { get; }
        NHAN_SU? Sys_NhanSu { get; }
        SYS_Profile Sys_Profile { get; }
        QUYEN_NGUOI_DUNG? Sys_Quyen_Nguoi_Dung { get; }
        SO_GD? Sys_SoGD { get; }
        TRUONG? Sys_Truong { get; }
        NGUOI_DUNG? Sys_User { get; }
        QLTPConnection QLTPWorkingConnection { get; }
        public SYS_Profile Sys_Token_Infomation { get; }
    }
    public class AuthContext : IAuthContext
    {
        #region Contructor
        public AuthContext(HttpContext httpContext,
            Lazy<INguoiDungRepository> nguoiDungRepository,
            Lazy<INhanSuRepository> nhanSuRepository,
            Lazy<ITruongRepository> truongRepository,
            Lazy<ISoGDRepository> soGDRepository,
            Lazy<IQuyenNguoiDungRepository> quyenNguoiDungRepository,
            Lazy<IMenuRepository> menuRepository,
            Lazy<IGroupUserRepository> groupUserRepository,
            Lazy<IGroupUserMenuRepository> groupUserMenuRepository)
        {
            _httpContext = httpContext;
            _nguoiDungRepository = nguoiDungRepository;
            _nhanSuRepository = nhanSuRepository;
            _truongRepository = truongRepository;
            _soGDRepository = soGDRepository;
            _quyenNguoiDungRepository = quyenNguoiDungRepository;
            _menuRepository = menuRepository;
            _groupUserRepository = groupUserRepository;
            _groupUserMenuRepository = groupUserMenuRepository;
        }
        private readonly Lazy<INguoiDungRepository> _nguoiDungRepository;
        private readonly Lazy<INhanSuRepository> _nhanSuRepository;
        private readonly Lazy<ITruongRepository> _truongRepository;
        private readonly Lazy<ISoGDRepository> _soGDRepository;
        private readonly Lazy<IQuyenNguoiDungRepository> _quyenNguoiDungRepository;
        private readonly Lazy<IMenuRepository> _menuRepository;
        private readonly Lazy<IGroupUserRepository> _groupUserRepository;
        private readonly Lazy<IGroupUserMenuRepository> _groupUserMenuRepository;
        private HttpContext _httpContext;
        private SYS_Profile? _Sys_Profile = null;
        #endregion
        #region Props
        NGUOI_DUNG? _Sys_User = null;
        NHAN_SU? _Sys_NhanSu;
        TRUONG? _Sys_Truong;
        SO_GD? _Sys_SoGD;
        QUYEN_NGUOI_DUNG? _Sys_Quyen_Nguoi_Dung;
        Menu? _Sys_Menu;
        GroupUser? _Sys_GroupUser;
        GroupUserMenu? _Sys_Group_User_Menu;
        SYS_Profile? _Sys_Token_Infomation;
        #endregion
        public bool IsAuthenticated => Sys_User != null;
        public SYS_Profile Sys_Profile
        {
            get
            {
                if (_Sys_Profile == null)
                {
                    var isAuthen = _httpContext.User?.Identity?.IsAuthenticated ?? false;
                    if (!isAuthen)
                    {
                        _Sys_Profile = new SYS_Profile()
                        {
                            IsAuthenticated = false
                        };
                        return _Sys_Profile;
                    }
                    var user = _httpContext.User!;
                    // Check app Version
                    var appVersion = user.GetByClaim(UserClaimKey.APP_VERSION);
                    if (appVersion != ConfigHelper.AppSettings.VERSION)
                    {
                        _Sys_Profile = new SYS_Profile()
                        {
                            IsAuthenticated = false
                        };
                        return _Sys_Profile;
                    }
                    _Sys_Profile = new SYS_Profile()
                    {
                        IsAuthenticated = true,
                        NGUOI_DUNG_ID = CommonHelper.ConvertTo<decimal>(user.GetByClaim(UserClaimKey.NGUOI_DUNG_ID)),
                        USER_VERSION = CommonHelper.ConvertTo<Guid>(user.GetByClaim(UserClaimKey.USER_VERSION)),
                        SysTem_Cap_Hoc = user.GetByClaim(UserClaimKey.MA_CAP_HOC),
                        SysTem_Hoc_Ky = CommonHelper.ConvertTo<int>(user.GetByClaim(UserClaimKey.HOC_KY), LocalApi.GetKyNow()),
                        SysTem_ID_Truong = CommonHelper.ConvertTo<decimal>(user.GetByClaim(UserClaimKey.ID_TRUONG)),
                        SysTem_Nam_Hoc = CommonHelper.ConvertTo<int>(user.GetByClaim(UserClaimKey.MA_NAM_HOC), ConfigHelper.AppSettings.NAM_HOC),
                        SysTem_Ma_So_GD = user.GetByClaim(UserClaimKey.MA_SO_GD),
                        SysTem_Ma_Truong = user.GetByClaim(UserClaimKey.MA_TRUONG),
                    };
                }
                return _Sys_Profile;
            }
        }
        public QLTPConnection QLTPWorkingConnection => new QLTPConnection(Sys_Profile.SysTem_Nam_Hoc, Sys_Profile.SysTem_Cap_Hoc);
        public NGUOI_DUNG? Sys_User
        {
            get
            {

                if (_Sys_User == null)
                {
                    if (!Sys_Profile.IsAuthenticated)
                    {
                        return null;
                    }
                    var nd = _nguoiDungRepository.Value.GetByNguoiDungBasic(QLTPWorkingConnection, Sys_Profile.NGUOI_DUNG_ID ?? 0);
                    if (nd?.VERSION == null || nd?.VERSION == Sys_Profile.USER_VERSION)
                    {
                        _Sys_User = nd;
                    }
                }
                return _Sys_User;
            }
        }
        public NHAN_SU? Sys_NhanSu
        {
            get
            {

                if (_Sys_NhanSu == null)
                {
                    if (Sys_User == null)
                    {
                        return null;
                    }
                    if (!string.IsNullOrEmpty(Sys_User.MA_NHAN_SU))
                    {
                        _Sys_NhanSu = _nhanSuRepository.Value.getByMaBasic(QLTPWorkingConnection, Sys_Profile.SysTem_Nam_Hoc, Sys_Profile.SysTem_Ma_Truong, Sys_User.MA_NHAN_SU);
                    }
                }
                return _Sys_NhanSu;
            }
        }
        public TRUONG? Sys_Truong
        {
            get
            {
                if (_Sys_Truong == null)
                {
                    if (Sys_User == null)
                    {
                        return null;
                    }
                    _Sys_Truong = _truongRepository.Value.getByIDBasic(QLTPWorkingConnection, Sys_Profile.SysTem_ID_Truong ?? 0);
                }
                return _Sys_Truong;
            }
        }
        public SO_GD? Sys_SoGD
        {
            get
            {
                if (_Sys_SoGD == null)
                {
                    if (Sys_User == null)
                    {
                        return null;
                    }
                    _Sys_SoGD = _soGDRepository.Value.getByMa(QLTPWorkingConnection, Sys_Profile.SysTem_Ma_So_GD);
                }
                return _Sys_SoGD;
            }
        }
        public QUYEN_NGUOI_DUNG? Sys_Quyen_Nguoi_Dung
        {
            get
            {
                if (_Sys_Quyen_Nguoi_Dung == null)
                {
                    if (Sys_User == null)
                    {
                        return null;
                    }
                    _Sys_Quyen_Nguoi_Dung = _quyenNguoiDungRepository.Value.GetByNguoiDungID(QLTPWorkingConnection, Sys_User.ID);
                }
                return _Sys_Quyen_Nguoi_Dung;
            }
        }
        public Menu? Sys_Menu
        {
            get
            {
                if (_Sys_Menu == null)
                {
                    if (Sys_User == null)
                    {
                        return null;
                    }
                    var path = _httpContext.Request.Path.Value ?? "";
                    _Sys_Menu = _menuRepository.Value.getByLink(QLTPWorkingConnection, path.Substring(0, path.LastIndexOf("/"))).FirstOrDefault();
                }
                return _Sys_Menu;
            }
        }
        public GroupUser? Sys_GroupUser
        {
            get
            {
                if (_Sys_GroupUser == null)
                {
                    if (Sys_User == null || Sys_Quyen_Nguoi_Dung == null)
                    {
                        return null;
                    }
                    _Sys_GroupUser = _groupUserRepository.Value.getByID(QLTPWorkingConnection, Sys_Quyen_Nguoi_Dung.ID_NHOM_QUYEN);
                }
                return _Sys_GroupUser;
            }
        }
        public GroupUserMenu? Sys_Group_User_Menu

        {
            get
            {
                if (_Sys_Group_User_Menu == null)
                {
                    if (Sys_User == null || Sys_Quyen_Nguoi_Dung == null || Sys_Menu == null)
                    {
                        return null;
                    }
                    _Sys_Group_User_Menu = _groupUserMenuRepository.Value.getByGroupUserID(QLTPWorkingConnection, Sys_Quyen_Nguoi_Dung.ID_NHOM_QUYEN).FirstOrDefault(x => x.MenuID == Sys_Menu.MenuID);
                }
                return _Sys_Group_User_Menu;
            }
        }
        public SYS_Profile Sys_Token_Infomation
        {
            get
            {
                if (_Sys_Token_Infomation == null)
                {
                    var jwt = _httpContext.GetAccessTokenFromHeader();
                    if (string.IsNullOrEmpty(jwt) || AuthHelper.ValidJwt(jwt, out var user))
                    {
                        _Sys_Token_Infomation = new SYS_Profile() { IsAuthenticated = false };
                        return _Sys_Token_Infomation;
                    }
                    _Sys_Token_Infomation = new SYS_Profile()
                    {
                        IsAuthenticated = true,
                        NGUOI_DUNG_ID = CommonHelper.ConvertTo<decimal>(user.GetByClaim(UserClaimKey.NGUOI_DUNG_ID)),
                        USER_VERSION = CommonHelper.ConvertTo<Guid>(user.GetByClaim(UserClaimKey.USER_VERSION)),
                        SysTem_Cap_Hoc = user.GetByClaim(UserClaimKey.MA_CAP_HOC),
                        SysTem_Hoc_Ky = CommonHelper.ConvertTo<int>(user.GetByClaim(UserClaimKey.HOC_KY), LocalApi.GetKyNow()),
                        SysTem_ID_Truong = CommonHelper.ConvertTo<decimal>(user.GetByClaim(UserClaimKey.ID_TRUONG)),
                        SysTem_Nam_Hoc = CommonHelper.ConvertTo<int>(user.GetByClaim(UserClaimKey.MA_NAM_HOC), ConfigHelper.AppSettings.NAM_HOC),
                        SysTem_Ma_So_GD = user.GetByClaim(UserClaimKey.MA_SO_GD),
                        SysTem_Ma_Truong = user.GetByClaim(UserClaimKey.MA_TRUONG),
                    };
                }
                return _Sys_Token_Infomation;
            }
        }
    }
}
