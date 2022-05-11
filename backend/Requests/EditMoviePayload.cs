using System.Collections.Generic;

namespace backend.Requests
{
    public class EditMoviePayload
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public List<int> DirectorIds { get; set; }
        public List<int> GenreIds { get; set; }
        public List<int> CreditIds { get; set; }
    }
}
