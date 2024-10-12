using Microsoft.AspNetCore.Identity;

namespace HSX.Core.Models;

public class AppUser : IdentityUser<int>
{

    public AppUser()
    {
        
    }

    public AppUser(string userName) : base(userName)
    {
    }
}
