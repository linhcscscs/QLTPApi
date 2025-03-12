using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class ResultEntity
    {
        public ResultEntity()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public ResultEntity(bool res, string msg)
        {
            Res = res;
            Msg = msg;
        }

        public bool Res { get; set; } = true;
        public string Msg { get; set; }
        public object ResObject { get; set; }
        public string hoTen { get; set; }
        public string ngaySinh { get; set; }
        public string tenLop { get; set; }
    }

}
