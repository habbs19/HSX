using HSX.Core.Models;

namespace HSX.Core.Interfaces;

public interface ITokenService
{
    Task<string> CreateToken(AppUser user);

}
