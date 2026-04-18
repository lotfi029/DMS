namespace Application.Features.Users.Queries.GetById;

public sealed record GetUserByIdQuery(string UserId) : IQuery<DetailedUserResponse>;

public sealed class GetUserByIdQueryHandler(
    IUserDomainService userService,
    IAuditService auditService,
    IAuditContextAccessor auditContextAccessor) : IQueryHandler<GetUserByIdQuery, DetailedUserResponse>
{
    public async Task<Result<DetailedUserResponse>> HandleAsync(GetUserByIdQuery query, CancellationToken ct = default)
    {
        var userResult = await userService.GetByIdAsync(query.UserId, ct);
        if (userResult.IsFailure)
            return userResult.Error;

        var user = userResult.Value!.Adapt<DetailedUserResponse>();
        
        await auditService.LogActionAsync(
            action: AuditAction.UserViewed,
            module: AuditModules.Users,
            entityName: AuditEntityNames.User,
            entityId: auditContextAccessor.GetCurrent().UserId,
            ct: ct,
            outcome: AuditOutcome.Success);

        return user;
    }
}
