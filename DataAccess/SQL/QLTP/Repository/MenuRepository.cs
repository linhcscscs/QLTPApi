using AutoMapper;
using DataAccess.Caching;
using DataAccess.Caching.Interface;
using DataAccess.SQL.QLTP.Context;
using DataAccess.SQL.QLTP.Models;
using DataAccess.SQL.QLTP.Repository.BaseRepository;
using Force.DeepCloner;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAccess.SQL.QLTP.Repository
{
    public interface IMenuRepository : IQLTPRepository<Menu>
    {
        public Menu? getByID(QLTPConnection connection, long id);
        public List<Menu> getAll(QLTPConnection connection, string lg = "VI");
        public List<Menu> getByLink(QLTPConnection connection, string url);
        public List<Menu> getByGroupUserID(QLTPConnection connection, long idNhomQuyen, string lg = "VI");
    }
    class MenuRepository : QLTPRepository<Menu>, IMenuRepository
    {
        public MenuRepository(IQLTPContextFactory contextFactory, ICacheProvider cache, IMapper mapper, IServiceProvider serviceProvider) : base(contextFactory, cache, mapper, serviceProvider)
        {
        }
        public Menu? getByID(QLTPConnection connection, long id)
        {
            return _cache.GetByKey(
                getDataSource: () =>
                {
                    using (var context = _contextFactory.GetContext(connection, false))
                    {
                        return (from p in context.Menu
                                where p.MenuID == id
                                select p).FirstOrDefault();
                    }
                },
                key: _cache.BuildCachedKey("Menu", "getByID", connection.ma_nam_hoc, id),
                cacheTime: 300000
            );
        }
        public List<Menu> getAll(QLTPConnection connection, string lg = "VI")
        {
            return _cache.GetByKey(
                    getDataSource: () =>
                    {
                        using (var context = _contextFactory.GetContext(connection, false))
                        {
                            return (from p in context.Menu
                                    orderby p.Order
                                    select p).ToList().Select(c =>
                                    {
                                        c.MenuNameView = ((lg == "VI" || string.IsNullOrEmpty(c.MenuNameEG))
                                        ? c.MenuName : c.MenuNameEG);
                                        return c;
                                    }).ToList();
                        }
                    },
                    key: _cache.BuildCachedKey("Menu", "getAll", connection.ma_nam_hoc, lg),
                    cacheTime: 300000
                ) ?? new List<Menu>();
        }
        public List<Menu> getByLink(QLTPConnection connection, string url)
        {
            return _cache.GetByKey(
                    getDataSource: () =>
                    {

                        List<Menu> listMenu = new List<Menu>();
                        listMenu = getAll(connection);
                        return listMenu.Where(c => c.Link != null && c.Link.ToUpper().Trim().Replace("~/", "") == url.ToUpper().Trim().Replace("~/", "")).ToList();
                    },
                    key: _cache.BuildCachedKey("Menu", "getByLink", connection.ma_nam_hoc, url),
                    cacheTime: 300000
                ) ?? new List<Menu>();
        }
        public List<Menu> getByGroupUserID(QLTPConnection connection, long idNhomQuyen, string lg = "VI")
        {
            return _cache.GetByKey(
                    getDataSource: () =>
                    {
                        using (var context = _contextFactory.GetContext(connection, false))
                        {
                            string strQuery = @"select * from MENU 
                                        where exists (select * from GroupUserMenu 
                                                      join GroupUser on GroupUserMenu.GroupUserID=GroupUser.GroupUserID
			                                          where GroupUser.Status=1 and MENU.MenuID=GroupUserMenu.MenuID and GroupUserMenu.GroupUserID={0} AND GroupUserMenu.IsView=1)
                                        and Menu.Status=1";
                            var data = context.Database.SqlQueryRaw<Menu>(strQuery, idNhomQuyen).ToList();
                            data.Select(c =>
                            {
                                c.MenuNameView = ((lg == "VI" || string.IsNullOrEmpty(c.MenuNameEG))
                                ? c.MenuName : c.MenuNameEG);
                                return c;
                            }).ToList();
                            return data;
                        }
                    },
                    key: _cache.BuildCachedKey("Menu", "GroupUser", "GroupUserMenu", "getByGroupUserID", connection.ma_nam_hoc, idNhomQuyen, lg),
                    cacheTime: 300000
                ) ?? new List<Menu>();
        }
    }
}
