namespace Application.Mapping;

internal sealed class MappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        
        config.NewConfig<ApplicationRoleClaim, PermissionResponse>()
            .Map(dest => dest.Name, src => src.ClaimValue);
    }
}
