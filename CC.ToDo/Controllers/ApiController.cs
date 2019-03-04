using Microsoft.AspNetCore.Mvc;

namespace CC.ToDo.Controllers
{

    [ApiController]
    public class ApiController : Controller
    {

        [Route("api/")]
        public ActionResult<string> Get()
        {
            return Ok();
        }
    }
}
