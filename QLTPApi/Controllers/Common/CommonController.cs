using AutoMapper;
using DataAccess.Helper.ControllerHelper;
using DataAccess.SQL.QLTP.Models;
using DataAccess.SQL.QLTP.Repository;
using Microsoft.AspNetCore.Authorization;
using DataAccess.Dtos;
using Microsoft.AspNetCore.Mvc;
using QLTPApi.Authentication;

namespace QLTPApi.Controllers.Common
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]/[action]")]
    public class CommonController : ControllerBase
    {
        #region Contructor
        private ISoGDRepository _soGDRepository;
        private IPhongGDRepository _phongGDRepository;
        private ITruongRepository _truongRepository;
        private IAuthContext _authContext;
        private IMapper _mapper;
        public CommonController(
            ISoGDRepository soGDRepository,
            IPhongGDRepository phongGDRepository,
            ITruongRepository truongRepository,
            IAuthContext authContext,
            IMapper mapper
            )
        {
            _soGDRepository = soGDRepository;
            _phongGDRepository = phongGDRepository;
            _truongRepository = truongRepository;
            _authContext = authContext;
            _mapper = mapper;
        }
        #endregion
        #region Method
        [HttpGet]
        public IActionResult DmSoGD()
        {
            try
            {
                var lstDm = _soGDRepository.getAllActive(_authContext.QLTPWorkingConnection) ?? new List<SO_GD>();
                var result = _mapper.Map<List<DmSoGdDto>>(lstDm);
                return ControllerHelper.Success(result);
            }
            catch
            {
                return ControllerHelper.Error();
            }
        }
        #endregion
    }
}
