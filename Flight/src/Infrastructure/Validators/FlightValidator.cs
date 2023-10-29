using Domain.DTOs;
using FluentValidation;

namespace Infrastructure.Validators;

public class FlightValidator : AbstractValidator<FlightDto>
{
    public FlightValidator()
    {
        RuleFor(x => x.Origin).NotNull();
    }
}
