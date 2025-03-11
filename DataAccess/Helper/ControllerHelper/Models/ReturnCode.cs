using DataAccess.Helper.ControllerHelper.Values;
using DataAccess.Helper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helper.ControllerHelper.Models
{
    public class ReturnCode
    {
        private ErrorCode? _eErrorCode;
        private string? _errorCode = null;
        private string? _errorMsg = null;
        private bool? _isSuccess = null;
        public ReturnCode(ErrorCode returnCode)
        {
            _eErrorCode = returnCode;
        }
        public ReturnCode(bool isSuccess = true) {
            _isSuccess = isSuccess;
        }
        public ErrorCode? errorCode
        {
            set
            {
                _eErrorCode = value;
            }
        }

        public bool Success
        {
            get
            {
                return _eErrorCode == null && string.IsNullOrEmpty(ErrorCode) && _isSuccess != true;
            }
        }
        public string ErrorCode
        {
            get
            {
                return _errorCode ?? _eErrorCode?.ToString() ?? "";
            }
            set
            {
                _errorCode = value;
            }
        }
        public string ErrorMsg
        {
            get
            {
                return _errorMsg ?? _eErrorCode?.GetDescription() ?? "";
            }
            set
            {
                _errorMsg = value;
            }
        }
        public int StatusCode => Success ? 200 : _eErrorCode?.GetStatusCode() ?? 500;
        public object Data { get; set; }
    }
}
