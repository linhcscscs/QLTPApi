using AutoMapper;
using DataAccess.Caching;
using DataAccess.Caching.Interface;
using DataAccess.Models;
using DataAccess.SQL.QLTP.Context;
using DataAccess.SQL.QLTP.Repository.BaseRepository;
using DataAccess.Values;
using Force.DeepCloner;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SQL.QLTP.Repository
{
    public interface INguoiDungRepository : IQLTPRepository<NGUOI_DUNG>
    {
        public NGUOI_DUNG? GetByID(QLTPConnection connection, decimal id);
        public NGUOI_DUNG? getLoginTruong(QLTPConnection connection, string userName, string password, string so, string truong, string cap_don_vi);
        public NGUOI_DUNG? getByTKTruong(QLTPConnection connection, string userName, string so, string truong, string cap_don_vi);
        public NGUOI_DUNG? GetByNguoiDungBasic(QLTPConnection connection, decimal id);
    }
    public class NguoiDungRepository : QLTPRepository<NGUOI_DUNG>, INguoiDungRepository
    {
        public NguoiDungRepository(IQLTPContextFactory contextFactory, ICacheProvider cache, IMapper mapper, IServiceProvider serviceProvider) : base(contextFactory, cache, mapper, serviceProvider)
        {
        }
        #region Gets
        public NGUOI_DUNG? GetByID(QLTPConnection connection, decimal id)
        {
            return _cache.GetByKey(
                getDataSource: () =>
                {
                    using (var context = _contextFactory.GetContext(connection, false))
                    {
                        return context.NGUOI_DUNG.FirstOrDefault(p => p.ID == id);
                    }
                },
                key: _cache.BuildCachedKey("NGUOI_DUNG", "GetByID", connection.ma_nam_hoc, id),
                cacheTime: CachingTime.CACHING_TIME_DEFAULT_IN_1_MINUTES
            );
        }

        public NGUOI_DUNG? getLoginTruong(QLTPConnection connection, string userName, string password, string so, string truong, string cap_don_vi)
        {
            return _cache.GetByKey(
                getDataSource: () =>
                {
                    using (var context = _contextFactory.GetContext(connection, false))
                    {
                        string? maSo = (so == "-1") ? null : so;
                        return context.NGUOI_DUNG.FirstOrDefault(p =>
                            p.TEN_DANG_NHAP == userName &&
                            p.MAT_KHAU == password &&
                            p.TRANG_THAI == 1 &&
                            ((p.IS_ROOT_SYS != null && p.IS_ROOT_SYS.Value == 1) ||
                            (p.MA_CAP_DON_VI == cap_don_vi && p.MA_SO_GD == maSo && p.MA_TRUONG == truong)));
                    }
                },
                key: _cache.BuildCachedKey("NGUOI_DUNG", "getLoginTruong", connection.ma_nam_hoc, userName, password, so, truong, cap_don_vi),
                cacheTime: CachingTime.CACHING_TIME_DEFAULT_IN_5_MINUTES
            );
        }

        public NGUOI_DUNG? getByTKTruong(QLTPConnection connection, string userName, string so, string truong, string cap_don_vi)
        {
            return _cache.GetByKey(
                getDataSource: () =>
                {
                    using (var context = _contextFactory.GetContext(connection, false))
                    {
                        string maSo = (so == "-1") ? null : so;
                        return context.NGUOI_DUNG.FirstOrDefault(p =>
                            p.TEN_DANG_NHAP == userName &&
                            p.TRANG_THAI == 1 &&
                            ((p.IS_ROOT_SYS != null && p.IS_ROOT_SYS.Value == 1) ||
                            (p.MA_CAP_DON_VI == cap_don_vi && p.MA_SO_GD == maSo && p.MA_TRUONG == truong)));
                    }
                },
                key: _cache.BuildCachedKey("NGUOI_DUNG", "getByTKTruong", connection.ma_nam_hoc, userName, so, truong, cap_don_vi),
                cacheTime: CachingTime.CACHING_TIME_DEFAULT_IN_5_MINUTES
            );
        }

        public NGUOI_DUNG? GetByNguoiDungBasic(QLTPConnection connection, decimal id)
        {
            return _cache.GetByKey(
                getDataSource: () =>
                {
                    using (var context = _contextFactory.GetContext(connection, false))
                    {
                        return context.NGUOI_DUNG.AsNoTracking().FirstOrDefault(p => p.ID == id);
                    }
                },
                key: _cache.BuildCachedKey("NGUOI_DUNG", "GetByNguoiDungBasic", connection.ma_nam_hoc, id),
                cacheTime: CachingTime.CACHING_TIME_DEFAULT_IN_5_MINUTES
            );
        }
        #endregion

    }
}
