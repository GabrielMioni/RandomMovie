using backend.Requests;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditMovieController : ControllerBase
    {
        private MovieService _movieService;

        public EditMovieController(MovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpPost]
        public IActionResult EditMovie (EditMoviePayload data)
        {
            var movieDto = _movieService.EditMovie(data);

            return Ok(movieDto);
        }
    }
}
