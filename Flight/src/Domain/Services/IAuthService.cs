using Domain.DTOs;

namespace Domain.Services;

public interface IAuthService
{
    Task<JwtResponseDto> AuthenticateWithRefreshAsync(string login, string password);
    Task<JwtResponseDto> RefreshAccessToken(string refreshToken);
}