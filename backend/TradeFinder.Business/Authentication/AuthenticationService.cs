using TradeFinder.DataManager;

namespace TradeFinder.Business.Authentication;

public class AuthenticationService
{
    private readonly IAuthRepository _authRepository;
    private readonly JwtTokenGenerator _jwt;
    
    public AuthenticationService(IAuthRepository authRepository, JwtTokenGenerator jwt)
    {
        _authRepository = authRepository;
        _jwt = jwt;
    }

    public async Task<string> Register(string email, string password)
    {
        if (await _authRepository.ExistsByEmail(email))
        {
            throw new Exception("Данная почта уже зарегистрирована");
        }

        var user = new User
        {
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            IsActive = true
        };
        
        Guid userId = await _authRepository.Create(user);
        return _jwt.Generate(userId, user.Email);
    }
    
    public async Task<string> Login(string email, string password)
    {
        var user = await _authRepository.GetByEmail(email);
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new Exception("Неверный логин или пароль");

        if (!user.IsActive)
            throw new Exception("Учетная запись этого пользователя заблокирована");
        
        return _jwt.Generate(user.Id, user.Email);
    }
}