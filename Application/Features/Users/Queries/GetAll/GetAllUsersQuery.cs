namespace Application.Features.Users.Queries.GetAll;

public sealed record GetAllUsersQuery(string UserId) : IQuery<IEnumerable<UserListResponse>>;

public sealed class GetAllUsersQueryHandler(
    IUserDomainService userService,
    IAuditService auditService) : IQueryHandler<GetAllUsersQuery, IEnumerable<UserListResponse>>
{
    public async Task<Result<IEnumerable<UserListResponse>>> HandleAsync(GetAllUsersQuery query, CancellationToken ct = default)
    {
        var response = await userService.GetAllAsync(query.UserId, ct);
        
        if (response.IsFailure)
            return Result.Success(Enumerable.Empty<UserListResponse>());

        var result = response.Value.Adapt<IEnumerable<UserListResponse>>()!;
        
        await auditService.LogActionAsync(
            action: AuditAction.UserListed,
            module: AuditModules.Users,
            entityName: AuditEntityNames.User,
            entityId: query.UserId,
            description: $"User '{query.UserId}' retrieved all users.",
            ct: ct);

        return Result.Success(result);
    }
}
