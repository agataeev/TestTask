namespace Domain.DTOs;

public class UserDto
{
    public long Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public long RoleId { get; set; }
}