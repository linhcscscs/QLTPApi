using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helper.ControllerHelper.Models
{
    public class ReturnCode
    {
        public ReturnCode() { }
        public ReturnCode(bool isSuccess)
        {
            status = isSuccess ? 1 : 0;
            message = isSuccess ? "" : "Có lỗi xảy ra";
        }
        public int status { get; set; } = 1;
        public string message { get; set; } = "";
        public string error_code { get; set; } = "";
        public int? status_code { get; set; } = null;
        public object? data { get; set; } = new object();
    }
}
