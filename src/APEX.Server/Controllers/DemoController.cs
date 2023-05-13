using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace APEX.Server.Controllers
{
    [Authorize]
    [ApiController]
    [RequiredScope("access_as_user")]
    [Route("[controller]")]
    public class DemoController : ControllerBase
    {
        public string Get()
        {
            return string.Join("\r\n", User.Identity.Name, User.Identity.AuthenticationType);
        }
    }
}