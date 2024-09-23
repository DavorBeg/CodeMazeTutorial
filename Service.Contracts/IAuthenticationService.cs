using Microsoft.AspNetCore.Identity;
using Shared.DataTransferObjects.AuthenticationDtos;

namespace Service.Contracts
{
	public interface IAuthenticationService
	{
		Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
	}
}
