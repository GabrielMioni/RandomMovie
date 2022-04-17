using System.Collections.Generic;

namespace backend.Requests
{
    public class GetMoviesPaginatedRequest
    {
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public List<string> SortBy { get; set; }
        public List<bool> SortDesc { get; set; }
    }
}
