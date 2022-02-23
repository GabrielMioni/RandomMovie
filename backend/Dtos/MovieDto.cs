using backend.Models.Filters;
using System.Collections.Generic;

namespace backend.Dtos
{
    public class MovieDto
    {
        public int Id { get; set; }
        public Country Country { get; set; }
        public Decade Decade { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public List<DirectorDto> Directors { get; set; }
        public List<GenreDto> Genres { get; set; }
    }
}