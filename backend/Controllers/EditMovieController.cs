using backend.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditMovieController : ControllerBase
    {
        [HttpPost]
        public IActionResult blah (EditMoviePayload data)
        {
            return Ok(data);
        }
    }
}
