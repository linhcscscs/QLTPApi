using AutoMapper;
using DataAccess.Caching.Interface;
using DataAccess.SQL.QLTP.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.SQL.QLTP.Repository.BaseRepository
{
    public interface IQLTPRepository<TEntity> where TEntity : class, new()
    {
        public DbSet<TEntity> GetDbSet(QLTPConnection connection, bool isWriteEntity = false);
    }
    public abstract class QLTPRepository<TEntity> : IQLTPRepository<TEntity>
        where TEntity : class, new()
    {
        #region Contructor
        protected IQLTPContextFactory _contextFactory;
        protected ICacheProvider _cache;
        protected IMapper _mapper;
        protected IServiceProvider _serviceProvider;
        public QLTPRepository(IQLTPContextFactory contextFactory, 
            ICacheProvider cache, 
            IMapper mapper, 
            IServiceProvider serviceProvider)
        {
            _contextFactory = contextFactory;
            _cache = cache;
            _mapper = mapper;
            _serviceProvider = serviceProvider;
        }
        #endregion
        #region Method
        public DbSet<TEntity> GetDbSet(QLTPConnection connection, bool isWriteEntity = false)
            => _contextFactory.GetContext(connection, isWriteEntity).Set<TEntity>();
        public virtual void ClearCache()
            => _cache.RemoveByFirstName(typeof(TEntity).Name);
        #endregion
    }
}
