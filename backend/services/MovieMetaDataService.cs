using AutoMapper;
using backend.Data;
using backend.Dtos;
using backend.Models;
using backend.Models.Deserializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace backend.Services
{
    public class MovieMetaDataService
    {
        private readonly IConfiguration _config;
        private ApplicationDbContext _context;
        private IMapper _mapper;
        private readonly string ApiKey;
        private readonly string RootUrl;
        private readonly LevenshteinDistanceService _levService;

        public MovieMetaDataService(ApplicationDbContext context, IConfiguration config, LevenshteinDistanceService levenshteinDistanceService, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
            ApiKey = _config["MovieDbApi:ApiKey"];
            RootUrl = _config["MovieDbApi:Url"];
            _levService = levenshteinDistanceService;
        }

        public void GetMetaDataForMovie()
        {
            // var movie _context.Movies.
            var movieFromDb = _context.Movies
                .Include(m => m.Country)
                .Include(m => m.Decade)
                .Include(m => m.Movie_Directors)
                .ThenInclude(md => md.Director)
                .Include(m => m.Movie_Genres)
                .ThenInclude(mg => mg.Genre)
                .FirstOrDefault(m => m.Title == "Mulholland Dr.");

            if (movieFromDb == null) return;

            var movieDto = _mapper.Map<MovieDto>(movieFromDb);

            var movieSearchByTitle = SearchMovieByTitle(movieDto.Title);

            var results = movieSearchByTitle.results;

            foreach (var movieResult in results)
            {
                var releasedYearInt = Int32.Parse(movieResult.release_date.Split('-')[0]);

                if (releasedYearInt < 1900)
                {
                    continue;
                }

                var yearDifference = GetYearDifference(releasedYearInt, movieDto.Year);

                if (yearDifference > 2)
                {
                    continue;
                }

                var distance = LevenshteinDistanceService.Compute(movieResult.title, movieDto.Title);

                if (distance > 10)
                {
                    continue;
                }

                if (distance > 2)
                {
                    var directorMatched = CheckMovieDirectorByApiId(movieResult.id, movieDto.Directors);
                    if (!directorMatched)
                    {
                        continue;
                    }
                }

                // add movie meta data
            }
        }

        private int GetYearDifference (int yearA, int yearB)
        {
            return yearA > yearB? yearA - yearB: yearB - yearA;
        }

        private bool CheckMovieDirectorByApiId(int apiId, List<DirectorDto> directors)
        {
            var credits = GetMovieCreditsByApiId(apiId);
            var directorNamesFromApi = credits.crew
                .FindAll(c => c.job == "Director")
                .Select(c => c.name)
                .ToList();

            var directorNamesFromDb = directors.Select(d => d.Name);

            foreach (var name in directorNamesFromApi)
            {
                if (directorNamesFromDb.Contains(name))
                {
                    return true;
                }
            }

            return false;
        }

        public Credits GetMovieCreditsByApiId(int movieId)
        {
            var response = GetResponseFromMovieApi($"movie/{movieId}/credits", null);
            var cast = JsonSerializer.Deserialize<Credits>(response);
            return cast;
        }

        public string SearchMovieByDirector (string directorName)
        {
            var response = GetResponseFromMovieApi("search/person", directorName);

            return response;           
        }

        public MovieSearch SearchMovieByTitle (string title)
        {
            var response = GetResponseFromMovieApi("search/movie", title);
            var movieSearchResponse = JsonSerializer.Deserialize<MovieSearch>(response);

            return movieSearchResponse;
        }

        private string GetResponseFromMovieApi (string endpoint, string queryValue)
        {
            var queryParams = new Dictionary<string, string>()
            {
                { "query", queryValue }
            };

            return SendRequest(endpoint, queryParams);
        }

        private string SendRequest (string endpoint, Dictionary<string, string> queryParams = null)
        {
            var uri = $"{RootUrl}/{endpoint}";

            var uriBuilder = new UriBuilder(uri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            if (queryParams != null)
            {
                foreach (var keyValue in queryParams)
                {
                    query.Add(keyValue.Key, keyValue.Value);
                }
            }

            query.Add("api_key", ApiKey);
            uriBuilder.Query = query.ToString();
            uri = uriBuilder.ToString();

            var httpRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpRequest.Accept = "application/json";

            var response = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
