using AngleSharp.Dom;
using AutoMapper;
using backend.Data;
using backend.Dtos;
using backend.Extensions;
using backend.Models;
using backend.Models.Filters;
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

        public List<MovieDto> GetMovie()
        {
            var movies = _context.Movies
                .Include(m => m.Country)
                .Include(m => m.Decade)
                .Include(m => m.Movie_Directors)
                .ThenInclude(md => md.Director)
                .Include(m => m.Movie_Genres)
                .ThenInclude(mg => mg.Genre)
                .ToList();

            return movies.Select(m => _mapper.Map<MovieDto>(m)).ToList();
        }

        public async Task SaveMoviesAsync()
        {
            var movieDtos = await GetAllMoviesAsync();

            await _context.TruncateTable("Movies");
            await _context.TruncateTable("Movie_Directors");
            await _context.TruncateTable("Movie_Genres");

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
