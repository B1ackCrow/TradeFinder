using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace TradeFinder.Business.Authentication;

public class JwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string Generate(Guid userId, string email)
    {
        var jwt = _configuration.GetSection("Jwt");


        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email)
        };


        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt["Key"]!)
        );


        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );


        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                int.Parse(jwt["ExpiresMinutes"]!)
            ),
            signingCredentials: credentials
        );


        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}