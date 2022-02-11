using System.Collections.Generic;

namespace backend.Models.Filters
{
    public class Genre : AbstractFilter
    {
        public List<Movie_Genre> Movie_Genres { get; set; }
    }
}
