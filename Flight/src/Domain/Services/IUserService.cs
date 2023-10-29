using Domain.DTOs;

namespace Domain.Services;

public interface IUserService
{
    Task<UserDto> AddAsync(UserDto userDto);
    Task UpdateAsync(UserDto userDto);
    Task DeleteAsync(long id);
    Task<UserDto> GetByIdAsync(long id);
}