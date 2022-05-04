using AngleSharp.Dom;
using AutoMapper;
using backend.Data;
using backend.Dtos;
using backend.Extensions;
using backend.Models;
using backend.Models.Filters;
using backend.Requests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services
{
    public class MovieService
    {
        private ApplicationDbContext _context;
        private HtmlReader _htmlReader;
        private IMapper _mapper;

        public MovieService(ApplicationDbContext context, HtmlReader htmlReader, IMapper mapper)
        {
            _context = context;
            _htmlReader = htmlReader;
            _mapper = mapper;
        }

        private MovieDto MapMovieDtoWithCredits(Movie movie)
        {
            var movieDto = _mapper.Map<MovieDto>(movie);

            foreach (var person in movie.Movie_Person)
            {
                var personId = person.PersonId;
                var index = movieDto.Credits.FindIndex(c => c.Id == personId);
                if (index > -1)
                {
                    movieDto.Credits[index].Character = person.Character;
                }
            }

            return movieDto;
        }

        public GetMoviesPaginatedResponse GetMoviesPaginated(GetMoviesPaginatedRequest request)
        {
            var page = request.Page <= 0 ? 1 : request.Page;
            var itemsPerPage = request.ItemsPerPage <= 0 ? 5 : request.ItemsPerPage;

            var skip = (page - 1) * itemsPerPage;

            var sortDesc = request.SortDesc;
            var sortBy = request.SortBy;

            var totalMovieCount = _context.Movies.Count();

            var moviesIncludeQueryable = _context.Movies
                .Include(m => m.Country)
                .Include(m => m.Decade)
                .Include(m => m.Meta)
                .Include(m => m.Movie_Directors)
                .ThenInclude(md => md.Director)
                .Include(m => m.Movie_Genres)
                .ThenInclude(mg => mg.Genre)
                .Include(m => m.Movie_Person)
                .ThenInclude(md => md.Person)
                .AsQueryable();

            if (sortBy.Count > 0)
            {
                var orderBy = SetMovieOrderBy(moviesIncludeQueryable, sortBy[0], sortDesc[0]);

                if (sortBy.Count > 1)
                {
                    var sortIndex = 1;

                    while (sortIndex <= sortBy.Count -1)
                    {
                        var by = sortBy[sortIndex];
                        var desc = sortDesc[sortIndex];

                        orderBy = SetMovieThenOrderBy(orderBy, by, desc);
                        sortIndex++;
                    }
                }

                moviesIncludeQueryable = orderBy.AsQueryable();
            }

            var skipTakeQueryable = moviesIncludeQueryable.Skip(skip).Take(itemsPerPage);

            var movies = skipTakeQueryable.ToList();

            var movieDtos = new List<MovieDto>();

            foreach (var movie in movies)
            {
                var movieDto = MapMovieDtoWithCredits(movie);
                movieDtos.Add(movieDto);
            }

            var total = movieDtos.Count;
            var pageCount = Decimal.Divide(totalMovieCount, itemsPerPage);

            return new GetMoviesPaginatedResponse
            {
                Movies = movieDtos,
                Total = totalMovieCount,
                PageCount = (int)Math.Ceiling(pageCount)
            };
        }

        private IOrderedEnumerable<Movie> SetMovieOrderBy (IQueryable<Movie> moviesQueryable, string sortBy, bool sortDesc)
        {
            switch (sortBy)
            {
                case "id":
                    return sortDesc
                        ? moviesQueryable.OrderByDescending(MovieColumnOrder.IdColumn)
                        : moviesQueryable.OrderBy(MovieColumnOrder.IdColumn);
                case "country":
                    return sortDesc
                        ? moviesQueryable.OrderByDescending(MovieColumnOrder.CountryColumn)
                        : moviesQueryable.OrderBy(MovieColumnOrder.CountryColumn);
                case "directors":
                    return sortDesc
                        ? moviesQueryable.OrderByDescending(MovieColumnOrder.DirectorColumn)
                        : moviesQueryable.OrderBy(MovieColumnOrder.DirectorColumn);
                case "genres":
                    return sortDesc
                        ? moviesQueryable.OrderByDescending(MovieColumnOrder.GenreColumn)
                        : moviesQueryable.OrderBy(MovieColumnOrder.GenreColumn);
                case "year":
                    return sortDesc
                        ? moviesQueryable.OrderByDescending(MovieColumnOrder.YearColumn)
                        : moviesQueryable.OrderBy(MovieColumnOrder.YearColumn);
                case "title":
                    return sortDesc
                        ? moviesQueryable.OrderByDescending(MovieColumnOrder.TitleColumn)
                        : moviesQueryable.OrderBy(MovieColumnOrder.TitleColumn);
                default:
                    return null;
            }
        }

        private IOrderedEnumerable<Movie> SetMovieThenOrderBy(IOrderedEnumerable<Movie> moviesQueryable, string sortBy, bool sortDesc)
        {
            switch (sortBy)
            {
                case "id":
                    return sortDesc
                        ? moviesQueryable.ThenByDescending(MovieColumnOrder.IdColumn)
                        : moviesQueryable.ThenBy(MovieColumnOrder.IdColumn);
                case "country":
                    return sortDesc
                        ? moviesQueryable.ThenByDescending(MovieColumnOrder.CountryColumn)
                        : moviesQueryable.ThenBy(MovieColumnOrder.CountryColumn);
                case "directors":
                    return sortDesc
                        ? moviesQueryable.ThenByDescending(MovieColumnOrder.DirectorColumn)
                        : moviesQueryable.ThenBy(MovieColumnOrder.DirectorColumn);
                case "genres":
                    return sortDesc
                        ? moviesQueryable.ThenByDescending(MovieColumnOrder.GenreColumn)
                        : moviesQueryable.ThenBy(MovieColumnOrder.GenreColumn);
                case "year":
                    return sortDesc
                        ? moviesQueryable.ThenByDescending(MovieColumnOrder.YearColumn)
                        : moviesQueryable.ThenBy(MovieColumnOrder.YearColumn);
                case "title":
                    return sortDesc
                        ? moviesQueryable.OrderByDescending(MovieColumnOrder.TitleColumn)
                        : moviesQueryable.OrderBy(MovieColumnOrder.TitleColumn);
                default:
                    return null;
            }
        }

        public MovieDto GetMovieById(int movieId)
        {
            var movie = _context.Movies
                .Include(m => m.Country)
                .Include(m => m.Decade)
                .Include(m => m.Meta)
                .Include(m => m.Movie_Directors)
                .ThenInclude(md => md.Director)
                .Include(m => m.Movie_Genres)
                .ThenInclude(mg => mg.Genre)
                .Include(m => m.Movie_Person)
                .ThenInclude(md => md.Person)
                .FirstOrDefault(m => m.Id == movieId);
            //.FirstOrDefault(m => m.Id == 2580);

            if (movie == null)
            {
                return null;
            }

            var movieDto = _mapper.Map<MovieDto>(movie);

            foreach (var person in movie.Movie_Person)
            {
                var personId = person.PersonId;
                var index = movieDto.Credits.FindIndex(c => c.Id == personId);
                if (index > -1)
                {
                    movieDto.Credits[index].Character = person.Character;
                }
            }

            return movieDto;
        }

        public MovieDto GetRandomMovieDto(RandomMovieRequest req = null)
        {
            var random = new Random();

            var movies = GetMoviesByRequestData(req);
            var moviesCount = movies.Count();

            if (moviesCount <= 0)
                return null;

            var randomIndex = random.Next(0, moviesCount);
            var movie = movies[randomIndex];

            var movieDto = _mapper.Map<MovieDto>(movie);

            foreach (var person in movie.Movie_Person)
            {
                var personId = person.PersonId;
                var index = movieDto.Credits.FindIndex(c => c.Id == personId);
                if (index > -1)
                {
                    movieDto.Credits[index].Character = person.Character;
                }
            }

            return movieDto;
        }

        public List<Movie> GetMoviesByRequestData(RandomMovieRequest req = null)
        {
            IQueryable<Movie> movieQuery = _context.Movies
                .Include(m => m.Country)
                .Include(m => m.Decade)
                .Include(m => m.Meta)
                .Include(m => m.Movie_Directors)
                .ThenInclude(md => md.Director)
                .Include(m => m.Movie_Genres)
                .ThenInclude(mg => mg.Genre)
                .Include(m => m.Movie_Person)
                .ThenInclude(md => md.Person)
                .AsQueryable();

            var countryIds = req.CountryIds ?? new List<int>();
            var decadeIds = req.DecadeIds ?? new List<int>();
            var directorIds = req.DirectorIds ?? new List<int>();
            var genreIds = req.GenreIds ?? new List<int>();

            if (countryIds.Any())
                movieQuery = movieQuery.Where(m => countryIds.Contains(m.Country.Id));

            if (decadeIds.Any())
                movieQuery = movieQuery.Where(m => decadeIds.Contains(m.Decade.Id));

            if (directorIds.Any())
                movieQuery = movieQuery.Where(m => m.Movie_Directors.Any(md => directorIds.Contains(md.DirectorId)));

            if (genreIds.Any())
                movieQuery = movieQuery.Where(m => m.Movie_Genres.Any(mg => genreIds.Contains(mg.GenreId)));

            return movieQuery.ToList();
        }

        public async Task SaveMoviesAsync()
        {
            var movieDtos = await GetAllMoviesAsync();

            await _context.TruncateTable("Movies");
            await _context.TruncateTable("Movie_Directors");
            await _context.TruncateTable("Movie_Genres");
            await _context.TruncateTable("MovieMetas");

            var counter = 0;

            foreach (var movieDto in movieDtos)
            {
                AddMovie(movieDto);
                counter++;

                if (counter >= 100)
                {
                    await _context.SaveChangesAsync();
                    counter = 0;
                }
            }
        }

        private async Task<List<MovieDto>> GetAllMoviesAsync()
        {
            var genres = _context.Genres.ToList().OrderBy(g => g.Name);

            //var genres = new List<Genre>();
            //genres.Add(new Genre { Id = 1, Name = "Avant-garde" });
            //genres.Add(new Genre { Id = 1, Name = "Action-Adventure" });
            //genres.Add(new Genre { Id = 1, Name = "Animation" });
            //genres.Add(new Genre { Id = 1, Name = "Comedy" });
            //genres.Add(new Genre { Id = 1, Name = "Crime" });
            //genres.Add(new Genre { Id = 1, Name = "Documentary" });

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

            return movieDtos;
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
                var directorDtos = GetDirectorDtosFromString(directorString);
                var genreDtos = new List<GenreDto> { _mapper.Map<GenreDto>(genre) };

                var movieDto = new MovieDto
                {
                    Country = country,
                    Decade = decade,
                    Directors = directorDtos,
                    Genres = genreDtos,
                    Title = movieTitleString,
                    Year = Int32.Parse(yearString)
                };
                movieList.Add(movieDto);
            }

            return movieList;
        }

        private Decade GetDecadeFromString(string yearString)
        {
            var year = Int32.Parse(yearString);
            var decade = _context.Decades.FirstOrDefault(d => year >= d.DecadeInt && year <= d.DecadeInt + 9);
            return decade;
        }

        private List<DirectorDto> GetDirectorDtosFromString(string directorsString)
        {
            // var directorNames = _filterService.GetDirectorNamesFromString(directorsString);
            var directorNames = directorsString.GetDirectorNamesFromString();
            return directorNames.Select(name => _mapper.Map<DirectorDto>(GetDirectorByName(name))).ToList();
        }

        private Director GetDirectorByName(string directorName)
        {
            return _context.Directors.FirstOrDefault(d => d.Name == directorName);
        }

        private string GetInnerHtml(IElement elm, string querySelector)
        {
            return elm.QuerySelector(querySelector).InnerHtml.Trim();
        }

        public void AddMovie(MovieDto movieDto)
        {
            var movieTitle = movieDto.Title;

            if (movieTitle == null || movieDto.Decade == null)
            {
                Console.WriteLine(movieTitle, movieDto.Decade);
            }

            var movieInDb = _context.Movies.FirstOrDefault(m => m.Title == movieTitle && m.Decade.Id == movieDto.Decade.Id);

            if (movieInDb != null)
            {
                UpdateMovie(movieInDb, movieDto);
                return;
            }

            var movie = new Movie
            {
                Title = movieDto.Title,
                Decade = movieDto.Decade,
                Country = movieDto.Country,
                Year = movieDto.Year
            };
            _context.Movies.Add(movie);
            _context.SaveChanges();

            UpdateMovie(movie, movieDto);
        }

        private void UpdateMovie(Movie movie, MovieDto movieDto)
        {
            AddDirectorsToMovie(movie, movieDto);
            AddGenresToMovie(movie, movieDto);
        }

        private void AddGenresToMovie(Movie movie, MovieDto movieDto)
        {
            var genres = movieDto.Genres;

            foreach (var genre in genres)
            {
                var movie_genreInDb = _context.Movie_Genres.Any(mg => mg.MovieId == movie.Id && mg.GenreId == genre.Id);

                if (movie_genreInDb)
                {
                    continue;
                }

                var movie_genre = new Movie_Genre
                {
                    MovieId = movie.Id,
                    GenreId = genre.Id,
                };

                _context.Movie_Genres.Add(movie_genre);
            };
        }

        private void AddDirectorsToMovie(Movie movie, MovieDto movieDto)
        {
            var directors = movieDto.Directors;

            foreach(var director in directors)
            {
                var director_movieInDb = _context.Movie_Directors.Any(md => md.MovieId == movie.Id && md.DirectorId == director.Id);

                if (director_movieInDb)
                {
                    continue;
                }

                var movie_director = new Movie_Director
                {
                    MovieId = movie.Id,
                    DirectorId = director.Id,
                };

                _context.Movie_Directors.Add(movie_director);
            }
        }
    }
}
