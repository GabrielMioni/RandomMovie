using System.Collections.Generic;

namespace backend.Models.Deserializers
{
    public class MovieSearch
    {
        public int page { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }
        public List<MovieSearchResult> results { get; set; }
    }
}
