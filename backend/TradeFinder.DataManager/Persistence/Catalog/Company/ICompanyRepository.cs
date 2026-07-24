namespace TradeFinder.DataManager;

public interface ICompanyRepository
{
    Task<List<Company>> GetCompanies();
}