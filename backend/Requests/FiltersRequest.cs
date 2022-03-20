using backend.Dtos;
using backend.Models.Filters;
using System.Collections.Generic;
using System.Linq;

namespace backend.Requests
{
    public class FiltersRequest
    {
        public List<Country> Countries { get; set; }
        public List<Decade> Decades { get; set; }
        public List<DirectorDto> Directors { get; set; }
        public List<GenreDto> Genres { get; set; }

        public FiltersRequest(List<Country> countries, List<Decade> decades, List<DirectorDto> directorDtos, List<GenreDto> genreDtos)
        {
            Countries = countries.OrderBy(country => country.Name).ToList();
            Decades = decades.OrderBy(decade => decade.Name).ToList();
            Directors = directorDtos.OrderBy(director => director.Name).ToList();
            Genres = genreDtos.OrderBy(genre => genre.Name).ToList();
        }
    }
}
