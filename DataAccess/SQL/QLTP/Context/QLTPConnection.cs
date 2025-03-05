using DataAccess.Helper.ConfigHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SQL.QLTP.Context
{
    public class QLTPConnection
    {
        public QLTPConnection() { }
        public QLTPConnection(int ma_nam_hoc, string ma_cap_hoc)
        {
            this.ma_nam_hoc = ma_nam_hoc;
            this.ma_cap_hoc = ma_cap_hoc;
        }
        public int ma_nam_hoc { get; set; } = ConfigHelper.AppSettings.NAM_HOC;
        public string ma_cap_hoc { get; set; } = "";
    }
}
