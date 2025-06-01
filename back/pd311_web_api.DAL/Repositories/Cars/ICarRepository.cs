using pd311_web_api.DAL.Entities;

namespace pd311_web_api.DAL.Repositories.Cars
{
    public interface ICarRepository 
        : IGenericRepository<Car, string>
    {
        IQueryable<Car> GetCars(int page, int pageSize, string? manufacture);
        IQueryable<Car> GetCarsByManufacture(string manufacture);
    }
}
