namespace TradeFinder.DataManager;

public interface ICompanyRepository
{
    Task<List<Company>> GetAll();
    Task<Company?> GetById(Guid id);

}