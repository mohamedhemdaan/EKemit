using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EKemit.APIs.Errors;

namespace EKemit.APIs.Controllers
{
    [Route("errors/{Code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        public ActionResult Error(int Code)
        {
            return NotFound(new ApiResponse(Code));
        }
    }
}
