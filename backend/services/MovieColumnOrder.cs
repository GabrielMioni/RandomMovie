using backend.Models;
using System.Linq;

namespace backend.Services
{
    public static class MovieColumnOrder
    {
        public static int IdColumn(Movie movie)
        {
            return movie.Id;
        }

        public static string CountryColumn(Movie movie)
        {
            return movie.Country.Name;
        }

        public static string DirectorColumn(Movie movie)
        {
            var movieDirectors = movie.Movie_Directors.FirstOrDefault();

            if (movieDirectors == null)
                return "";

            return movieDirectors.Director.Name;
        }

        public static string GenreColumn(Movie movie)
        {
            var movieGenres = movie.Movie_Genres.FirstOrDefault();

            if (movieGenres == null)
                return "";

            return movieGenres.Genre.Name;
        }

        public static int YearColumn(Movie movie)
        {
            return movie.Year;
        }

        public static string TitleColumn(Movie movie)
        {
            return movie.Title;
        }
    }
}
