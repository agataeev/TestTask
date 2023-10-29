using Domain.DTOs;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public AuthController(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(AuthDto dto)
    {
        JwtResponseDto jwt;
        try
        {
            jwt = await _authService.AuthenticateWithRefreshAsync(dto.Username, dto.Password);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
        return Ok(jwt);
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshDto dto)
    {
        var jwt = await _authService.RefreshAccessToken(dto.RefreshToken);
        return Ok(jwt);
    }
}