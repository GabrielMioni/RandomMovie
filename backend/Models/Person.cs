using System.Collections.Generic;

namespace backend.Models
{
    public class Person
    {
        public int Id { get; set; }
        public int ApiId { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string KnownFor { get; set; }
        public string ProfilePath { get; set; }
        public List<Movie_Person> Movie_Persons { get; set; }
    }
}
