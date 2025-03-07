using AutoMapper;
using DataAccess.Caching.Interface;
using DataAccess.Helper.CommonHelper;
using DataAccess.SQL.QLTP.Context;
using DataAccess.SQL.QLTP.Models;
using DataAccess.SQL.QLTP.Repository.BaseRepository;
using DataAccess.Values;
using Force.DeepCloner;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DataAccess.SQL.QLTP.Repository
{
    public interface IQuyenNguoiDungRepository : IQLTPRepository<QUYEN_NGUOI_DUNG>
    {
        public QUYEN_NGUOI_DUNG? GetByNguoiDungID(QLTPConnection connection, decimal id_nguoi_dung);
        public List<QUYEN_NGUOI_DUNG> getListQuyenNguoiDungByIdNhomQuyen(QLTPConnection connection, string soMa, string truongMa, long IdNhomQuyen);
    }
    class QuyenNguoiDungRepository : QLTPRepository<QUYEN_NGUOI_DUNG>, IQuyenNguoiDungRepository
    {
        public QuyenNguoiDungRepository(IQLTPContextFactory contextFactory, ICacheProvider cache, IMapper mapper, IServiceProvider serviceProvider) : base(contextFactory, cache, mapper, serviceProvider)
        {
        }
        public QUYEN_NGUOI_DUNG? GetByNguoiDungID(QLTPConnection connection, decimal id_nguoi_dung)
        {
            return _cache.GetByKey(
                    getDataSource: () =>
                    {
                        connection.ma_cap_hoc = CommonHelper.IfNull(connection.ma_cap_hoc, SysCapHoc.C2);
                        using (var context = _contextFactory.GetContext(connection, false))
                        {
                            return (from p in context.QUYEN_NGUOI_DUNG
                                    where p.ID_NGUOI_DUNG == id_nguoi_dung
                                    select p).FirstOrDefault();
                        }
                    },
                    key: _cache.BuildCachedKey("QUYEN_NGUOI_DUNG", "GetByNguoiDungID", connection.ma_nam_hoc, id_nguoi_dung)
                );
        }
        public List<QUYEN_NGUOI_DUNG> getListQuyenNguoiDungByIdNhomQuyen(QLTPConnection connection, string soMa, string truongMa, long IdNhomQuyen)
        {
            return _cache.GetByKey(
            getDataSource: () =>
            {
                connection.ma_cap_hoc = CommonHelper.IfNull(connection.ma_cap_hoc, SysCapHoc.C2);
                using (var context = _contextFactory.GetContext(connection, false))
                {
                    return (from p in context.QUYEN_NGUOI_DUNG
                            where p.MA_SO_GD == soMa && p.MA_TRUONG == truongMa && p.ID_NHOM_QUYEN == IdNhomQuyen
                            select p).ToList();
                }
            },
                key: _cache.BuildCachedKey("QUYEN_NGUOI_DUNG", "getListQuyenNguoiDungByIdNhomQuyen", connection.ma_nam_hoc, soMa, truongMa, IdNhomQuyen),
                cacheTime: 300000
            ) ?? new List<QUYEN_NGUOI_DUNG>();
        }
    }
}
