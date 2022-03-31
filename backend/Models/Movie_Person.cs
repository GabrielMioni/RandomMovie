namespace backend.Models
{
    public class Movie_Person
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
