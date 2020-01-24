using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sandbox.Presentation.Web.Api.V1.Topics.Doots
{
    [Authorize]
    [ApiController]
    [Route("api/v1/topics/updoot")]
    public class TopicsUpdootsController : Controller
    {
        [HttpPost("{id:long}")]
        public IActionResult Post(long id)
        {
            return Ok();
        }
    }
}
