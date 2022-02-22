using AutoMapper;
using backend.Dtos;
using backend.Models.Filters;

namespace backend.MappingProfiles
{
    public class FilterProfile : Profile
    {
        public FilterProfile()
        {
            CreateMap<Director, DirectorDto>();
            CreateMap<DirectorDto, Director>();

            CreateMap<Genre, GenreDto>();
            CreateMap<GenreDto, Genre>();
        }
    }
}
