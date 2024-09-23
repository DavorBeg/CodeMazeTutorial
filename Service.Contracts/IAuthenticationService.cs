using Microsoft.AspNetCore.Identity;
using Shared.DataTransferObjects.AuthenticationDtos;

namespace Service.Contracts
{
	public interface IAuthenticationService
	{
		Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
		Task<bool> ValidateUser(UserForAuthenticationDto userForAuthentication);
		Task<string> CreateToken();
	}
}
