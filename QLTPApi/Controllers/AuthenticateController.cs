using DataAccess.Helper.ControllerHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QLTPApi.Authentication;
using QLTPApi.Authentication.Models;

namespace QLTPApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly ILogger<AuthenticateController> _logger;
        private IAuthService _authService;
        private IAuthContext _authContext;

        public AuthenticateController(ILogger<AuthenticateController> logger, IAuthService authService, IAuthContext authContext)
        {
            _logger = logger;
            _authService = authService;
            _authContext = authContext;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            return ControllerHelper.ReturnCode(_authService.Login(model));
        }
    }
}
