using DataAccess.Helper.Common;
using DataAccess.Helper.ControllerHelper.Values;
using DataAccess.Helper.Extensions;

namespace DataAccess.Helper.ControllerHelper.Models
{
    public class ReturnCode
    {
        private ErrorCode? _errorCode;
        private string? _errorCodeMsg = null;
        private string? _errorMsg = null;
        private bool? _isSuccess = null;
        public ReturnCode(ErrorCode returnCode)
        {
            _errorCode = returnCode;
        }
        public ReturnCode(bool isSuccess = true)
        {
            _isSuccess = isSuccess;
        }
        public bool Success
        {
            get
            {
                return (_errorCode == null && string.IsNullOrEmpty(ErrorCode)) || _isSuccess == false;
            }
        }
        public string ErrorCode
        {
            get
            {
                return CommonHelper.IfNull(_errorCodeMsg, _errorCode?.ToString() ?? "");
            }
            set
            {
                _errorCodeMsg = value;
            }
        }
        public string ErrorMsg
        {
            get
            {
                return CommonHelper.IfNull(_errorMsg, _errorCode?.GetDescription() ?? "");
            }
            set
            {
                _errorMsg = value;
            }
        }
        public int StatusCode => Success ? 200 : _errorCode?.GetStatusCode() ?? 500;
        public object Data { get; set; }
    }
}
