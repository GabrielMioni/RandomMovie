using AngleSharp.Dom;
using AutoMapper;
using backend.Data;
using backend.Dtos;
using backend.Extensions;
using backend.Models.Filters;
using backend.Requests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace backend.Services
{

    public class FilterService
    {
        private ApplicationDbContext _context;
        private HtmlReader _htmlReader;
        private string NamePrefixPattern;
        private IMapper _mapper;

        public FilterService(ApplicationDbContext context, HtmlReader htmlReader, IMapper mapper)
        {
            _context = context;
            _htmlReader = htmlReader;
            _mapper = mapper;
        }

        public async Task SaveFiltersAsync()
        {
            await SaveAllFiltersAsync();
        }

        public List<DirectorDto> SearchDirectors (string search)
        {
            var searchString = search ?? string.Empty;

            var directors = searchString.Trim().Length > 0
                ? _context.Directors.Where(d => d.Name.Contains(searchString) && d.Name != "").OrderBy(d => d.LastName).Take(25)
                : _context.Directors.Where(d => d.Name != "").OrderBy(d => d.LastName).Take(25);

            return directors.Select(d => _mapper.Map<DirectorDto>(d)).ToList();
        }

        public List<GenreDto> GetGenres ()
        {
            var genres = _context.Genres;

            return genres.OrderBy(g => g.Name).Select(g => _mapper.Map<GenreDto>(g)).ToList();
        }

        public List<Country> GetCountries ()
        {
            var countries = _context.Countries;
            return countries.OrderBy(c => c.Name).ToList();
        }

        public object GetAllFiltersData()
        {
            var countries = _context.Countries.ToList();
            var decades = _context.Decades.Where(d => d.Name != "0000s").ToList();
            var directors = _context.Directors.Select(d => _mapper.Map<DirectorDto>(d)).ToList(); 
            var genres = _context.Genres.Select(g => _mapper.Map<GenreDto>(g)).ToList();

            var filterRequest = new FiltersRequest(countries, decades, directors, genres);

            return filterRequest;
        }

        private async Task SaveAllFiltersAsync()
        {
            var htmlCollection = await _htmlReader.ParseHtmlAsync("https://films.criterionchannel.com/", "main");

            var main = htmlCollection.Length > 0 ? htmlCollection[0] : null;

            await _context.TruncateTable("Movies");

            await _context.SaveChangesAsync();

            await AddCountriesAsync(main);
            await AddDecadesAsync(main);
            await AddDirectorsAsync(main);
            await AddGenresAsync(main);
            await _context.SaveChangesAsync();
        }

        private async Task AddCountriesAsync(IElement main)
        {
            var countries = CollectCountriesData(main);

            await _context.TruncateTable("Countries");
            await _context.Countries.AddRangeAsync(countries);
        }

        private async Task AddDecadesAsync(IElement main)
        {
            var decades = GetDecades(main);

            await _context.TruncateTable("Decades");
            await _context.Decades.AddRangeAsync(decades);
        }

        private async Task AddGenresAsync(IElement main)
        {
            var genres = GetGenres(main);

            await _context.TruncateTable("Genres");
            await _context.Genres.AddRangeAsync(genres);
        }

        private async Task AddDirectorsAsync(IElement main)
        {
            var directors = GetDirectors(main);

            await _context.TruncateTable("Directors");
            await _context.Directors.AddRangeAsync(directors);
        }

        private List<Country> CollectCountriesData(IElement main)
        {
            var countryStrings = GetFilterData(main, "country");

            var countryList = new List<Country>();

            foreach (var countryString in countryStrings)
            {
                var country = new Country
                {
                    Name = countryString.Replace("-", " ").Trim()
                };

                countryList.Add(country);
            }

            return countryList.OrderBy(c => c.Name).ToList();
        }

        private List<Decade> GetDecades(IElement main)
        {
            var decadeStrings = GetFilterData(main, "decade");

            var decadeList = new List<Decade>();

            decadeList.Add(new Decade { DecadeInt = 0, Name = "0000s" });
            decadeList.Add(new Decade { DecadeInt = 1900, Name = "1900s" });

            foreach (var decadeString in decadeStrings)
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

            foreach (var genreString in genreStrings)
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

            foreach (var label in labelElms)
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

            foreach (var tr in tableRows)
            {
                var directorString = GetInnerHtml(tr, ".criterion-channel__td--director");
                var directorNames = directorString.GetDirectorNamesFromString();

                foreach (var directorName in directorNames)
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

        //public List<string> GetDirectorNamesFromString(string directorString)
        //{
        //    directorString = Regex.Replace(directorString, "( and )|( , and)", ", ").Replace(",,", ",");

        //    return directorString
        //        .Split(", ")
        //        .Select(directorName => Regex.Replace(directorName.Trim(), @"\s+", " ")) // Convert any whitespace to a single space
        //        .Distinct()
        //        .ToList();
        //}
    }
}
