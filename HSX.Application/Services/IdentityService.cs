using HSX.Contract.Common;
using HSX.Contract.DTOs;
using HSX.Core.Interfaces;
using HSX.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace HSX.Application.Services
{
    public class IdentityService(UserManager<AppUser> userManager, ITokenService tokenProvider) : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly ITokenService _tokenProvider = tokenProvider;

        public Task<ServiceResult<UserDTO>> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<UserDTO>> GetUserByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var dto = new UserDTO
            {
                Username = user!.UserName!,
                Email = user.Email!,
                Id = user.Id
            };
            return ServiceResult.Ok(dto);
        }

        public async Task<ServiceResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var user = _userManager.Users;

            var dto = new List<UserDTO>();
            return ServiceResult.Ok(dto.AsEnumerable());
        }

        public async Task<ServiceResult<UserDTO>> LoginAsync(LoginDTO login)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.UserName == login.Email);
            if (user == null) return ServiceResult.NotFound<UserDTO>();

            var result = await _userManager.CheckPasswordAsync(user, login.Password);
            if (!result) return ServiceResult.Fail<UserDTO>(errors: ["Invalid Password"]);
            
            var dto = new UserDTO
            {
                Username = user.UserName!,
                Email = user.Email!,
                Token = await _tokenProvider.CreateToken(user)
            };

            return ServiceResult.Ok(dto);
        }

        public Task<ServiceResult> LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<UserDTO>> RegisterUserAsync(RegisterDTO register)
        {
            //var user = _mapper.Map<AppUser>(RegisterDTO);

            //var result = await _userManager.CreateAsync(user, RegisterDTO.Password);
            //if (!result.Succeeded) return BadRequest(result.Errors);

            //var roleResult = await _userManager.AddToRoleAsync(user, Constants.Roles.User);
            //if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            //return new UserDTO
            //{
            //    Username = user.UserName,
            //    Token = await _tokenProvider.CreateToken(user),
            //    KnownAs = user.Email
            //};

            throw new NotImplementedException();
        }
    }
}
