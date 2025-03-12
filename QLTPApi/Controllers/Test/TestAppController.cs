using DataAccess.Caching.Interface;
using DataAccess.Helper.ControllerHelper;
using DataAccess.Helper.LazyDI.ModelHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLTPApi.Controllers.Test.Model;

namespace QLTPApi.Controllers.Test
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestAppController : ControllerBase
    {
        public TestAppController(ICacheProvider cache)
        {
            _cache = cache;
        }
        private ICacheProvider _cache;
        [HttpPost]
        public IActionResult TestPatching(PatchModelTesting patch)
        {
            var model = new PatchingTesting();
            var res = model.ApplyPatch(patch);
            return ControllerHelper.Success(res.ResObject);
        }
        [HttpPost]
        public IActionResult TestCaching()
        {
            return ControllerHelper.Success(_cache.GetAllKey());
        }
    }
}
