using System.Collections;
using System.Linq.Expressions;
using Application.Services;
using AutoMapper;
using Domain;
using Domain.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using Moq;
using NUnit.Framework;

namespace Tests.Services;

public class FlightServiceTests
{
    [Test]
    public async Task GetAllAsync_FlightsFound_Should_ReturnsFlights()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var repositoryMock = new Mock<IFlightRepository>();
        var mapperMock = new Mock<IMapper>();

        var responseDto = new List<FlightDto>{};
        responseDto.Add(new FlightDto());
        
        var service = new FlightService(unitOfWorkMock.Object, mapperMock.Object);

        repositoryMock.Setup(v => v.ExistAsync(It.IsAny<Expression<Func<Flight, bool>>>()))
            .ReturnsAsync(true);

        repositoryMock.Setup(x => x.GetAllAsync(null, null))
            .ReturnsAsync(new List<Flight>());

        mapperMock.Setup(x => x.Map<FlightDto>(It.IsAny<Flight>()))
            .Returns(new FlightDto());

        unitOfWorkMock.Setup(v => v.FlightRepository)
            .Returns(repositoryMock.Object);

        // Act 
        var result = await service.GetAllAsync();
        
        // Assert
        Assert.AreEqual(responseDto , result);
    }
}