using AutoMapper;
using DataAccess.Caching;
using DataAccess.Caching.Interface;
using DataAccess.SQL.QLTP.Context;
using DataAccess.SQL.QLTP.Models;
using DataAccess.SQL.QLTP.Repository.BaseRepository;
using Force.DeepCloner;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DataAccess.SQL.QLTP.Repository
{
    public interface IGroupUserRepository : IQLTPRepository<GroupUser>
    {
        public GroupUser? getByID(QLTPConnection connection, long id);
        public List<GroupUser> getAllActive(QLTPConnection connection, string groupUserName = "");
        public List<GroupUser> getAll(QLTPConnection connection, string soMa, string truongMa, string groupUserCode = "", string groupUserName = "");
    }
    class GroupUserRepository : QLTPRepository<GroupUser>, IGroupUserRepository
    {
        public GroupUserRepository(IQLTPContextFactory contextFactory, ICacheProvider cache, IMapper mapper, IServiceProvider serviceProvider) : base(contextFactory, cache, mapper, serviceProvider)
        {
        }
        public GroupUser? getByID(QLTPConnection connection, long id)
        {
            return _cache.GetByKey(
                    getDataSource: () =>
                    {
                        using (var context = _contextFactory.GetContext(connection, false))
                        {
                            return (from p in context.GroupUser
                                    where p.GroupUserID == id
                                    select p).FirstOrDefault();
                        }
                    },
                    key: _cache.BuildCachedKey("GroupUser", "getByID", connection.ma_nam_hoc, id),
                    cacheTime: 300000
                );
        }
        public List<GroupUser> getAllActive(QLTPConnection connection, string groupUserName = "")
        {
            return _cache.GetByKey(
                        getDataSource: () =>
                        {
                            using (var context = _contextFactory.GetContext(connection, false))
                            {
                                return (from p in context.GroupUser
                                        where p.Status == 1
                                        select p).Where(x => x.GroupUserName.Contains(groupUserName)).ToList();
                            }
                        },
                        key: _cache.BuildCachedKey("GroupUser", "getAllActive", connection.ma_nam_hoc, groupUserName),
                        cacheTime: 300000
                    ) ?? new List<GroupUser>();
        }
        public List<GroupUser> getAll(QLTPConnection connection, string soMa, string truongMa, string groupUserCode = "", string groupUserName = "")
        {
            return _cache.GetByKey(
                        getDataSource: () =>
                        {
                            using (var context = _contextFactory.GetContext(connection, false))
                            {
                                return (from p in context.GroupUser
                                        where p.Status == 1
                                        select p).Where(x => x.GroupUserName.Contains(groupUserName)).ToList();
                            }
                        },
                        key: _cache.BuildCachedKey("GroupUser", "getAll", connection.ma_nam_hoc, soMa, truongMa, groupUserCode, groupUserName),
                        cacheTime: 300000
                    ) ?? new List<GroupUser>();
        }
    }
}
