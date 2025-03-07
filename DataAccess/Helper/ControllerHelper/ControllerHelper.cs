using DataAccess.Helper.ControllerHelper.Models;
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
            var model = new ReturnCode(true) { data = data };
            return new OkObjectResult(model);
        }

        public static ObjectResult Error(string messange = "", string error_code = "", int? status_code = null)
        {
            var model = new ReturnCode(false);
            if (!string.IsNullOrEmpty(messange)) model.message = messange;
            if (!string.IsNullOrEmpty(error_code)) model.error_code = error_code;
            var ret = new ObjectResult(model);
            ret.StatusCode = status_code ?? 500;
            return ret;
        }

        public static ObjectResult ReturnCode(ReturnCode model)
        {
            var ret = new ObjectResult(model);
            ret.StatusCode = model.status_code ?? (model.status == 1 ? 200 : 500);
            return ret;
        }
    }
}
