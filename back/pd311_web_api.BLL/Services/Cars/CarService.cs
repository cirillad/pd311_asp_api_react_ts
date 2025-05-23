using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using pd311_web_api.BLL.DTOs.Car;
using pd311_web_api.BLL.Services.Image;
using pd311_web_api.BLL.Services.Storage;
using pd311_web_api.DAL.Entities;
using pd311_web_api.DAL.Repositories.Cars;
using pd311_web_api.DAL.Repositories.Manufactures;

namespace pd311_web_api.BLL.Services.Cars
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly IManufactureRepository _manufactureRepository;
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;

        public CarService(ICarRepository carRepository, IMapper mapper, IManufactureRepository manufactureRepository, IStorageService storageService)
        {
            _carRepository = carRepository;
            _mapper = mapper;
            _manufactureRepository = manufactureRepository;
            _storageService = storageService;
        }

        public async Task<ServiceResponse> CreateAsync(CreateCarDto dto)
        {
            var entity = _mapper.Map<Car>(dto);

            if (!string.IsNullOrEmpty(dto.Manufacture))
            {
                entity.Manufacture = await _manufactureRepository
                    .GetByNameAsync(dto.Manufacture);
            }

            if(dto.Images.Count() > 0)
            {
                entity.Images = await SaveImagesAsync(dto.Images, entity.Id);
            }

            var result = await _carRepository.CreateAsync(entity);

            if(!result)
            {
                return new ServiceResponse("Не вдалося зберегти автомобіль");
            }

            return new ServiceResponse($"Автомобіль '{entity.Brand} {entity.Model}' додано", true);
        }

        public async Task<ServiceResponse> GetAllAsync(int page, int pageSize, string? manufacture)
        {
            int count = string.IsNullOrEmpty(manufacture)
                ? _carRepository.GetAll().Count()
                : _carRepository.GetCarsByManufacture(manufacture).Count();

            int pageCount = (int)Math.Ceiling((double)count / pageSize);

            page = page < 1 || page > pageCount ? 1 : page;

            var entities = await _carRepository
                .GetCars(page, pageSize, manufacture)
                .ToListAsync();

            var dtos = _mapper.Map<List<CarDto>>(entities);

            var listDto = new CarListDto
            {
                Cars = dtos,
                Page = page,
                PageCount = pageCount,
                TotalCount = count
            };

            return new ServiceResponse("Автомобілі отримано", true, listDto);
        }

        private async Task<List<CarImage>> SaveImagesAsync(List<IFormFile> images, string carId)
        {
            List<CarImage> carImages = [];

            var result = await _storageService.UploadImagesAsync(images, Path.Combine(Settings.CarsImagesPath, carId));
            foreach (string imagePath in result)
            {
                int index = imagePath.LastIndexOf('/');
                index = index == -1 ? imagePath.LastIndexOf('\\') : index;

                var carImage = new CarImage
                {
                    Path = imagePath,
                    Name = imagePath.Substring(index + 1)
                };
                carImages.Add(carImage);
            }
            return carImages;
        }
    }
}
