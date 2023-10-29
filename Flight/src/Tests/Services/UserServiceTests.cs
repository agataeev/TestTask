using System.Linq.Expressions;
using Application.Services;
using AutoMapper;
using Domain;
using Domain.DTOs;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;

namespace Tests.Services;

public class UserServiceTests
{
    [Test]
    public void UpdateUser_UserNotFound_Should_ThrowConflictException()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userRepositoryMock = new Mock<IUserRepository>();
    

        var user = new UserDto
        {
            Id = 1,
            Username = "visitor",
            Password = "123456",
            RoleId = 4
        };

        var  mapperMock = new Mock<IMapper>();
        var service = new UserService(unitOfWorkMock.Object, mapperMock.Object, new PasswordHasher<User>());

        userRepositoryMock.Setup(v => v.ExistAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(false);

        unitOfWorkMock.Setup(v => v.UserRepository)
            .Returns(userRepositoryMock.Object);

        unitOfWorkMock.Setup(v => v.RoleRepository)
            .Returns(new Mock<IRoleRepository>().Object);
            

        // Act => Assert
        Assert.ThrowsAsync<NotFoundException>(() => service.UpdateAsync(user), "The user doesn't exist");
    }
}