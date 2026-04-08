namespace Application.DTOs.Users;

public sealed class AddUserRequestValidator : AbstractValidator<AddUserRequest>
{
    public AddUserRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.");
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.");
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("A valid email is required.");
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("Username is required.");
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .WithMessage("Password must be at least 6 characters long.");
    }
}