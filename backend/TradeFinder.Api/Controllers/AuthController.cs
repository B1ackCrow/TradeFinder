using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeFinder.Business.Authentication;
using TradeFinder.Contracts.Authentication;

namespace TradeFinder.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly AuthenticationService _authenticationService;

    public AuthenticationController(ILogger<ProductsController> logger,  AuthenticationService authenticationService)
    {
        _logger = logger;
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        try
        {
            string token = await _authenticationService.Register(request.Email, request.Password);
            return Ok(token);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        try
        {
            var token = await _authenticationService.Login(
                loginRequest.Email, loginRequest.Password);

            Response.Cookies.Append(
                "accessToken", 
                token,
                new CookieOptions()
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.Now.AddDays(7)
                });
            
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPost("test")]
    public IActionResult Test()
    {
        return Ok("Hello, World!");
    }
}