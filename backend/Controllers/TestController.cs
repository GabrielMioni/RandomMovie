using Microsoft.AspNetCore.Mvc;
using AngleSharp;
using AngleSharp.Html.Parser;
using AngleSharp.Dom;
using backend.services;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private HtmlReader _htmlReader;

        public TestController(HtmlReader htmlReader)
        {
            _htmlReader = htmlReader;
        }

        [HttpGet]
        [Route("Values")]
        // GET : /api/Test
        public IActionResult GetTest()
        {
            // return Ok(new { Hello = "just wanted to say hi" });
            var result = _htmlReader.ParseHtml("https://www.criterionchannel.com/");
            return Ok(result);
        }
    }
}
