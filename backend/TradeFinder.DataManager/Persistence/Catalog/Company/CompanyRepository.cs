namespace TradeFinder.DataManager;

public class CompanyRepository : ICompanyRepository
{
    private readonly ConnectionFactory _connectionFactory;

    public CompanyRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<Company>> GetCompanies()
    {
        const string sql = """
                            SELECT *
                            FROM catalog.vw_companies
                           """;
        
        return new List<Company>();
    }

}