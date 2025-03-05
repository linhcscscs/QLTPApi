using DataAccess.Models;
using DataAccess.Values;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.SQL.QLTP.Context
{
    public interface IQLTPContextFactory
    {
        QUAN_LY_THU_PHI_2025Context GetContext(QLTPConnection connection, bool isWriteEntity = false);
        public string GetReadWriteConnectionString(QLTPConnection connection, bool isWriteEntity = false);
    }
    public class QLTPContextFactory : IQLTPContextFactory
    {
        public string GetReadWriteConnectionString(QLTPConnection connection, bool isWriteEntity = false)
        {
            var configDatabase = QLTPContextValue.ListDB.FirstOrDefault(c => c.MA_NAM_HOC == connection.ma_nam_hoc);
            if (configDatabase == null) configDatabase = QLTPContextValue.ListDB.FirstOrDefault();
            if (configDatabase == null) return "";
            if (isWriteEntity)
                return configDatabase.SQL_CONNECTION_STRING_WRITE;
            else
            {
                if (connection.ma_cap_hoc == SysCapHoc.C1 || connection.ma_cap_hoc == SysCapHoc.C2)
                {
                    if (!string.IsNullOrEmpty(configDatabase.SQL_CONNECTION_STRING_READ_02))
                        return configDatabase.SQL_CONNECTION_STRING_READ_02;
                }
                return configDatabase.SQL_CONNECTION_STRING_READ_01;
            }
        }
        public QUAN_LY_THU_PHI_2025Context GetContext(QLTPConnection connection, bool isWriteEntity = false)
        {
            var opt = new DbContextOptions<QUAN_LY_THU_PHI_2025Context>();
            var context = new QUAN_LY_THU_PHI_2025Context(opt);
            // AutoDetectChangesEnabled = false
            context.ChangeTracker.AutoDetectChangesEnabled = isWriteEntity;
            // ProxyCreationEnabled = false
            context.ChangeTracker.LazyLoadingEnabled = false;
            context.Database.SetConnectionString(GetReadWriteConnectionString(connection));
            return context;
        }
    }
}
