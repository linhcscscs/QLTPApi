using AutoMapper;
using DataAccess.Caching.Interface;
using DataAccess.Entities;
using DataAccess.Helper.ControllerHelper.Models;
using DataAccess.SQL.QLTP.Context;
using DataAccess.SQL.QLTP.Models;
using DataAccess.SQL.QLTP.Repository.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SQL.QLTP.Repository
{
    public interface IRefreshTokenRepository : IQLTPRepository<RefreshToken>
    {
        public List<RefreshToken>? GetByNguoiDung(QLTPConnection connection, decimal nguoiDungID, string appVersion, Guid? userVersion);
        public ResultEntity Insert(QLTPConnection connection, RefreshToken model);
    }
    class RefreshTokenRepository : QLTPRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(IQLTPContextFactory contextFactory, ICacheProvider cache, IMapper mapper, IServiceProvider serviceProvider) : base(contextFactory, cache, mapper, serviceProvider)
        {
        }
        #region GET
        public List<RefreshToken>? GetByNguoiDung(QLTPConnection connection, decimal nguoiDungID, string appVersion, Guid? userVersion)
        {
            return _cache.GetByKey(
                getDataSource: () =>
                {
                    using (var context = _contextFactory.GetContext(connection))
                    {
                        return context.RefreshToken.Where(x => x.NGUOI_DUNG_ID == nguoiDungID
                        && x.APP_VERSION == appVersion
                        && x.USER_VERSION == userVersion
                        && x.EXPERIED_DATE >= DateTime.Now
                        ).ToList();
                    }
                },
                key: _cache.BuildCachedKey("RefreshToken", "GetByNguoiDung", nguoiDungID, appVersion)
                );
        }
        #endregion
        #region SET
        public ResultEntity Insert(QLTPConnection connection, RefreshToken model)
        {
            ResultEntity res = new ResultEntity();
            _cache.RemoveByFirstName("RefreshToken");
            try
            {
                using (var context = _contextFactory.GetContext(connection, true))
                {
                    context.RefreshToken.Add(model);
                    context.SaveChanges();
                    res.ResObject = model;
                }
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "có lỗi xảy ra";
                return res;
            }
            return res;
        }
        #endregion
    }
}
