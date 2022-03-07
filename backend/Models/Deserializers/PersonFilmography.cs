using System.Collections.Generic;

namespace backend.Models.Deserializers
{
    public class PersonFilmography
    {
        public int id { get; set; }
        public List<MovieSearchResult> cast { get; set; }
        public List<MovieSearchResult> crew { get; set; }
    }
}
