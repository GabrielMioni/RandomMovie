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
        public List<Director> Directors { get; set; }
        public List<Genre> Genres { get; set; }
    }
}