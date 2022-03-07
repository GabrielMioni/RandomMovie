using AutoMapper;
using backend.Models;
using backend.Models.Deserializers;

namespace backend.MappingProfiles
{
    public class MovieMetaProfile : Profile
    {
        public MovieMetaProfile()
        {
            CreateMap<MovieSearchResult, MovieMeta>()
                .ForMember(dest => dest.OriginalTitle, opt => opt.MapFrom(src => src.original_title))
                .ForMember(dest => dest.OriginalLanguage, opt => opt.MapFrom(src => src.original_language))
                .ForMember(dest => dest.Overview, opt => opt.MapFrom(src => src.overview))
                .ForMember(dest => dest.PosterPath, opt => opt.MapFrom(src => src.poster_path))
                .ForMember(dest => dest.ReleasedDate, opt => opt.MapFrom(src => src.release_date))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.title));
        }
    }
}
