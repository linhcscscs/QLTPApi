using DataAccess.Helper.ConfigHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SQL.QLTP.Context
{
    public class QLTPSQLConnectEntity
    {
        #region Properties
        private int _ma_nam_hoc;
        private string _ConnectionStringReadStr01;
        private string _ConnectionStringReadStr02;
        private string _ConnectionStringWriteStr;
        #endregion
        #region Contructor
        public QLTPSQLConnectEntity(int ma_nam_hoc,
            string ConnectionStringReadStr01 = "",
            string ConnectionStringReadStr02 = "",
            string ConnectionStringWriteStr = "")
        {
            _ma_nam_hoc = ma_nam_hoc;
            _ConnectionStringReadStr01 = ConnectionStringReadStr01;
            _ConnectionStringReadStr02 = ConnectionStringReadStr02;
            _ConnectionStringWriteStr = ConnectionStringWriteStr;
        }
        #endregion
        #region GET SET
        public int MA_NAM_HOC => _ma_nam_hoc;
        public string SQL_CONNECTION_STRING_READ_01 => ConfigHelper.GetConnectionStringByName(_ConnectionStringReadStr01, _ConnectionStringWriteStr);
        public string SQL_CONNECTION_STRING_READ_02 => ConfigHelper.GetConnectionStringByName(_ConnectionStringReadStr02, _ConnectionStringWriteStr);
        public string SQL_CONNECTION_STRING_WRITE => ConfigHelper.GetConnectionStringByName(_ConnectionStringWriteStr);
        #endregion
    }
}
