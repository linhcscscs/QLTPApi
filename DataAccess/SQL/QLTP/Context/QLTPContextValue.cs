using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SQL.QLTP.Context
{
    public static class QLTPContextValue
    {
        public static readonly ReadOnlyCollection<QLTPSQLConnectEntity> ListDB =
            new ReadOnlyCollection<QLTPSQLConnectEntity>(
                new[]
    {
                new QLTPSQLConnectEntity(2022,
                    ConnectionStringReadStr01:"ConnectionQLTHUPHI_2022_R1",
                     ConnectionStringReadStr02:"ConnectionQLTHUPHI_2022_R2",
                    ConnectionStringWriteStr:"ConnectionQLTHUPHI_2022"),
                new QLTPSQLConnectEntity(2023,
                    ConnectionStringReadStr01:"ConnectionQLTHUPHI_2023_R1",
                     ConnectionStringReadStr02:"ConnectionQLTHUPHI_2023_R2",
                    ConnectionStringWriteStr:"ConnectionQLTHUPHI_2023"),
    }
    );
    }
}
