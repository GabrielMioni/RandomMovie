using backend.Models.Filters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public Country Country { get; set; }
        public Decade Decade { get; set; }
        public string Title { get; set; }

        public List<Movie_Director> Movie_Directors { get; set; }
        public List<Movie_Genre> Movie_Genres { get; set; }
    }
}
