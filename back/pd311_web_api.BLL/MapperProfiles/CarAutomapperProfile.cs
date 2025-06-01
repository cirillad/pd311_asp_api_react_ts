using AutoMapper;
using pd311_web_api.BLL.DTOs.Car;
using pd311_web_api.DAL.Entities;

namespace pd311_web_api.BLL.MapperProfiles
{
    public class CarAutomapperProfile : Profile
    {
        public CarAutomapperProfile()
        {
            // CreateCarDto -> Car
            CreateMap<CreateCarDto, Car>()
                .ForMember(dest => dest.Manufacture, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            // Car -> CarDto
            CreateMap<Car, CarDto>()
                .ForMember(dest => dest.Manufacture, opt => opt.MapFrom(src => src.Manufacture == null ? "" : src.Manufacture.Name))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => Path.Combine(i.Path, i.Name))));
        }
    }
}
