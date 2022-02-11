using System.Collections.Generic;

namespace backend.Models.Filters
{
    public class Director : AbstractFilter
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<Movie_Director> Movie_Directors { get; set; }
    }
}
