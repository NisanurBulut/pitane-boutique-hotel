using AutoMapper;
using Pitane.DTO;
using pitaneAPI.Models;


namespace pitaneAPI.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<HotelCreateDto, Hotel>().ReverseMap();
            CreateMap<Hotel, HotelUpdateDto>().ReverseMap();
            CreateMap<HotelDto, Hotel>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<HotelAmenityCreateDto, HotelAmenity>().ReverseMap();
            CreateMap<HotelAmenityUpdateDto, HotelAmenity>().ReverseMap();
            CreateMap<HotelAmenityDto, HotelAmenity>();

            CreateMap<HotelAmenity, HotelAmenityDto>()
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel != null ? src.Hotel.Name : string.Empty));
        }
    }
}