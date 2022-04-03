using backend.Models.Deserializers;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

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

        [HttpGet]
        [Route("Get")]
        public IActionResult GetMetaData ()
        {
            var configDetails = _metaDataService.GetConfigurationDetails();
            return Ok(configDetails);
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
        public async Task<IActionResult> CollectMetaDataAsync()
        {
            var results = await _metaDataService.CollectMoviesByDirectorAsync();
            return Ok(results);
        }

        [HttpPost]
        [Route("CollectConfiguration")]
        public IActionResult CollectMetaConfiguration()
        {
            var results = _metaDataService.GetConfigurationData();
            return Ok(results);
        }

        [HttpPost]
        [Route("CollectCredits")]
        public async Task<IActionResult> CollectCreditsAsync()
        {
            await _metaDataService.CollectMoviePersonsByApiIdAsync();
            return Ok();
        }
    }
}
