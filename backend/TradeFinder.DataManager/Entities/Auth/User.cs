namespace TradeFinder.DataManager;

public class User
{
    public Guid Id { get; set; }
    public string? Login { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}