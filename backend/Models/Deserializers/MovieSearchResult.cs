namespace backend.Models.Deserializers
{
    public class MovieSearchResult
    {
        public int id { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public string overview { get; set; }
        public string poster_path { get; set; }
        public string title { get; set; }
    }
}
