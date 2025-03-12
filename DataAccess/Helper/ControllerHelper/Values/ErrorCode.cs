using DataAccess.Helper.AttributeHelper.AttributeCollection;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace DataAccess.Helper.ControllerHelper.Values
{
    public enum ErrorCode
    {
        #region System
        [Description("Lỗi không xác định"), StatusCode(StatusCodes.Status500InternalServerError)]
        Internal_Error_Exception,
        #endregion

        #region Auth
        [Description("Tài khoản hoặc mật khẩu không đúng"), StatusCode(StatusCodes.Status403Forbidden)]
        Invalid_Credentials,
        [Description("Phiên đăng nhập đã kết thúc, vui lòng đăng nhập lại"), StatusCode(StatusCodes.Status401Unauthorized)]
        Invalid_Token,
        [Description("AccessToken hết hạn"), StatusCode(StatusCodes.Status401Unauthorized)]
        Expired_Token,
        [Description("Chưa đăng nhập"), StatusCode(StatusCodes.Status401Unauthorized)]
        Unauthorized,
        [Description("Bạn không được phép truy cập tài nguyên này"), StatusCode(StatusCodes.Status403Forbidden)]
        Forbidden,
        #endregion

        #region Internal

        #region File
        [Description("Hãy chọn file tải lên"), StatusCode(StatusCodes.Status422UnprocessableEntity)]
        NoFileUploaded,
        [Description("File tải lên không đúng định dạng"), StatusCode(StatusCodes.Status422UnprocessableEntity)]
        FileExtensionInvalid,
        #endregion

        #endregion

        #region Common
        [Description("Không tìm thấy tài nguyên"), StatusCode(StatusCodes.Status404NotFound)]
        Not_Found,
        #endregion
    }
}
