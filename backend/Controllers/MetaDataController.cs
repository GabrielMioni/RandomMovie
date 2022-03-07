using backend.Models.Deserializers;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetaDataController : ControllerBase
    {
        private MovieMetaDataService _metaDataService;

        public MetaDataController(MovieMetaDataService metaDataService)
        {
            _metaDataService = metaDataService;
        }

        [HttpPost]
        [Route("Get")]
        public IActionResult GetMetaData ()
        {
            var response = _metaDataService.SearchMovieByTitle("A mother should be loved");

            return Ok(response);
        }

        [HttpPost]
        [Route("Director")]
        public IActionResult GetMetaDataByDirector()
        {
            var response = _metaDataService.SearchMovieByDirector("David Lynch");

            return Ok(response);
        }

        [HttpPost]
        [Route("MovieCast")]
        public IActionResult GetMovieCast()
        {
            var response = _metaDataService.GetMovieCreditsByApiId(185789);

            return Ok(response);
        }

        [HttpPost]
        [Route("Collect")]
        public IActionResult CollectMetaData()
        {
            var results = _metaDataService.CollectMoviesByDirector();
            return Ok(results);
        }
    }
}
