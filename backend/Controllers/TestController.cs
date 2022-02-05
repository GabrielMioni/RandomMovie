﻿using Microsoft.AspNetCore.Mvc;
using AngleSharp.Dom;
using backend.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using backend.Data;
using backend.Models.Filters;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private HtmlReader _htmlReader;
        private ApplicationDbContext _context;

        public TestController(HtmlReader htmlReader, ApplicationDbContext context)
        {
            _htmlReader = htmlReader;
            _context = context;
        }

        [HttpGet]
        [Route("Filters")]
        public async Task<IActionResult> GetFiltersAsync()
        {
            var htmlCollection = await _htmlReader.ParseHtmlAsync("https://films.criterionchannel.com/", "main");

            var main = htmlCollection.Length > 0 ? htmlCollection[0] : null;

            await AddCountriesAsync(main);
            await AddDecadesAsync(main);
            await AddDirectorsAsync(main);
            await AddGenresAsync(main);
            await _context.SaveChangesAsync();

            var countries = _context.Countries.ToList();
            var decades = _context.Decades.ToList();
            var directors = _context.Directors.ToList();
            var genres = _context.Genres.ToList();

            return Ok(new { genres, decades, countries, directors });
        }

        private async Task AddCountriesAsync(IElement main)
        {
            var countries = GetCountries(main);

            await TruncateTable("Countries");
            await _context.Countries.AddRangeAsync(countries);
        }

        private async Task AddDecadesAsync(IElement main)
        {
            var decades = GetDecades(main);

            await TruncateTable("Decades");
            await _context.Decades.AddRangeAsync(decades);
        }

        private async Task AddGenresAsync(IElement main)
        {
            var genres = GetGenres(main);

            await TruncateTable("Genres");
            await _context.Genres.AddRangeAsync(genres);
        }

        private async Task AddDirectorsAsync(IElement main)
        {
            var directors = getDirectors(main);

            await TruncateTable("Directors");
            await _context.Directors.AddRangeAsync(directors);
        }

        private async Task TruncateTable(string tableName)
        {
            var sqlCommand = string.Format("TRUNCATE TABLE {0}", tableName);
            await _context.Database.ExecuteSqlRawAsync(sqlCommand);
        }

        private List<Country> GetCountries(IElement main)
        {
            var countryStrings = GetFilterData(main, "country");

            var countryList = new List<Country>();

            foreach(var countryString in countryStrings)
            {
                var country = new Country
                {
                    Name = countryString
                };

                countryList.Add(country);
            }

            return countryList.OrderBy(c => c.Name).ToList();
        }

        private List<Decade> GetDecades(IElement main)
        {
            var decadeStrings = GetFilterData(main, "decade");

            var decadeList = new List<Decade>();

            foreach(var decadeString in decadeStrings)
            {
                var decade = new Decade
                {
                    Name = decadeString
                };

                decadeList.Add(decade);
            }

            return decadeList.OrderBy(d => d.Name).ToList();
        }

        private List<Genre> GetGenres(IElement main)
        {
            var genreStrings = GetFilterData(main, "genre");

            var genreList = new List<Genre>();

            foreach(var genreString in genreStrings)
            {
                var genre = new Genre
                {
                    Name = genreString
                };

                genreList.Add(genre);
            }

            return genreList.OrderBy(g => g.Name).ToList();
        }

        private List<string> GetFilterData(IElement main, string labelName)
        {
            var labelSelector = string.Format("*[data-filter-group='{0}'] label", labelName);
            var labelElms = main.QuerySelectorAll(labelSelector);

            var labelValues = new List<string>();

            foreach(var label in labelElms)
            {
                labelValues.Add(label.InnerHtml.Trim());
            }

            return labelValues;
        }

        private List<Director> getDirectors(IElement main)
        {
            var tableRows = main.QuerySelectorAll("#gridview tbody tr");

            var directorList = new List<Director>();

            foreach(var tr in tableRows)
            {
                var directorStrings = GetInnerHtml(tr, ".criterion-channel__td--director")
                    .Replace(" and ", ", ")
                    .Replace(", and", ", ")
                    .Replace("  ", " ")
                    .Split(", ")
                    .Select(directorName => directorName.Trim())
                    .Distinct();
                
                foreach(var directorName in directorStrings)
                {
                    var alreadyInList = directorList.Find(directorInList => directorInList.FullName == directorName);

                    if (alreadyInList != null)
                    {
                        continue;
                    }

                    string firstName = directorName;
                    string lastName = directorName;
                    string fullName = directorName;

                    var indexOfVon = directorName.ToLower().IndexOf(" von ");
                    var index = indexOfVon > -1
                        ? indexOfVon
                        : directorName.LastIndexOf(' ');

                    if (index > -1)
                    {
                        firstName = directorName.Substring(0, index);
                        lastName = directorName.Substring(index + 1);
                    }

                    var directorDto = new Director
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        FullName = fullName
                    };

                    directorList.Add(directorDto);
                }
            }

            return directorList.OrderBy(d => d.LastName).ToList();
        }

        private string GetInnerHtml(IElement elm, string querySelector)
        {
            return elm.QuerySelector(querySelector).InnerHtml.Trim();
        }
    }
}
