using Domain.DTOs;
using FluentValidation.TestHelper;
using Infrastructure.Validators;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;

namespace Tests.Validators;

public class UserValidatorTests
{
    private UserValidator validator;
    
    [SetUp]
    public void Setup()
    {
        validator = new UserValidator();
    }
    
    [Test]
    public void Should_have_error_when_Username_is_null()
    {
        var model = new UserDto { Username = null };
        var result = validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(u => u.Username);
    }
}