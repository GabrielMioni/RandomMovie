using System.Collections.Generic;

namespace backend.Requests
{
    public class EditMoviePayload
    {
        public int CountryId { get; set; }
        public List<int> CreditIds { get; set; }
        public List<int> DirectorIds { get; set; }
        public List<int> GenreIds { get; set; }
        public int MovieId { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
    }
}
