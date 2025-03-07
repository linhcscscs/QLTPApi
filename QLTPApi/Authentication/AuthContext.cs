using DataAccess.Entities;
using DataAccess.Helper.AuthHelper;
using DataAccess.Helper.CommonHelper;
using DataAccess.Helper.ConfigHelper;
using DataAccess.SQL.QLTP.Context;
using DataAccess.SQL.QLTP.Models;
using DataAccess.SQL.QLTP.Repository;
using Microsoft.AspNetCore.Http;
using QLTPApi.Authentication.Models;
using QLTPApi.Authentication.Values;

namespace QLTPApi.Authentication
{
    public interface IAuthContext
    {

    }
    public class AuthContext : IAuthContext
    {
        #region Contructor
        public AuthContext(HttpContext httpContext,
            INguoiDungRepository nguoiDungRepository,
            INhanSuRepository nhanSuRepository,
            ITruongRepository truongRepository,
            ISoGDRepository soGDRepository,
            IQuyenNguoiDungRepository quyenNguoiDungRepository,
            IMenuRepository menuRepository,
            IGroupUserRepository groupUserRepository,
            IGroupUserMenuRepository groupUserMenuRepository)
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
        private IGroupUserMenuRepository _groupUserMenuRepository;
        private IGroupUserRepository _groupUserRepository;
        private IMenuRepository _menuRepository;
        private IQuyenNguoiDungRepository _quyenNguoiDungRepository;
        private INguoiDungRepository _nguoiDungRepository;
        private INhanSuRepository _nhanSuRepository;
        private ITruongRepository _truongRepository;
        private ISoGDRepository _soGDRepository;
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
                    _Sys_Profile = new SYS_Profile()
                    {
                        IsAuthenticated = true,
                        NGUOI_DUNG_ID = CommonHelper.ConvertTo<decimal>(AuthHelper.GetByClaim(user, UserClaimKey.NGUOI_DUNG_ID)),
                        USER_VERSION = CommonHelper.ConvertTo<Guid>(AuthHelper.GetByClaim(user, UserClaimKey.USER_VERSION)),
                        SysTem_Cap_Hoc = AuthHelper.GetByClaim(user, UserClaimKey.MA_CAP_HOC),
                        SysTem_Hoc_Ky = CommonHelper.ConvertTo<int>(AuthHelper.GetByClaim(user, UserClaimKey.HOC_KY), LocalApi.GetKyNow()),
                        SysTem_ID_Truong = CommonHelper.ConvertTo<decimal>(AuthHelper.GetByClaim(user, UserClaimKey.ID_TRUONG)),
                        SysTem_Nam_Hoc = CommonHelper.ConvertTo<int>(AuthHelper.GetByClaim(user, UserClaimKey.MA_NAM_HOC), ConfigHelper.AppSettings.NAM_HOC),
                        SysTem_Ma_So_GD = AuthHelper.GetByClaim(user, UserClaimKey.MA_SO_GD),
                        SysTem_Ma_Truong = AuthHelper.GetByClaim(user, UserClaimKey.MA_TRUONG),
                    };
                }
                return _Sys_Profile;
            }
        }
        public QLTPConnection WorkingConnection => new QLTPConnection(Sys_Profile.SysTem_Nam_Hoc, Sys_Profile.SysTem_Cap_Hoc);
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
                    var nd = _nguoiDungRepository.GetByNguoiDungBasic(WorkingConnection, Sys_Profile.NGUOI_DUNG_ID ?? 0);
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
                        _Sys_NhanSu = _nhanSuRepository.getByMaBasic(WorkingConnection, Sys_Profile.SysTem_Nam_Hoc, Sys_Profile.SysTem_Ma_Truong, Sys_User.MA_NHAN_SU);
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
                    _Sys_Truong = _truongRepository.getByIDBasic(WorkingConnection, Sys_Profile.SysTem_ID_Truong ?? 0);
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
                    _Sys_SoGD = _soGDRepository.getByMa(WorkingConnection, Sys_Profile.SysTem_Ma_So_GD);
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
                    _Sys_Quyen_Nguoi_Dung = _quyenNguoiDungRepository.GetByNguoiDungID(WorkingConnection, Sys_User.ID);
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
                    _Sys_Menu = _menuRepository.getByLink(WorkingConnection, path.Substring(0, path.LastIndexOf("/"))).FirstOrDefault();
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
                    _Sys_GroupUser = _groupUserRepository.getByID(WorkingConnection, Sys_Quyen_Nguoi_Dung.ID_NHOM_QUYEN);
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
                    _Sys_Group_User_Menu = _groupUserMenuRepository.getByGroupUserID(WorkingConnection, Sys_Quyen_Nguoi_Dung.ID_NHOM_QUYEN).FirstOrDefault(x => x.MenuID == Sys_Menu.MenuID);
                }
                return _Sys_Group_User_Menu;
            }
        }
    }
}
