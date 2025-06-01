using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pd311_web_api.BLL.DTOs.Manufactures;
using pd311_web_api.BLL.Services.Image;
using pd311_web_api.DAL;
using pd311_web_api.DAL.Entities;
using pd311_web_api.DAL.Repositories.Manufactures;

namespace pd311_web_api.BLL.Services.Manufactures
{
    public class ManufactureService : IManufactureService
    {
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly IManufactureRepository _manufactureRepository;
        private readonly AppDbContext _context;
        private readonly ILogger<ManufactureService> _logger;

        public ManufactureService(IMapper mapper, AppDbContext context, IImageService imageService, IManufactureRepository manufactureRepository, ILogger<ManufactureService> logger)
        {
            _mapper = mapper;
            _context = context;
            _imageService = imageService;
            _manufactureRepository = manufactureRepository;
            _logger = logger;
        }

        public async Task<bool> CreateAsync(CreateManufactureDto dto)
        {
            var entity = _mapper.Map<Manufacture>(dto);
            string? imageName = null;

            if(dto.Image != null)
            {
                _imageService.CreateImagesDirectory(Settings.ManufacturesImagesPath);

                imageName = await _imageService.SaveImageAsync(dto.Image, Settings.ManufacturesImagesPath);
                if(imageName != null)
                {
                    imageName = Path.Combine(Settings.ManufacturesImagesPath, imageName);
                }
            }

            entity.Image = imageName;
            var result = await _manufactureRepository.CreateAsync(entity);

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _context.Manufactures.FirstOrDefaultAsync(m => m.Id == id);

            if(entity == null)
            {
                return false;
            }

            if(!string.IsNullOrEmpty(entity.Image))
            {
                _imageService.DeleteImage(entity.Image);
            }

            _context.Manufactures.Remove(entity);
            var result = await _context.SaveChangesAsync();
            return result != 0;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var entities = await _manufactureRepository
                .GetAll()
                .ToListAsync();

            var dtos = _mapper.Map<List<ManufactureDto>>(entities);

            string tab = "      ";
            string names = string.Join($"\n{tab}", dtos.Select(d => d.Name));

            _logger.LogInformation(new EventId(200, "Ok"), $"Manufactures received: {DateTime.Now.ToString("dd.MM.yyyy")}");
            _logger.LogInformation(new EventId(200, "Ok"), names);

            return new ServiceResponse("Виробники отримано", true, dtos);
        }

        public async Task<ServiceResponse> GetByIdAsync(string id)
        {
            var entity = await _manufactureRepository.GetByIdAsync(id);
            var dto = _mapper.Map<ManufactureDto>(entity);
            return new ServiceResponse("Виробника отримано", true, dto);
        }

        public async Task<bool> UpdateAsync(UpdateManufactureDto dto)
        {
            var entity = await _context.Manufactures
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == dto.Id);

            if (entity == null)
            {
                return false;
            }

            entity = _mapper.Map(dto, entity);

            if(dto.Image != null)
            {
                if (!string.IsNullOrEmpty(entity.Image))
                {
                    _imageService.DeleteImage(entity.Image);
                }

                var imageName = await _imageService.SaveImageAsync(dto.Image, Settings.ManufacturesImagesPath);
                if (imageName != null)
                {
                    imageName = Path.Combine(Settings.ManufacturesImagesPath, imageName);
                }
                entity.Image = imageName;
            }

            _context.Update(entity);
            var result = await _context.SaveChangesAsync();
            return result != 0;
        }
    }
}
