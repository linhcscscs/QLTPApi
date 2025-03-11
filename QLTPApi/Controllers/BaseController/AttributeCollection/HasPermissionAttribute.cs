using DataAccess.Helper.ControllerHelper;
using DataAccess.Helper.ControllerHelper.Values;
using DataAccess.Helper.Extensions.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using QLTPApi.Authentication;

namespace QLTPApi.Controllers.BaseController.AttributeCollection
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class HasPermissionAttribute : Attribute, IAuthorizationFilter, ICustomAttribute<SysTypeAccess>
    {
        private SysTypeAccess _sysTypeAccess;
        public HasPermissionAttribute(SysTypeAccess sysTypeAccess)
        {
            _sysTypeAccess = sysTypeAccess;
        }

        public SysTypeAccess GetValue()
        {
            return _sysTypeAccess;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // authorization
            var authContext = context.HttpContext.RequestServices.GetService<IAuthContext>();

            if (authContext?.IsAuthenticated != true)
            {
                // Expired_Token
                if (authContext?.Sys_Token_Infomation.IsAuthenticated == true)
                {
                    context.Result = ControllerHelper.ReturnCode(ErrorCode.Expired_Token);
                    return;
                }
                context.Result = ControllerHelper.ReturnCode(ErrorCode.Invalid_Token);
                return;
            }

            var permissons = authContext.Sys_Group_User_Menu;

            var isRoot = authContext.Sys_User?.IS_ROOT == 1 || authContext.Sys_User?.IS_ROOT_SYS == 1 || authContext.Sys_GroupUser?.ISRoot == 1;

            var hasPermission = false;
            switch (_sysTypeAccess)
            {
                case SysTypeAccess.View:
                    hasPermission = isRoot || permissons?.IsView == 1;
                    break;
                case SysTypeAccess.Add:
                    hasPermission = isRoot || permissons?.IsAdd == 1 && permissons?.IsView == 1;
                    break;
                case SysTypeAccess.Edit:
                    hasPermission = isRoot || permissons?.IsEdit == 1 && permissons?.IsView == 1;
                    break;
                case SysTypeAccess.Delete:
                    hasPermission = isRoot || permissons?.IsDelete == 1 && permissons?.IsView == 1;
                    break;
                case SysTypeAccess.Upload:
                    hasPermission = isRoot || permissons?.IsUpload == 1 && permissons?.IsView == 1;
                    break;
                case SysTypeAccess.Auth:
                    //hasPermission = isRoot || permissons.Any(q => permissons?.IsAuth == 1) || authContext.IsAuthenticated;
                    hasPermission = authContext.IsAuthenticated;
                    break;
                case SysTypeAccess.IsRoot:
                    hasPermission = isRoot;
                    break;
                case SysTypeAccess.IsRootSys:
                    hasPermission = authContext.Sys_User?.IS_ROOT_SYS == 1;
                    break;
                default:
                    break;
            }
            if (!hasPermission)
            {
                context.Result = ControllerHelper.ReturnCode(ErrorCode.Forbidden);
                return;
            }
        }
    }
}
