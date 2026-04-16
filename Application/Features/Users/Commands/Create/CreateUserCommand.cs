namespace Application.Features.Users.Commands.Create;

public sealed record CreateUserCommand(AddUserRequest Request) : ICommand<CreateUserResponse>;

public sealed class CreateUserCommandHandler(
    IAuthService authService,
    IDepartmentDomainService departmentDomainService) : ICommandHandler<CreateUserCommand, CreateUserResponse>
{
    public async Task<Result<CreateUserResponse>> HandleAsync(CreateUserCommand command, CancellationToken ct = default)
    {
        //TODO: validate the role and department existence before creating the user
        var newUser = new CreateUserRequest(
            command.Request.FirstName,
            command.Request.LastName,
            command.Request.Password,
            command.Request.Email,
            command.Request.UserName
        );

        var registerResult = await authService.RegisterAsync(command.Request.RoleId!, newUser, ct);

        if (registerResult.IsFailure)
            return registerResult.Error;

        if (command.Request.DepartmentId.HasValue)
        {
            var departmentResult = await departmentDomainService
                .AddUserAsync(
                registerResult.Value!,
                command.Request.DepartmentId.Value,
                ct
            );

            if (departmentResult.IsFailure)
                return departmentResult.Error;
        }

        var response = new CreateUserResponse(
            UserId: registerResult.Value!,
            UserName: newUser.UserName,
            Email: newUser.Email,
            Password: newUser.Password);

        return Result.Success(response);
    }
}