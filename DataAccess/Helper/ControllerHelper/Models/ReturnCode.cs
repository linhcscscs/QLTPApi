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
        private EReturnCode? _eReturnCode;
        private string? _errorCode = null;
        private string? _errorMsg = null;
        private bool? _isSuccess = null;
        public ReturnCode(EReturnCode returnCode)
        {
            _eReturnCode = returnCode;
        }
        public ReturnCode(bool isSuccess = true) {
            _isSuccess = isSuccess;
        }
        public EReturnCode? EReturnCode
        {
            set
            {
                _eReturnCode = value;
            }
        }

        public bool Success
        {
            get
            {
                return _eReturnCode == null && string.IsNullOrEmpty(ErrorCode) && _isSuccess != true;
            }
        }
        public string ErrorCode
        {
            get
            {
                return _errorCode ?? _eReturnCode?.ToString() ?? "";
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
                return _errorMsg ?? _eReturnCode?.GetDescription() ?? "";
            }
            set
            {
                _errorMsg = value;
            }
        }
        public int StatusCode => Success ? 200 : _eReturnCode?.GetStatusCode() ?? 500;
        public object Data { get; set; }
    }
}
