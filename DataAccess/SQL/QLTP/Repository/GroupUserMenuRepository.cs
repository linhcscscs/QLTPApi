using AutoMapper;
using DataAccess.Caching;
using DataAccess.Caching.Interface;
using DataAccess.Entities;
using DataAccess.SQL.QLTP.Context;
using DataAccess.SQL.QLTP.Models;
using DataAccess.SQL.QLTP.Repository.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.SQL.QLTP.Repository
{
    public interface IGroupUserMenuRepository : IQLTPRepository<GroupUser>
    {
        public List<GroupUserMenu> getByGroupUserID(QLTPConnection connection, long guID);
        public List<GroupUserMenuEntity> getQuyenByGroupUserID(QLTPConnection connection, long guid);
    }
    class GroupUserMenuRepository : QLTPRepository<GroupUser>, IGroupUserMenuRepository
    {
        public GroupUserMenuRepository(IQLTPContextFactory contextFactory, ICacheProvider cache, IMapper mapper, IServiceProvider serviceProvider) : base(contextFactory, cache, mapper, serviceProvider)
        {
        }
        public List<GroupUserMenu> getByGroupUserID(QLTPConnection connection, long guID)
        {
            return _cache.GetByKey(
                getDataSource: () =>
                {
                    using (var context = _contextFactory.GetContext(connection, false))
                    {
                        return context.GroupUserMenu
                            .Join(context.GroupUser, p => p.GroupUserID, g => g.GroupUserID, (p, g) => new { p, g })
                            .Where(x => x.p.GroupUserID == guID && x.g.Status == 1)
                            .Select(x => x.p)
                            .ToList();
                    }
                },
                key: _cache.BuildCachedKey("GroupUserMenu", "GroupUser", "getByGroupUserID", connection.ma_nam_hoc, guID),
                cacheTime: CachingTime.CACHING_TIME_DEFAULT_IN_5_MINUTES
            ) ?? new List<GroupUserMenu>();
        }
        public List<GroupUserMenuEntity> getQuyenByGroupUserID(QLTPConnection connection, long guid)
        {
            return _cache.GetByKey(
                getDataSource: () =>
                {
                    using (var context = _contextFactory.GetContext(connection, false))
                    {
                        string strQuery = @"SELECT m.MenuID, m.ParentID, m.MenuCode, m.MenuName, 
                                             gum.GroupUserMenuID, gum.GroupUserID, gum.isView, gum.IsAdd, 
                                             gum.IsEdit, gum.IsDelete, gum.IsUpload, gum.IsAuth, 
                                             gum.CreateBy, gum.CreateAt, gum.UpdateBy, gum.UpdateAt, 
                                             gum.AuthBy, gum.AuthAt 
                                      FROM MENU m
                                      LEFT JOIN GroupUserMenu gum ON m.MenuID = gum.MenuID 
                                      AND gum.GroupUserID = {0}
                                      ORDER BY m.[Order]";

                        return context.Database.SqlQueryRaw<GroupUserMenuEntity>(strQuery, guid).ToList();
                    }
                },
                key: _cache.BuildCachedKey("MENU", "GroupUserMenu", "getQuyenByGroupUserID", connection.ma_nam_hoc, guid),
                cacheTime: CachingTime.CACHING_TIME_DEFAULT_IN_5_MINUTES
            ) ?? new List<GroupUserMenuEntity>();
        }
    }
}
