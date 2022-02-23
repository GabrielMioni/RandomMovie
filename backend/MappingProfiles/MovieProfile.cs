using AutoMapper;
using backend.Dtos;
using backend.Models;
using System.Linq;

namespace backend.MappingProfiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, MovieDto>()
                .ForMember(dest => dest.Directors, opt => opt.MapFrom(src => src.Movie_Directors.Select(md => md.Director)))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Movie_Genres.Select(mg => mg.Genre)));
            CreateMap<MovieDto, Movie>();
        }
    }
}
