using AutoMapper;
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
    public interface IGroupUserRepository : IQLTPRepository<GroupUser>
    {
    }
    class GroupUserRepository : QLTPRepository<GroupUser>, IGroupUserRepository
    {
        public GroupUserRepository(IQLTPContextFactory contextFactory, ICacheProvider cache, IMapper mapper, IServiceProvider serviceProvider) : base(contextFactory, cache, mapper, serviceProvider)
        {
        }
    }
}
