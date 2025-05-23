using pd311_web_api.DAL.Entities;

namespace pd311_web_api.DAL.Repositories.Manufactures
{
    public interface IManufactureRepository
        : IGenericRepository<Manufacture, string>
    {
        Task<Manufacture?> GetByNameAsync(string name);
    }
}
