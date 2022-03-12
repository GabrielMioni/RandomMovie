namespace backend.Models
{
    public class MovieMeta
    {
        public int Id { get; set; }
        public int ApiId { get; set; }
        public string OriginalTitle { get; set; }
        public string OriginalLanguage { get; set; }
        public string Overview { get; set; }
        public string PosterPath { get; set; }
        public string ReleasedDate { get; set; }
        public string Title { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
