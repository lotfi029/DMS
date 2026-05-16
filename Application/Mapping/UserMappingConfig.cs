using Application.DTOs.Clients;
using Application.DTOs.Employees;

namespace Application.Mapping;

internal sealed class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ApplicationUser, UserListResponse>()
            .Map(dest => dest.UserType, src => src.UserType.ToString());

        config.NewConfig<ApplicationUser, DetailedUserResponse>()
            .Map(dest => dest.UserType, src => src.UserType.ToString());

        config.NewConfig<Employee, EmployeeResponse>()
            .Map(dest => dest.FirstName, src => src.AppUser.FirstName)
            .Map(dest => dest.LastName, src => src.AppUser.LastName)
            .Map(dest => dest.Email, src => src.AppUser.Email)
            .Map(dest => dest.UserName, src => src.AppUser.UserName)
            .Map(dest => dest.IsActive, src => src.AppUser.IsActive)
            .Map(dest => dest.LastLoginAt, src => src.AppUser.LastLoginAt)
            .Map(dest => dest.CreatedAt, src => src.AppUser.CreatedAt);

        config.NewConfig<Employee, EmployeeListResponse>()
            .Map(dest => dest.FirstName, src => src.AppUser.FirstName)
            .Map(dest => dest.LastName, src => src.AppUser.LastName)
            .Map(dest => dest.Email, src => src.AppUser.Email)
            .Map(dest => dest.IsActive, src => src.AppUser.IsActive);

        config.NewConfig<Client, ClientResponse>()
            .Map(dest => dest.FirstName, src => src.AppUser.FirstName)
            .Map(dest => dest.LastName, src => src.AppUser.LastName)
            .Map(dest => dest.Email, src => src.AppUser.Email)
            .Map(dest => dest.UserName, src => src.AppUser.UserName) 
            .Map(dest => dest.IsActive, src => src.AppUser.IsActive);

    }
}