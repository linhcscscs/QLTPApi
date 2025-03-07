using AutoMapper;
using DataAccess.Caching;
using DataAccess.Caching.Interface;
using DataAccess.SQL.QLTP.Context;
using DataAccess.SQL.QLTP.Models;
using DataAccess.SQL.QLTP.Repository.BaseRepository;
using DataAccess.Values;

namespace DataAccess.SQL.QLTP.Repository
{
    public interface ITruongRepository : IQLTPRepository<TRUONG>
    {
        public List<TRUONG> getAllActive(QLTPConnection connection, string maSo, string maPhong, string capHoc, int maNamHoc);
        public TRUONG? getByIDBasic(QLTPConnection connection, decimal id);
        public TRUONG? getByMaBasic(QLTPConnection connection, int maNamHoc, string maTruong);
        public List<TRUONG> GetListByDonVi(QLTPConnection connection, int ma_nam_hoc, string ma_cap_hoc, string ma_so_gd, string ma_phong_gd = "", string ma_truong = "");
    }
    public class TruongRepository : QLTPRepository<TRUONG>, ITruongRepository
    {
        public TruongRepository(IQLTPContextFactory contextFactory, ICacheProvider cache, IMapper mapper, IServiceProvider serviceProvider) : base(contextFactory, cache, mapper, serviceProvider)
        {
        }
        public List<TRUONG> getAllActive(QLTPConnection connection, string maSo, string maPhong, string capHoc, int maNamHoc)
        {
            return _cache.GetByKey(
                getDataSource: () =>
                {
                    var detail = new List<TRUONG>();
                    var context = _contextFactory.GetContext(connection, false);
                    var tmp = (from p in context.TRUONG
                               where p.TRANG_THAI <= 4 && p.MA_SO_GD == maSo && p.MA_NAM_HOC == maNamHoc
                               select p).ToList();
                    if (capHoc == SysCapHoc.MamNon)
                        tmp = tmp.Where(x => x.IS_CAP_MN == 1).ToList();
                    else if (capHoc == SysCapHoc.C1)
                        tmp = tmp.Where(x => x.IS_CAP_TH == 1).ToList();
                    else if (capHoc == SysCapHoc.C2)
                        tmp = tmp.Where(x => x.IS_CAP_THCS == 1).ToList();
                    else if (capHoc == SysCapHoc.C3)
                        tmp = tmp.Where(x => x.IS_CAP_THPT == 1).ToList();
                    else if (capHoc == SysCapHoc.GDTX)
                        tmp = tmp.Where(x => x.IS_CAP_GDTX == 1).ToList();

                    if (!string.IsNullOrEmpty(maPhong))
                        tmp = tmp.Where(x => x.MA_PHONG_GD == maPhong).ToList();
                    detail = tmp.OrderBy(x => x.THU_TU).ThenBy(x => x.TEN).ToList();

                    if (detail == null) detail = new List<TRUONG>();
                    return detail;
                },
                key: _cache.BuildCachedKey("TRUONG", "getAllActive", maSo, maPhong, capHoc, maNamHoc),
                cacheTime: CachingTime.CACHING_TIME_DEFAULT_IN_5_MINUTES
                ) ?? new List<TRUONG>();
        }
        public TRUONG? getByIDBasic(QLTPConnection connection, decimal id)
        {
            return _cache.GetByKey(
                getDataSource: () =>
                {
                    using (var context = _contextFactory.GetContext(connection, false))
                    {
                        return context.TRUONG.FirstOrDefault(p => p.ID == id);
                    }
                },
                key: _cache.BuildCachedKey("TRUONG", "getByIDBasic", connection.ma_nam_hoc, id),
                cacheTime: CachingTime.CACHING_TIME_DEFAULT_IN_5_MINUTES
            );
        }

        public TRUONG? getByMaBasic(QLTPConnection connection, int maNamHoc, string maTruong)
        {
            string strKeyCache = _cache.BuildCachedKey("TRUONG", "getByMaBasic", maNamHoc, maTruong);

            return _cache.GetByKey(
                getDataSource: () =>
                {
                    using (var context = _contextFactory.GetContext(connection, false))
                    {
                        return context.TRUONG.FirstOrDefault(p => p.MA_NAM_HOC == maNamHoc && p.MA == maTruong);
                    }
                },
                key: strKeyCache,
                cacheTime: CachingTime.CACHING_TIME_DEFAULT_IN_5_MINUTES
            );
        }

        public List<TRUONG> GetListByDonVi(QLTPConnection connection, int ma_nam_hoc, string ma_cap_hoc, string ma_so_gd, string ma_phong_gd = "", string ma_truong = "")
        {
            return _cache.GetByKey(
                getDataSource: () =>
                {
                    using (var context = _contextFactory.GetContext(connection, false))
                    {
                        var query = context.TRUONG.Where(th => th.MA_NAM_HOC == ma_nam_hoc && th.MA_SO_GD == ma_so_gd && th.TRANG_THAI <= 4);

                        if (!string.IsNullOrEmpty(ma_cap_hoc))
                        {
                            if (ma_cap_hoc == SysCapHoc.MamNon) query = query.Where(th => th.IS_CAP_MN == 1);
                            else if (ma_cap_hoc == SysCapHoc.C1) query = query.Where(th => th.IS_CAP_TH == 1);
                            else if (ma_cap_hoc == SysCapHoc.C2) query = query.Where(th => th.IS_CAP_THCS == 1);
                            else if (ma_cap_hoc == SysCapHoc.C3) query = query.Where(th => th.IS_CAP_THPT == 1);
                            else if (ma_cap_hoc == SysCapHoc.GDTX) query = query.Where(th => th.IS_CAP_GDTX == 1);
                        }

                        if (!string.IsNullOrEmpty(ma_truong)) query = query.Where(th => th.MA == ma_truong);
                        if (!string.IsNullOrEmpty(ma_phong_gd)) query = query.Where(th => th.MA_PHONG_GD == ma_phong_gd);

                        return query.OrderBy(th => th.THU_TU).ThenBy(th => th.TEN).ToList();
                    }
                },
                key: _cache.BuildCachedKey("TRUONG", "GetListByDonVi", ma_nam_hoc, ma_cap_hoc, ma_so_gd, ma_phong_gd, ma_truong),
                cacheTime: CachingTime.CACHING_TIME_DEFAULT_IN_5_MINUTES
            ) ?? new List<TRUONG>();
        }
    }
}
