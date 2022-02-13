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
        private ApplicationDbContext _context;
        private string NamePrefixPattern;

        public FiltersController(HtmlReader htmlReader, ApplicationDbContext context)
        {
            _htmlReader = htmlReader;
            _context = context;
        }

        [HttpPost]
        [Route("Collect")]
        public async Task<IActionResult> CollectFiltersAsync()
        {
            await CollectAllFilters();

            var countries = _context.Countries.ToList();
            var decades = _context.Decades.ToList();
            var directors = _context.Directors.ToList();
            var genres = _context.Genres.ToList();

            var filterResponse = new
            {
                countries = new { data = countries, count = countries.Count },
                decades = new { data = decades, count = decades.Count },
                directors = new { data = directors, count = directors.Count },
                genres = new { data = genres, count = genres.Count }
            };

            return Ok(filterResponse);
        }

        private async Task CollectAllFilters()
        {
            var htmlCollection = await _htmlReader.ParseHtmlAsync("https://films.criterionchannel.com/", "main");

            var main = htmlCollection.Length > 0 ? htmlCollection[0] : null;

            await AddCountriesAsync(main);
            await AddDecadesAsync(main);
            await AddDirectorsAsync(main);
            await AddGenresAsync(main);
            await _context.SaveChangesAsync();
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
            var directors = GetDirectors(main);

            await TruncateTable("Directors");
            await _context.Directors.AddRangeAsync(directors);
        }

        private async Task TruncateTable(string tableName)
        {
            await _context.Database.ExecuteSqlRawAsync("Delete from " + tableName);
            await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('" + tableName + "', RESEED, 0)");
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
                var decadeInt = Regex.Match(decadeString, @"\d+").Value;

                var decade = new Decade
                {
                    Name = decadeString,
                    DecadeInt = Int32.Parse(decadeInt)
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
                var value = label.InnerHtml.Trim();
                labelValues.Add(Regex.Replace(value, "[/|\\s]", "-"));
            }

            return labelValues;
        }

        private List<Director> GetDirectors(IElement main)
        {
            var tableRows = main.QuerySelectorAll("#gridview tbody tr");

            var directorList = new List<Director>();

            foreach(var tr in tableRows)
            {
                var directorString = GetInnerHtml(tr, ".criterion-channel__td--director");
                directorString = Regex.Replace(directorString, "( and )|( , and)", ", ").Replace(",,", ",");

                var directorNames = directorString
                    .Split(", ")
                    .Select(directorName => Regex.Replace(directorName.Trim(), @"\s+", " "))
                    .Distinct();
                
                foreach(var directorName in directorNames)
                {
                    var alreadyInList = directorList.Find(d => d.Name.Trim() == directorName.Trim());

                    if (alreadyInList != null)
                    {
                        continue;
                    }

                    string firstName = directorName;
                    string lastName = directorName;
                    string name = directorName;

                    var prefixPattern = GetNamePrefixPattern();

                    var prefixMatch = Regex.Match(directorName, prefixPattern, RegexOptions.IgnoreCase);
                    var indexOfPrefix = prefixMatch.Success ? prefixMatch.Index : -1;

                    var index = indexOfPrefix > -1
                        ? indexOfPrefix
                        : directorName.LastIndexOf(' ');

                    if (index > -1)
                    {
                        firstName = directorName.Substring(0, index);
                        lastName = directorName.Substring(index + 1);
                    }

                    var director = new Director
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Name = name
                    };

                    directorList.Add(director);
                }
            }

            return directorList.OrderBy(d => d.LastName).ToList();
        }

        private string GetNamePrefixPattern()
        {
            if (NamePrefixPattern == null)
            {
                string[] prefixesList = { "af", "al", "el", "auf", "da", "de", "dai", "dal", "del", "della", "dei", "di", "des", "du", "d'", "of", "von", "van", "zu" };
                NamePrefixPattern = string.Join("|", prefixesList.Select(prefix => $"( {prefix} )"));
            }
            return NamePrefixPattern;
        }

        private string GetInnerHtml(IElement elm, string querySelector)
        {
            return elm.QuerySelector(querySelector).InnerHtml.Trim();
        }
    }
}
