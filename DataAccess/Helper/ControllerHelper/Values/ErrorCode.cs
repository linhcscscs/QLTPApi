using DataAccess.Helper.AttributeHelper.AttributeCollection;
using System.ComponentModel;

namespace DataAccess.Helper.ControllerHelper.Values
{
    public enum ErrorCode
    {
        #region System
        [Description("Lỗi không xác định"), StatusCode(500)]
        Internal_Error_Exception,
        #endregion

        #region Auth
        [Description("Tài khoản hoặc mật khẩu không đúng"), StatusCode(403)]
        Invalid_Credentials,
        [Description("Phiên đăng nhập đã kết thúc, vui lòng đăng nhập lại"), StatusCode(401)]
        Invalid_Token,
        [Description("AccessToken hết hạn"), StatusCode(401)]
        Expired_Token,
        [Description("Chưa đăng nhập"), StatusCode(401)]
        Unauthorized,
        [Description("Bạn không được phép truy cập tài nguyên này"), StatusCode(403)]
        Forbidden,
        #endregion

        #region Internal

        #region File
        [Description("Hãy chọn file tải lên"), StatusCode(422)]
        NoFileUploaded,
        [Description("File tải lên không đúng định dạng"), StatusCode(422)]
        FileExtensionInvalid,
        #endregion

        #endregion

        #region Common
        [Description("Không tìm thấy tài nguyên"), StatusCode(404)]
        Not_Found,
        #endregion
    }
}
