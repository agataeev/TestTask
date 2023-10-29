namespace Domain.DTOs;

public class JwtResponseDto
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime AccessTokenExpiration { get; set; }
    public IEnumerable<string> Permission { get; set; }
}