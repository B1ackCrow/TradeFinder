namespace TradeFinder.DataManager;

public interface IAuthRepository
{
    Task<bool> ExistsByEmail(string email);
    Task<User?> GetByEmail(string email);
    Task<User?> GetByLogin(string login);
    Task<Guid> Create(User user);
}