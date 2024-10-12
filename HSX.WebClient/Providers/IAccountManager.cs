using HSX.Contract.Common;
using HSX.Contract.DTOs;

namespace HSX.WebClient.Providers;

/// <summary>
/// Account management services.
/// </summary>
public interface IAccountManager
{
    /// <summary>
    /// Login service.
    /// </summary>
    /// <param name="email">User's email.</param>
    /// <param name="password">User's password.</param>
    /// <returns>The result of the request serialized to <see cref="ServiceResult"/>.</returns>
    public Task<ServiceResult> LoginAsync(LoginDTO model);

    /// <summary>
    /// Log out the logged in user.
    /// </summary>
    /// <returns>The asynchronous task.</returns>
    public Task LogoutAsync();

    /// <summary>
    /// Registration service.
    /// </summary>
    /// <param name="email">User's email.</param>
    /// <param name="password">User's password.</param>
    /// <returns>The result of the request serialized to <see cref="ServiceResult"/>.</returns>
    public Task<ServiceResult> RegisterAsync(RegisterDTO model);

    public Task<bool> CheckAuthenticatedAsync();
}
