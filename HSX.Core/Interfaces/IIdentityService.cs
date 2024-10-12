using HSX.Contract.Common;
using HSX.Contract.DTOs;

namespace HSX.Core.Interfaces;

public interface IIdentityService
{
    Task<ServiceResult<UserDTO>> LoginAsync(LoginDTO login);
    Task<ServiceResult> LogoutAsync();
    Task<ServiceResult<UserDTO>> GetUserByIdAsync(int id);
    Task<ServiceResult<UserDTO>> GetUserByEmailAsync(string email);
    Task<ServiceResult<UserDTO>> RegisterUserAsync(RegisterDTO register);
}
