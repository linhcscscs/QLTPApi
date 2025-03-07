using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helper.ControllerHelper.Values
{
    public static class ErrorCode
    {
        #region Auth
        [Description("AccessToken hết hạn")]
        public static string Expired_Token = "Expired_Token";
        [Description("AccessToken không hợp lệ")]
        public static string Invalid_Token = "Invalid_Token";
        #endregion

        #region Permission
        [Description("Bạn không được quyền sử dụng tính năng này")]
        public static string Forbidden = "Forbidden";
        #endregion
    }
}
