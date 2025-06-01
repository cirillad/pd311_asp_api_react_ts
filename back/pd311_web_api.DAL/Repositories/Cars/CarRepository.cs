using Microsoft.EntityFrameworkCore;
using pd311_web_api.DAL.Entities;

namespace pd311_web_api.DAL.Repositories.Cars
{
    public class CarRepository
        : GenericRepository<Car, string>,
        ICarRepository
    {
        public CarRepository(AppDbContext context)
            : base(context) { }

        public IQueryable<Car> GetCars(int page, int pageSize, string? manufacture)
        {
            var entities = string.IsNullOrEmpty(manufacture) 
                ? GetAll() 
                : GetCarsByManufacture(manufacture);

            entities = entities
                .Include(e => e.Manufacture)
                .Include(e => e.Images)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return entities;
        }

        public IQueryable<Car> GetCarsByManufacture(string manufacture)
        {
            var entities = GetAll()
                .Where(e => e.Manufacture == null ? false : e.Manufacture.Name.ToLower() == manufacture.ToLower());
            return entities;
        }
    }
}
