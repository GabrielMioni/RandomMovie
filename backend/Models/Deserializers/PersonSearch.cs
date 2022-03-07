using System.Collections.Generic;

namespace backend.Models.Deserializers
{
    // TODO: Extract PersonSearchResult to its own file
    public class PersonSearchResult
    {
        public int id { get; set; }
        public List<MovieSearchResult> known_for { get; set; }
        public string known_for_department { get; set; }
        public string name { get; set; }
    }

    public class PersonSearch
    {
        public int page { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }
        public List<PersonSearchResult> results { get; set; }
    }
}
