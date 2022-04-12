namespace backend.Requests
{
    public class GetMoviesPaginatedRequest
    {
        public int Page { get; set; }
        public int Take { get; set; }
    }
}
