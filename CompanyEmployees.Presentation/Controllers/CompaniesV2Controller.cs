using Asp.Versioning;
using CompanyEmployees.Presentation.Abstract;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers
{
	[ApiVersion("2.0")]
	[Route("api/v{version:apiVersion}/companies")]
	[ApiExplorerSettings(GroupName = "v2")]
	[ApiController]
	public class CompaniesV2Controller : ApiControllerBase
	{
		private readonly IServiceManager _service;

        public CompaniesV2Controller(IServiceManager service)
        {
			_service = service;
		}

		[MapToApiVersion("2.0")]
		[HttpGet]
		public async Task<IActionResult> GetCompanies()
		{
			var companies = await _service.CompanyService.GetAllCompaniesAsync(trackChanges: false);
			return Ok(companies);
		}

	}
}
