using System.Collections.Generic;

namespace backend.Requests
{
    public class RandomMovieRequest
    {
        public List<int> CountryIds { get; set; }
        public List<int> DecadeIds { get; set; }
        public List<int> DirectorIds { get; set; }
        public List<int> GenreIds { get; set; }
    }
}
