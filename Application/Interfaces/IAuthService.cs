namespace Application.Interfaces;

public interface IAuthService
{
    // TODO: add register method and remove from user service
    public Task<Result<AuthResponse>> GetTokenAsync(LoginRequest request, CancellationToken ct = default);
    public Task<Result<AuthResponse>> GetRefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct = default);
    public Task<Result> RevokeRefreshToken(RefreshTokenRequest request, CancellationToken ct = default);
}

public interface IUserService 
{
    public Task<Result> CreateUserAsync(AddUserRequest request, CancellationToken ct = default);
    public Task<Result<UserResponse>> GetUserByIdAsync(string userId, CancellationToken ct = default);
    public Task<Result<IEnumerable<UserResponse>>> GetAllUsersAsync(CancellationToken ct = default);
    public Task<Result> DeactiveUserAsync(string userId, CancellationToken ct = default);
    public Task<Result> ActiveUserAsync(string userId, CancellationToken ct = default);

    //public Task<Result> UpdateUserAsync(UpdateUserRequest request, CancellationToken ct = default);
    public Task<Result> DeleteUserAsync(string userId, CancellationToken ct = default);
}
