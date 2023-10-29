using Domain.DTOs;
using FluentValidation;

namespace Infrastructure.Validators;

public class UserValidator : AbstractValidator<UserDto>
{
    public UserValidator()
    {
        RuleFor(x => x.Password)
            .NotNull()
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")
            .WithMessage("Пароль должен содержать не менее 8 и не более 15 символов и хотя бы 1 строчную, 1 прописную, 1 цифру");
        RuleFor(x=>x.Username).NotNull();

    }
    
    private bool Valid(string password)
    {
        return password.Length >= 8;
    }
}
