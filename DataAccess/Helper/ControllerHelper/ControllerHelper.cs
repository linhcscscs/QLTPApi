using DataAccess.Helper.ControllerHelper.Models;
using DataAccess.Helper.ControllerHelper.Values;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helper.ControllerHelper
{
    public static partial class ControllerHelper
    {
        public static OkObjectResult Success(object? data = null)
        {
            var model = new ReturnCode(true) { Data = data };
            return new OkObjectResult(model);
        }
        public static ObjectResult Error(string messange = "", string error_code = "", int? status_code = null)
        {
            var model = new ReturnCode(false);
            if (!string.IsNullOrEmpty(messange)) model.ErrorMsg = messange;
            if (!string.IsNullOrEmpty(error_code)) model.ErrorCode = error_code;
            var ret = new ObjectResult(model);
            ret.StatusCode = status_code ?? 500;
            return ret;
        }
        public static ObjectResult ReturnCode(ReturnCode model)
        {
            var ret = new ObjectResult(model);
            ret.StatusCode = model.StatusCode;
            return ret;
        }
        public static ObjectResult ReturnCode(ErrorCode errorCode)
        {
            var model = new ReturnCode(errorCode);
            var ret = new ObjectResult(model);
            return ret;
        }
        public static IActionResult ReturnFile(this ControllerBase controllerBase, string filePath, string fileName, bool isDelete = true)
        {
            if (System.IO.File.Exists(filePath))
            {
                var fileOption = isDelete ? FileOptions.DeleteOnClose : FileOptions.None;
                var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None, 4096, fileOption);
                return controllerBase.File(
                        fileStream: fs,
                        contentType: System.Net.Mime.MediaTypeNames.Application.Octet,
                        fileDownloadName: fileName);
            }
            return controllerBase.NotFound();
        }
        public static IActionResult ExceptionErrorStatus500
        {
            get
            {
                return ReturnCode(new ReturnCode(ErrorCode.Internal_Error_Exception));
            }
        }
    }
}
