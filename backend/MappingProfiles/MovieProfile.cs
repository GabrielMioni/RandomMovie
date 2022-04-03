using AutoMapper;
using backend.Dtos;
using backend.Models;
using backend.Models.Deserializers;
using backend.Models.Filters;
using System.Linq;

namespace backend.MappingProfiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, MovieDto>()
                .ForMember(dest => dest.Directors, opt => opt.MapFrom(src => src.Movie_Directors.Select(md => md.Director)))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Movie_Genres.Select(mg => mg.Genre)))
                .ForMember(dest => dest.Credits, opt => opt.MapFrom(src => src.Movie_Person.Select(mg => mg.Person)));
            CreateMap<MovieDto, Movie>();

            CreateMap<Director, DirectorDto>();

            CreateMap<Movie_Director, MovieDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Movie.Id))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Movie.Country))
                .ForMember(dest => dest.Decade, opt => opt.MapFrom(src => src.Movie.Decade))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Movie.Title))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Movie.Year))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Movie.Movie_Genres.Select(mg => mg.Genre)));

            CreateMap<MovieSearchResult, MovieMeta>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ApiId, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.OriginalLanguage, opt => opt.MapFrom(src => src.original_language))
                .ForMember(dest => dest.OriginalTitle, opt => opt.MapFrom(src => src.original_title))
                .ForMember(dest => dest.Overview, opt => opt.MapFrom(src => src.overview))
                .ForMember(dest => dest.PosterPath, opt => opt.MapFrom(src => src.poster_path))
                .ForMember(dest => dest.ReleasedDate, opt => opt.MapFrom(src => src.release_date))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.title));

            CreateMap<CreditPerson, Person>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ApiId, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.OriginalName, opt => opt.MapFrom(src => src.original_name))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.KnownFor, opt => opt.MapFrom(src => src.known_for_department))
                .ForMember(dest => dest.ProfilePath, opt => opt.MapFrom(src => src.profile_path));

            CreateMap<Person, PersonDto>();            
        }
    }
}
