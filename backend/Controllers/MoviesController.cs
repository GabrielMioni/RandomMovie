using AngleSharp.Dom;
using backend.Data;
using backend.Dtos;
using backend.Models;
using backend.Models.Filters;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private HtmlReader _htmlReader;
        private ApplicationDbContext _context;
        private MovieService _movieService;
        private FilterService _filterService;

        public MoviesController(HtmlReader htmlReader, ApplicationDbContext context, MovieService movieService, FilterService filterService)
        {
            _htmlReader = htmlReader;
            _context = context;
            _movieService = movieService;
            _filterService = filterService;
        }

        [HttpPost]
        [Route("Collect")]
        public async Task<IActionResult> CollectMoviesAsync()
        {
            var genres = _context.Genres.ToList().OrderBy(g => g.Name);
            var movieDtos = new List<MovieDto>();

            foreach (var genre in genres)
            {
                var genreName = genre.Name.ToLower();
                var url = $"https://films.criterionchannel.com/?genre={genreName}";
                var htmlCollection = await _htmlReader.ParseHtmlAsync(url, "main");
                var main = htmlCollection.Length > 0 ? htmlCollection[0] : null;

                var moviesDtosForGenre = GetMoviesData(main, genre);
                movieDtos = movieDtos.Concat(moviesDtosForGenre).ToList();
            }

            var counter = 0;

            foreach (var movieDto in movieDtos)
            {
                _movieService.AddMovie(movieDto);
                counter++;

                if (counter >= 100)
                {
                    await _context.SaveChangesAsync();
                    counter = 0;
                }
            }

            return Ok(movieDtos);
        }

        private List<MovieDto> GetMoviesData(IElement main, Genre genre)
        {
            var tableRows = main.QuerySelectorAll("#gridview tbody tr");

            var movieList = new List<MovieDto>();

            foreach (var row in tableRows)
            {
                var countryString = GetInnerHtml(row, ".criterion-channel__td--country span:first-of-type").Trim();
                var directorString = GetInnerHtml(row, ".criterion-channel__td--director").Trim();
                var movieTitleString = GetInnerHtml(row, ".criterion-channel__td--title a").Trim();
                var yearString = GetInnerHtml(row, ".criterion-channel__td--year").Trim();

                var country = _context.Countries.FirstOrDefault(c => c.Name.Replace("-", " ") == countryString);
                var decade = GetDecadeFromString(yearString);
                var directors = GetDirectorsFromString(directorString);

                var movie = new MovieDto
                {
                    Country = country,
                    Decade = decade,
                    Directors = directors,
                    Genres = new List<Genre> { genre },
                    Title = movieTitleString
                };
                movieList.Add(movie);
            }

            return movieList;
        }

        private Decade GetDecadeFromString(string yearString)
        {
            var year = Int32.Parse(yearString);
            var decade = _context.Decades.FirstOrDefault(d => year >= d.DecadeInt && year <= d.DecadeInt + 9);
            return decade;
        }

        private List<Director> GetDirectorsFromString(string directorsString)
        {
            var directorNames = _filterService.GetDirectorNamesFromString(directorsString);
            return directorNames.Select(name => GetDirectorByName(name)).ToList();
        }

        private Director GetDirectorByName(string directorName)
        {
            return _context.Directors.FirstOrDefault(d => d.Name == directorName);
        }

        private string GetInnerHtml(IElement elm, string querySelector)
        {
            return elm.QuerySelector(querySelector).InnerHtml.Trim();
        }

        // TODO Make TruncateTable a helper function and replace the the method here and in FiltersController.TruncateTable()
        private async Task TruncateTable(string tableName)
        {
            await _context.Database.ExecuteSqlRawAsync("Delete from " + tableName);
            await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('" + tableName + "', RESEED, 0)");
        }

        //private Task GetMovies(IElement main)
        //{

        //}
    }
}
