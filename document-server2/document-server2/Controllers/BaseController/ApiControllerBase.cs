using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace document_server2.Controllers.BaseController
{
    [Route("api/[controller]")]
    [EnableCors("Origins")]
    public class ApiControllerBase : Controller
    {
        protected string UserEmail => User?.Identity?.IsAuthenticated == true ? User.Identity.Name : string.Empty;
    }
}
