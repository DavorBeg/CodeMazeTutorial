using Asp.Versioning;
using CompanyEmployees.Presentation.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.AuthenticationDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
	[ApiVersion("1.0")]
	[Route("api/authentication")]
	public class AuthenticationController : ControllerBase
	{
		public readonly IServiceManager _service;
        public AuthenticationController(IServiceManager service) => _service = service;

		[HttpPost]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
		{
			var result = await _service.AuthenticationService.RegisterUser(userForRegistration);
			if(!result.Succeeded)
			{
				foreach(var error in result.Errors)
				{
					ModelState.TryAddModelError(error.Code, error.Description);
				}
				return BadRequest(ModelState);
			}

			return StatusCode(201);
		}

		[HttpPost("login")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
		{
			if(!await _service.AuthenticationService.ValidateUser(user))
			{
				return Unauthorized();
			}

			var tokenDto = await _service.AuthenticationService.CreateToken(populateExp: true);

			return Ok(tokenDto);
		}

		[HttpPost("refresh")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
		{
			var tokenDtoToReturn = await
			_service.AuthenticationService.RefreshToken(tokenDto);
			return Ok(tokenDtoToReturn);
		}
	}
}
