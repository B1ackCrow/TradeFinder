namespace TradeFinder.DataManager;

public interface IDataAccess
{
    ICompanyRepository Companies { get; }
}