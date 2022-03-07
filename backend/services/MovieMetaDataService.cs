using AutoMapper;
using backend.Data;
using backend.Dtos;
using backend.Models;
using backend.Models.Deserializers;
using backend.Models.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
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

        //public List<DirectorDto> CollectMovieMetaDataThroughDirector ()
        public List<DirectorDto> GetAllDirectors()
        {
            var directors = _context.Directors
                .Include(d => d.Movie_Directors)
                    .ThenInclude(md => md.Movie)
                    .ThenInclude(m => m.Decade)
                .Include(m => m.Movie_Directors)
                    .ThenInclude(md => md.Movie)
                    .ThenInclude(m => m.Country)
                .Include(m => m.Movie_Directors)
                    .ThenInclude(md => md.Movie)
                    .ThenInclude(mg => mg.Movie_Genres)
                    .ThenInclude(mg => mg.Genre)
                .Where(d => !d.Name.Equals(""))
                .ToList();

            var directorDtoList = directors.Select(d =>
            {
                var directorDto = _mapper.Map<DirectorDto>(d);
                var movieDtos = d.Movie_Directors.Select(md => _mapper.Map<MovieDto>(md.Movie)).ToList();
                directorDto.Movies = movieDtos;
                return directorDto;
            }).ToList();

            return directorDtoList;
        }

        public List<MovieSearchResult> CollectMoviesByDirector()
        {
            var directorDtos = GetAllDirectors();

            var movieMetaList = new List<MovieSearchResult>();

            foreach (var directorDto in directorDtos)
            {
                var movieDictionary = GetDirectorMoviesDictionary(directorDto);

                foreach (var kvp in movieDictionary)
                {
                    var title = kvp.Value != null ? kvp.Value.title : "******Not Found*****";
                    Debug.WriteLine($"{kvp.Key} - {title}");

                    if (kvp.Value != null)
                    {
                        movieMetaList.Add(kvp.Value);
                    }
                }
            }

            return movieMetaList;
        }

        private Dictionary<string, MovieSearchResult> GetDirectorMoviesDictionary (DirectorDto directorDto)
        {
            var movieDictionary = new Dictionary<string, MovieSearchResult>();

            foreach (var movieTitle in directorDto.Movies.Select(m => m.Title))
            {
                movieDictionary[movieTitle] = null;
            }

            var possibleDirectors = GetPossibleDirectors(directorDto.Name);

            foreach (var possibleDirector in possibleDirectors)
            {
                var directorFilmography = new PersonFilmography();

                foreach (var movieDto in directorDto.Movies)
                {
                    var movieTitle = movieDto.Title;

                    if (movieDictionary[movieTitle] != null)
                        continue;

                    // Try to find the movie in the person search's 'known_for' property
                    var foundMovieSearchResult = possibleDirector.known_for.Find(kf => FindFoundKnownForMovie(kf, movieDto));
                    if (foundMovieSearchResult != null)
                    {
                        movieDictionary[movieTitle] = foundMovieSearchResult;
                        continue;
                    }

                    // Set directorFilmography if it hasn't been
                    if (directorFilmography.id == 0)
                    {
                        var filmography = GetMovieCreditsByPersonApiId(possibleDirector.id);
                        if (filmography.id <= 0)
                            continue;

                        directorFilmography = filmography;
                    }

                    // Try to find the movie in the directorFilmography.
                    var foundFilmography = directorFilmography.crew.Find(filmographyCrewResult => FindFoundKnownForMovie(filmographyCrewResult, movieDto, 10));
                    if (foundFilmography != null)
                    {
                        movieDictionary[movieTitle] = foundFilmography;
                    }
                }

                var nullMovies = movieDictionary.Where(dictionaryItem => dictionaryItem.Value == null).ToList();
                if (nullMovies.Count <= 0)
                    break;
            }

            // Try to find the movie by searching title
            foreach (var movieDto in directorDto.Movies)
            {
                var movieTitle = movieDto.Title;

                if (movieDictionary[movieTitle] != null)
                    continue;

                var movieSearch = SearchMovieByTitle(movieTitle);
                var movieSearchResults = movieSearch.results;
                var foundMovie = movieSearchResults.Find(movieResult => FindFoundKnownForMovie(movieResult, movieDto));

                if (foundMovie != null)
                {
                    movieDictionary[movieTitle] = foundMovie;
                    continue;
                }

                if (movieSearch.total_pages <= 1)
                    continue;

                var page = 2;
                while (page < movieSearch.total_pages)
                {
                    var innerResponse = SearchMovieByTitle(movieTitle, page);
                    var innerResult = innerResponse.results;
                    var innerFoundMovie = innerResult.Find(movieResult => FindFoundKnownForMovie(movieResult, movieDto));

                    if (foundMovie != null)
                    {
                        movieDictionary[movieTitle] = foundMovie;
                        break;
                    }
                    page++;
                }
            }

            return movieDictionary;
        }

        private List<PersonSearchResult> GetPossibleDirectors(string directorName)
        {
            var response = SearchMovieByDirector(directorName);
            var results = response.results.ToList();

            if (response.total_pages > 1)
            {
                var currentPage = 2;
                while (currentPage < response.total_pages)
                {
                    var newResponse = SearchMovieByDirector(directorName, currentPage);
                    results = results.Concat(newResponse.results).ToList();

                    currentPage++;
                }
            }

            return results.FindAll(r => LevenshteinDistanceService.Compute(r.name, directorName) < 5);
        }

        private bool FindFoundKnownForMovie(MovieSearchResult movieSearchResult, MovieDto movieDto, int yearTolerance = 2)
        {
            if (movieSearchResult.title == null || movieSearchResult.release_date == null)
            {
                return false;
            }

            var regex = new Regex("[^a-zA-Z0-9 -]");
            var movieSearchResultTitle = regex.Replace(movieSearchResult.title, "").Trim();
            var movieSearchResultOriginalTitle = regex.Replace(movieSearchResult.original_title, "").Trim();
            var movieDtoTitle = regex.Replace(movieDto.Title, "").Trim();

            //if (movieSearchResultTitle.Contains("100 Boyfriends") || movieSearchResultTitle.Contains("Tell Them We Are Rising"))
            //{
            //    Console.WriteLine("bringo");
            //}

            var knownForYear = 0;
            Int32.TryParse(movieSearchResult.release_date.Split('-')[0], out knownForYear);

            var yearDifference = GetYearDifference(knownForYear, movieDto.Year);
            var yearPasses = yearDifference <= yearTolerance;

            var movieTitlesContainExactMatch =
                FindContainedTitles(movieDtoTitle, movieSearchResultTitle) ||
                FindContainedTitles(movieSearchResultTitle, movieDtoTitle);

            if (movieTitlesContainExactMatch && yearPasses)
            {
                return true;
            }

            var movieTitleDistance = LevenshteinDistanceService.Compute(movieSearchResultTitle, movieDtoTitle);
            var movieTitleOriginalDistance = LevenshteinDistanceService.Compute(movieSearchResultOriginalTitle, movieDtoTitle);

            return (movieTitleDistance <= 3 || movieTitleOriginalDistance <= 3) && yearDifference <= yearTolerance;
        }

        private bool FindContainedTitles (string movieTitleOne, string movieTitleTwo)
        {
            if (movieTitleOne.Length > 5 && movieTitleTwo.Length > 5)
            {
                return movieTitleOne.Contains(movieTitleTwo);
            }
            return false;
        }

        public List<MovieMeta> CollectMovieMetaData()
        {
            var movieFromDb = _context.Movies
                .Include(m => m.Country)
                .Include(m => m.Decade)
                .Include(m => m.Movie_Directors)
                .ThenInclude(md => md.Director)
                .Include(m => m.Movie_Genres)
                .ThenInclude(mg => mg.Genre);

            var movieDtos = movieFromDb.Select(m => _mapper.Map<MovieDto>(m));

            var movieMetas = new List<MovieMeta>();

            foreach (var movieDto in movieDtos)
            {
                System.Diagnostics.Debug.WriteLine(movieDto.Title);
                var movieMeta = GetMetaDataForMovie(movieDto);
                if (movieMeta != null)
                {
                    System.Diagnostics.Debug.WriteLine(movieMeta.Title);
                    movieMetas.Add(movieMeta);
                } else
                {
                    System.Diagnostics.Debug.WriteLine($"Unable to find {movieDto.Title}");
                };
            }

            return movieMetas;
        }

        public MovieMeta GetMetaDataForMovie(MovieDto movieDto)
        {
            // var movie _context.Movies.
            //var movieFromDb = _context.Movies
            //    .Include(m => m.Country)
            //    .Include(m => m.Decade)
            //    .Include(m => m.Movie_Directors)
            //    .ThenInclude(md => md.Director)
            //    .Include(m => m.Movie_Genres)
            //    .ThenInclude(mg => mg.Genre)
            //    .FirstOrDefault(m => m.Title == "Mulholland Dr.");

            //if (movieFromDb == null) return;

            //var movieDto = _mapper.Map<MovieDto>(movieFromDb);

            var movieSearchByTitle = SearchMovieByTitle(movieDto.Title);

            var results = movieSearchByTitle.results;

            if (movieSearchByTitle.total_pages > 1)
            {
                var page = 2;
                while (page <= movieSearchByTitle.total_pages)
                {
                    var movieSearchPage = SearchMovieByTitle(movieDto.Title, page);
                    var newResults = movieSearchPage.results;
                    results = results.Concat(newResults).ToList();
                    page++;
                }
            }

            var movieMeta = new MovieMeta();

            foreach (var movieResult in results)
            {
                int releasedYearInt = 0;

                var releaseDate = movieResult.release_date;

                if (releaseDate != null)
                {
                    Int32.TryParse(movieResult.release_date.Split('-')[0], out releasedYearInt);
                }

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

                movieMeta = _mapper.Map<MovieMeta>(movieResult);
                movieMeta.MovieId = movieDto.Id;
                break;
            }

            // return movieMeta;

            return movieMeta.Id == 0 ? null : movieMeta;
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

        public PersonSearch SearchMovieByDirector (string directorName, int page = 0)
        {
            var response = GetResponseFromMovieApi("search/person", directorName, page);
            var movieByDirectorResponse = JsonSerializer.Deserialize<PersonSearch>(response);

            return movieByDirectorResponse;           
        }

        public PersonFilmography GetMovieCreditsByPersonApiId(int personId)
        {
            var url = $"person/{personId}/movie_credits";

            var response = GetResponseFromMovieApi(url, null);
            var directorFilmography = JsonSerializer.Deserialize<PersonFilmography>(response);

            return directorFilmography;
        }

        public MovieSearch SearchMovieByTitle (string title, int page = 0)
        {
            var response = GetResponseFromMovieApi("search/movie", title, page);
            var movieSearchResponse = JsonSerializer.Deserialize<MovieSearch>(response);

            return movieSearchResponse;
        }

        private string GetResponseFromMovieApi (string endpoint, string queryValue = null, int page = 0)
        {
            var queryParams = new Dictionary<string, string>();

            if (queryValue != null)
            {
                queryParams["query"] = queryValue;
            }

            if (page > 0)
            {
                queryParams["page"] = page.ToString();
            }

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
