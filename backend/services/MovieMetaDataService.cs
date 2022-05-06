using AutoMapper;
using backend.Data;
using backend.Dtos;
using backend.Extensions;
using backend.Models;
using backend.Models.Deserializers;
using backend.Requests;
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

        public ConfigurationPayload GetConfigurationDetails ()
        {
            var baseUrls = _context.MovieMetaUrls.FirstOrDefault();

            var posterSizes = GetImageSizeByType(ImageTypes.Poster);
            var backDropSizes = GetImageSizeByType(ImageTypes.Backdrop);
            var logoSizes = GetImageSizeByType(ImageTypes.Logo);

            var payload = new ConfigurationPayload
            {
                BaseUrls = _mapper.Map<BaseUrlsDto>(baseUrls),
                BackDropSizes = backDropSizes,
                LogoSizes = logoSizes,
                PosterSizes = posterSizes,
            };

            return payload;
        }

        public List<PersonDto> SearchPeople (string search)
        {
            var searchString = search ?? string.Empty;

            var people = searchString.Trim().Length > 0
                ? _context.Persons.Where(p => p.Name.Contains(searchString) && p.Name != "").OrderBy(p => p.Name.Trim()).Take(25)
                : _context.Persons.Where(p => p.Name != "").OrderBy(d => d.Name.Trim()).Take(25);

            return people.Select(d => _mapper.Map<PersonDto>(d)).ToList();
        }

        private Dictionary<string, string> GetImageSizeByType (string type)
        {
            var sizes = _context.MovieMetaImageSizes
                .Where(size => size.Type == type && size.Size != "original")
                .Select(size => size.Size)
                .ToList()
                .OrderBy(size =>
                {
                    string numberString = Regex.Replace(size, "[^0-9.]", "");
                    var number = 0;
                    Int32.TryParse(numberString, out number);
                    return number;
                })
                .ToList();

            var isEven = sizes.Count % 2 == 0;

            var sizeDictionary = new Dictionary<string, string>();

            if (isEven && sizes.Count >= 6)
            {
                sizeDictionary.Add("xs", sizes[0]); // w92
                sizeDictionary.Add("sm", sizes[1]); // w154
                sizeDictionary.Add("md", sizes[2]); // w185
                sizeDictionary.Add("lg", sizes[3]); // w342
                sizeDictionary.Add("xl", sizes[4]); // w500
                sizeDictionary.Add("xxl", sizes[5]); // w780

            } else if (!isEven && sizes.Count <= 3)
            {
                sizeDictionary.Add("sm", sizes[0]);
                sizeDictionary.Add("md", sizes[1]);
                sizeDictionary.Add("lg", sizes[2]);
            }

            sizeDictionary.Add("original", "original");


            return sizeDictionary;
        }

        public Configuration GetConfigurationData()
        {
            var response = CollectMovieMetaConfigurationData();

            var imagesConfig = response.images;

            var baseUrl = imagesConfig.base_url;
            var secureBaseUrl = imagesConfig.base_url;
            var posterSizes = imagesConfig.poster_sizes;
            var backdropSizes = imagesConfig.backdrop_sizes;
            var logoSizes = imagesConfig.logo_sizes;

            var movieMetaUrl = new MovieMetaUrl
            {
                BaseUrl = baseUrl,
                SecureBaseUrl = secureBaseUrl
            };

            _context.MovieMetaUrls.Add(movieMetaUrl);

            var imageSizeDictionary = new Dictionary<string, List<string>>();

            imageSizeDictionary.Add(ImageTypes.Poster, posterSizes);
            imageSizeDictionary.Add(ImageTypes.Backdrop, backdropSizes);
            imageSizeDictionary.Add(ImageTypes.Logo, logoSizes);

            foreach (var kvp in imageSizeDictionary)
            {
                var type = kvp.Key;
                var sizes = kvp.Value;

                foreach (var size in sizes)
                {
                    var metaImageSieze = new MovieMetaImageSize
                    {
                        Size = size,
                        Type = type
                    };
                    _context.MovieMetaImageSizes.Add(metaImageSieze);                    
                }
                _context.SaveChanges();
            }

            return response;
        }

        //public List<DirectorDto> CollectMovieMetaDataThroughDirector ()
        public List<DirectorDto> GetAllDirectorsAsync()
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

        public async Task CollectMoviePersonsByApiIdAsync()
        {
            await _context.TruncateTable("Movie_Persons");
            await _context.TruncateTable("Persons");

            var metas = _context.MovieMetas.ToList();

            var creditsList = new List<Credits>();
            var moviePersonList = new List<Person>();

            foreach (var meta in metas)
            {
                var movieApiId = meta.ApiId;

                var credits = GetMovieCreditsByApiId(movieApiId);
                creditsList.Add(credits);

                var actors = credits.cast
                    .Where(c => c.known_for_department == "Acting")
                    .Take(6);

                var directors = credits.crew
                    .Where(d => d.known_for_department == "Directing");

                var allMovieCredits = actors.Concat(directors);

                //var persons = allMovieCredits.Select(creditPerson => _mapper.Map<Person>(creditPerson));

                //moviePersonList.AddRange(persons);

                foreach (var creditPerson in allMovieCredits)
                {
                    var personFromDb = _context.Persons.FirstOrDefault(p => p.ApiId == creditPerson.id);

                    if (personFromDb != null)
                    {
                        continue;
                    }

                    var person = _mapper.Map<Person>(creditPerson);

                    var details = GetPersonDetailsByPersonApiId(person.ApiId);
                    if (details != null)
                    {
                        person.Biography = FormatPersonBiography(details.biography);
                        person.Birthday = details.birthday;
                        person.Deathday = details.deathday;
                    }

                    _context.Persons.Add(person);
                    _context.SaveChanges();

                    var moviePerson = new Movie_Person
                    {
                        PersonId = person.Id,
                        MovieId = meta.MovieId
                    };
                    _context.Movie_Persons.Add(moviePerson);
                };
            }

            // return moviePersonList;
        }

        private string FormatPersonBiography (string biography)
        {
            var biographyParts = biography.Split("\n\n");
            var noWikipedia = biographyParts.Where(part => !part.ToLower().Contains("wikipedia"));
            return String.Join("\n\n", noWikipedia);
        }

        public async Task<List<MovieSearchResult>> CollectMoviesByDirectorAsync()
        {
            await _context.TruncateTable("MovieMetas");

            var directorDtos = GetAllDirectorsAsync();

            var movieMetaList = new List<MovieSearchResult>();

            var blah = new Dictionary<Movie, MovieMeta>();

            foreach (var directorDto in directorDtos)
            {
                var movieDictionary = GetDirectorMoviesDictionary(directorDto);

                foreach (var kvp in movieDictionary)
                {
                    var movieDtoTitle = kvp.Key;
                    var movieDto = directorDto.Movies.FirstOrDefault(m => m.Title == movieDtoTitle);
                    var movieSearchResult = kvp.Value;

                    if (movieDto == null ||  movieSearchResult == null)
                    {
                        continue;
                    }

                    // var movie = _context.Movies.FirstOrDefault(m => m.Id == movieDto.Id);
                    var movieMeta = _mapper.Map<MovieMeta>(movieSearchResult);
                    movieMeta.MovieId = movieDto.Id;

                    _context.MovieMetas.Add(movieMeta);
                }
                _context.SaveChanges();
            }

            //foreach (var kvp in blah)
            //{
            //    var movie = kvp.Key;
            //    var movieMeta = kvp.Value;

            //    movieMeta.Movie = movie;
            //    _context.Update(movieMeta);

            //    //movie.Meta = movieMeta;
            //    //_context.Update(movie);

            //    //movieMeta.Movie = movie;
            //    //_context.MovieMetas.Add(movieMeta);
            //}
            //_context.SaveChanges();

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

                    var movieResult = FindMovieSearchResultByDirector(movieDto, possibleDirector, ref directorFilmography);
                    movieDictionary[movieTitle] = movieResult;
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

                var foundMovieByTitle = FindMovieSearchResultByTitle(movieDto);
                movieDictionary[movieTitle] = foundMovieByTitle;
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
                return false;

            var regex = new Regex("[^a-zA-Z0-9 -]");
            var movieSearchResultTitle = regex.Replace(movieSearchResult.title, "").Trim();
            var movieSearchResultOriginalTitle = regex.Replace(movieSearchResult.original_title, "").Trim();
            var movieDtoTitle = regex.Replace(movieDto.Title, "").Trim();

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

        private MovieSearchResult FindMovieSearchResultByDirector(MovieDto movieDto, PersonSearchResult possibleDirector, ref PersonFilmography directorFilmography)
        {
            var movieTitle = movieDto.Title;

            // Try to find the movie in the person search's 'known_for' property
            var foundMovieSearchResult = possibleDirector.known_for.Find(kf => FindFoundKnownForMovie(kf, movieDto));
            if (foundMovieSearchResult != null)
            {
                return foundMovieSearchResult;
            }

            // Try to find the movie in the director's filmography
            if (directorFilmography.id == 0)
            {
                var filmography = GetMovieCreditsByPersonApiId(possibleDirector.id);
                if (filmography.id > 0)
                {
                    directorFilmography = filmography;
                } else
                {
                    return null;
                }
            }

            var foundFilmographyResult = directorFilmography.crew.Find(filmographyCrewResult => FindFoundKnownForMovie(filmographyCrewResult, movieDto, 10));

            return foundFilmographyResult;
        }

        private MovieSearchResult FindMovieSearchResultByTitle(MovieDto movieDto)
        {
            var movieTitle = movieDto.Title;
            var movieSearch = SearchMovieByTitle(movieTitle);
            var movieSearchResults = movieSearch.results;

            var foundMovie = movieSearchResults.Find(movieResult => FindFoundKnownForMovie(movieResult, movieDto));

            if (foundMovie != null)
                return foundMovie;

            if (movieSearch.total_pages <= 1)
                return null;

            var page = 2;
            while (page < movieSearch.total_pages)
            {
                var innerResponse = SearchMovieByTitle(movieTitle, page);
                var innerResult = innerResponse.results;
                var innerFoundMovie = innerResult.Find(movieResult => FindFoundKnownForMovie(movieResult, movieDto));

                if (foundMovie != null)
                {
                    return foundMovie;
                }
                page++;
            }

            return null;
        }

        private bool FindContainedTitles (string movieTitleOne, string movieTitleTwo)
        {
            if (movieTitleOne.Length > 5 && movieTitleTwo.Length > 5)
            {
                return movieTitleOne.Contains(movieTitleTwo);
            }
            return false;
        }      

        private int GetYearDifference (int yearA, int yearB)
        {
            return yearA > yearB? yearA - yearB: yearB - yearA;
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

        public PersonDetails GetPersonDetailsByPersonApiId(int personId)
        {
            var url = $"person/{personId}";
            var response = GetResponseFromMovieApi(url, null);
            var personDetailsResponse = JsonSerializer.Deserialize<PersonDetails>(response);
            return personDetailsResponse;
        }

        public MovieSearch SearchMovieByTitle (string title, int page = 0)
        {
            var response = GetResponseFromMovieApi("search/movie", title, page);
            var movieSearchResponse = JsonSerializer.Deserialize<MovieSearch>(response);

            return movieSearchResponse;
        }

        public Configuration CollectMovieMetaConfigurationData ()
        {
            var response = GetResponseFromMovieApi("configuration");
            var configuration = JsonSerializer.Deserialize<Configuration>(response);
            return configuration;
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
