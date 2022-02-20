using backend.Data;
using backend.Dtos;
using backend.Models;
using backend.Models.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services
{
    public class MovieService
    {
        private ApplicationDbContext _context;

        public MovieService(ApplicationDbContext context)
        {
            _context = context;
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

        private void UpdateMovie(Movie movie, MovieDto movieDto)
        {
            AddDirectorsToMovie(movie, movieDto);
            AddGenresToMovie(movie, movieDto);
        }
    }
}
