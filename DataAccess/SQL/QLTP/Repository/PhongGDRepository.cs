using AutoMapper;
using DataAccess.Caching;
using DataAccess.Caching.Interface;
using DataAccess.SQL.QLTP.Context;
using DataAccess.SQL.QLTP.Models;
using DataAccess.SQL.QLTP.Repository.BaseRepository;
using DataAccess.Values;

namespace DataAccess.SQL.QLTP.Repository
{
    public interface IPhongGDRepository : IQLTPRepository<PHONG_GD>
    {
        TRUONG? getByIDBasic(QLTPConnection connection, decimal id);
        TRUONG? getByMaBasic(QLTPConnection connection, int maNamHoc, string maTruong);
        List<TRUONG> GetListByDonVi(QLTPConnection connection, int ma_nam_hoc, string ma_cap_hoc, string ma_so_gd, string ma_phong_gd = "", string ma_truong = "");
        List<PHONG_GD> GetListPhongByMaSoGD(QLTPConnection connection, string maSoGD, int maNamHoc);
    }

    public class PhongGDRepository : QLTPRepository<PHONG_GD>, IPhongGDRepository
    {
        public PhongGDRepository(IQLTPContextFactory contextFactory, ICacheProvider cache, IMapper mapper, IServiceProvider serviceProvider) : base(contextFactory, cache, mapper, serviceProvider)
        {
        }
        public TRUONG? getByIDBasic(QLTPConnection connection, decimal id)
        {
            if (id <= 0)
                return null;

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
            return _cache.GetByKey(
                getDataSource: () =>
                {
                    using (var context = _contextFactory.GetContext(connection, false))
                    {
                        return context.TRUONG.FirstOrDefault(p => p.MA_NAM_HOC == maNamHoc && p.MA == maTruong);
                    }
                },
                key: _cache.BuildCachedKey("TRUONG", "getByMaBasic", maNamHoc, maTruong),
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
        public List<PHONG_GD> GetListPhongByMaSoGD(QLTPConnection connection, string maSoGD, int maNamHoc)
        {
            return _cache.GetByKey(
                getDataSource: () =>
                {
                    using (var context = _contextFactory.GetContext(connection, false))
                    {
                        return context.PHONG_GD.Where(p => p.TRANG_THAI == 1 && p.MA_SO_GD == maSoGD && p.MA_NAM_HOC == maNamHoc)
                                               .OrderBy(p => p.THU_TU).ThenBy(p => p.TEN)
                                               .ToList();
                    }
                },
                key: _cache.BuildCachedKey("PHONG_GD", "GetListPhongByMaSoGD", maSoGD, maNamHoc),
                cacheTime: CachingTime.CACHING_TIME_DEFAULT_IN_5_MINUTES
            ) ?? new List<PHONG_GD>();
        }
    }
}
