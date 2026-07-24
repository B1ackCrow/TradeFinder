namespace TradeFinder.DataManager;

public sealed class DataAccess : IDataAccess
{
    private ICompanyRepository _companyRepository;
    
    private readonly ConnectionFactory _connectionFactory;

    public DataAccess(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    
    public ICompanyRepository Companies => _companyRepository ??= new CompanyRepository(_connectionFactory);
}