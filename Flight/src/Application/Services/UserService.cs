using AutoMapper;
using Domain;
using Domain.DTOs;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Services;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher<User> passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<UserDto> AddAsync(UserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        
        var exists = await _unitOfWork.UserRepository.ExistAsync(x => x.Username == user.Username);
        if (exists)
            throw new ConflictException("The user already exists.");
        
        var role = await _unitOfWork.RoleRepository.GetByIdAsync(userDto.RoleId);
        if (role == null)
            throw new NotFoundException("The role doesn't exist.");
        
        user.RoleId = role.Id;
        user.Password = _passwordHasher.HashPassword(user, user.Password);
            
        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<UserDto>(user);
    }
    
    public async Task UpdateAsync(UserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        var role = await _unitOfWork.RoleRepository.GetByIdAsync(userDto.RoleId) ?? throw new NotFoundException("The role doesn't exist.");
        user.RoleId = role.Id;
        
        await _unitOfWork.UserRepository.UpdateAsync(user);
        await _unitOfWork.SaveAsync();
    }
    
    public async Task DeleteAsync(long id)
    {
        var exists = await _unitOfWork.UserRepository.ExistAsync(x => x.Id == id);
        if (!exists)
            throw new NotFoundException("The user doesn't exist.");
        
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        
        await _unitOfWork.UserRepository.DeleteAsync(user);
        await _unitOfWork.SaveAsync();
    }
    
    public async Task<UserDto> GetByIdAsync(long id)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        return _mapper.Map<UserDto>(user);
    }
}