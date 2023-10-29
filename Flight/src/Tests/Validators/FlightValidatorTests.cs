using Domain.DTOs;
using FluentValidation.TestHelper;
using Infrastructure.Validators;
using NUnit.Framework;

namespace Tests.Validators;

public class FlightValidatorTests
{
    private FlightValidator validator;

    [SetUp]
    public void Setup()
    {
        validator = new FlightValidator();
    }
    
    [Test]
    public void Should_have_error_when_ORIGIN_is_null()
    {
        var model = new FlightDto { Origin = null };
        var result = validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(f => f.Origin);
    }
}