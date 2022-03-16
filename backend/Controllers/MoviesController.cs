﻿using backend.Requests;
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

        [HttpGet]
        [Route("GetRandomMovie")]
        public IActionResult GetMovies(RandomMovieRequest request)
        {
            var movies = _movieService.GetRandomMovie(request);
            return Ok(movies);
        }
    }
}
