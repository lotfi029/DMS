namespace Application.Mapping;

public sealed class MappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ApplicationRoleClaim, PermissionResponse>()
            .Map(dest => dest.Name, src => src.ClaimValue);

        config.NewConfig<ApplicationUser, UserListResponse>()
            .Map(dest => dest.UserType, src => src.UserType.ToString());

        config.NewConfig<ApplicationUser, DetailedUserResponse>()
            .Map(dest => dest.UserType, src => src.UserType.ToString());
    }
}
