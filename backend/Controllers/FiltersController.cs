using Microsoft.AspNetCore.Mvc;
using AngleSharp.Dom;
using backend.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using backend.Data;
using backend.Models.Filters;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FiltersController : ControllerBase
    {
        private HtmlReader _htmlReader;
        private FilterService _filterService;
        private string NamePrefixPattern;

        public FiltersController(FilterService filterService)
        {
            _filterService = filterService;
        }

        [HttpPost]
        [Route("Collect")]
        public async Task<IActionResult> CollectFiltersAsync()
        {
            await _filterService.SaveFiltersAsync();

            var filterResponse = _filterService.GetAllFiltersData();

            return Ok(filterResponse);
        }
    }
}
