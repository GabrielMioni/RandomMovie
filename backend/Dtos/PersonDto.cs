namespace backend.Dtos
{
    public class PersonDto
    {
        public int Id { get; set; }
        public int ApiId { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string Biography { get; set; }
        public string Birthday { get; set; }
        public string Deathday { get; set; }
        public string KnownFor { get; set; }
        public string ProfilePath { get; set; }
    }
}
