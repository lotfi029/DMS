namespace Application.Features.Roles.Queries.GetById;

public sealed record GetRoleByIdQuery(
    string RoleId) : IQuery<RoleResponse>; 

internal sealed class GetRoleByIdQueryValidator : AbstractValidator<GetRoleByIdQuery>
{
    public GetRoleByIdQueryValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("Role ID is required.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Role ID must be a valid GUID.");
    }
}

internal sealed class GetRoleByIdQueryHandler(
    IRoleService service,
    IAuditService auditService) : IQueryHandler<GetRoleByIdQuery, RoleResponse>
{
    public async Task<Result<RoleResponse>> HandleAsync(GetRoleByIdQuery query, CancellationToken ct = default)
    {
        var result = await service.GetByIdAsync(query.RoleId, ct);

        if (result.IsFailure)
            return result.Error;

        await auditService.LogActionAsync(
            action: AuditAction.RoleViewed,
            module: AuditModules.Roles,
            entityName: AuditEntityNames.Role,
            outcome: result.IsSuccess ? AuditOutcome.Success : AuditOutcome.Failure,
            ct: ct);

        return result;
    }
}