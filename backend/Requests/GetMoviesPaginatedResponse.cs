using backend.Dtos;
using System.Collections.Generic;

namespace backend.Requests
{
    public class GetMoviesPaginatedResponse
    {
        public int Total { get; set; }
        public int PageCount { get; set; }
        public List<MovieDto> Movies { get; set; }
    }
}
