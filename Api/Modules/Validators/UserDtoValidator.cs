using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(255).MinimumLength(3);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(255).MinimumLength(3);
        RuleFor(x => x.Username).NotEmpty().MaximumLength(255).MinimumLength(3);
    }
}

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(255).MinimumLength(3);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(255).MinimumLength(3);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Username).NotEmpty().MaximumLength(255).MinimumLength(3);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(16).MinimumLength(8);
    }
}

public class UserUpdatePasswordDtoValidator : AbstractValidator<UserUpdatePasswordDto>
{
    public UserUpdatePasswordDtoValidator()
    {
        RuleFor(x => x.CurrentPassword).NotEmpty().MaximumLength(16).MinimumLength(8);
        RuleFor(x => x.NewPassword).NotEmpty().MaximumLength(16).MinimumLength(8);
    }
}

public class UserUpdateEmailDtoValidator : AbstractValidator<UserUpdateEmailDto>
{
    public UserUpdateEmailDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}