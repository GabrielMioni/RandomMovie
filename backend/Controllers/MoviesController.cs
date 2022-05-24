using backend.Requests;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private MovieService _movieService;

        public MoviesController(MovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpPost]
        [Route("Collect")]
        public async Task<IActionResult> CollectMoviesAsync()
        {
            await _movieService.SaveMoviesAsync();

            return Ok();
        }

        [HttpPost]
        [Route("GetRandomMovie")]
        public IActionResult GetMovies([FromBody]RandomMovieRequest request = null)
        {
            var movie = _movieService.GetRandomMovieDto(request);

            if (movie == null)
            {
                return new NoContentResult();
            }
            return Ok(movie);
        }

        [HttpGet]
        [Route("/api/Movies/{movieId}")]
        public IActionResult GetSpecificMovie(int movieId)
        {
            if (movieId <= 0)
            {
                return BadRequest("movieId is invalid");
            }

            var movie = _movieService.GetMovieDtoById(movieId);
            return Ok(movie);
        }

        [HttpPost]
        [Route("GetMovies")]
        public IActionResult GetPaginatedMovies(GetMoviesPaginatedRequest request)
        {
            var movies = _movieService.GetMoviesPaginated(request);
            return Ok(movies);
        }
    }
}
