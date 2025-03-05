using AutoMapper;
using DataAccess.Caching;
using DataAccess.Caching.Interface;
using DataAccess.Models;
using DataAccess.SQL.QLTP.Context;
using DataAccess.SQL.QLTP.Repository.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SQL.QLTP.Repository
{
    public interface ISoGDRepository : IQLTPRepository<SO_GD>
    {
        SO_GD getByMa(QLTPConnection connection, string maSO_GD);
        List<SO_GD> getAllActive(QLTPConnection connection);
        List<SO_GD> getSO_GDActive(QLTPConnection connection);
    }

    class SoGDRepository : QLTPRepository<SO_GD>, ISoGDRepository
    {
        public SoGDRepository(IQLTPContextFactory contextFactory, ICacheProvider cache, IMapper mapper, IServiceProvider serviceProvider) : base(contextFactory, cache, mapper, serviceProvider)
        {
        }
        public SO_GD getByMa(QLTPConnection connection, string maSO_GD)
        {
            return _cache.GetByKey(
                getDataSource: () =>
                {
                    using (var context = _contextFactory.GetContext(connection, false))
                    {
                        return context.SO_GD.FirstOrDefault(p => p.MA == maSO_GD);
                    }
                },
                key: _cache.BuildCachedKey("SO_GD", "getByMa", connection.ma_nam_hoc, maSO_GD),
                cacheTime: CachingTime.CACHING_TIME_DEFAULT_IN_5_MINUTES
            ) ?? new SO_GD();
        }

        public List<SO_GD> getAllActive(QLTPConnection connection)
        {
            return _cache.GetByKey(
                getDataSource: () =>
                {
                    using (var context = _contextFactory.GetContext(connection, false))
                    {
                        var detail = context.SO_GD.Where(p => p.TRANG_THAI == 1)
                                                  .OrderBy(x => x.THU_TU).ThenBy(x => x.TEN)
                                                  .ToList();
                        return detail;
                    }
                },
                key: _cache.BuildCachedKey("SO_GD", "getAllActive", connection.ma_nam_hoc),
                cacheTime: CachingTime.CACHING_TIME_DEFAULT_IN_1_MINUTES
            ) ?? new List<SO_GD>();
        }

        public List<SO_GD> getSO_GDActive(QLTPConnection connection)
        {
            return _cache.GetByKey(
                getDataSource: () =>
                {
                    using (var context = _contextFactory.GetContext(connection, false))
                    {
                        return context.SO_GD.Where(p => p.TRANG_THAI == 1)
                                            .OrderBy(p => p.THU_TU)
                                            .ToList();
                    }
                },
                key: _cache.BuildCachedKey("SO_GD", "getSO_GDActive", connection.ma_nam_hoc),
                cacheTime: CachingTime.CACHING_TIME_DEFAULT_IN_1_MINUTES
            ) ?? new List<SO_GD>();
        }

    }
}
