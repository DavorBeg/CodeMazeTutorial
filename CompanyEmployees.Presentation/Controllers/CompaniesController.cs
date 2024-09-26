using Application.Companies.Queries;
using Asp.Versioning;
using CompanyEmployees.Presentation.Abstract;
using CompanyEmployees.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.CompanyDtos;

namespace CompanyEmployees.Presentation.Controllers
{
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/companies")]
	[ApiExplorerSettings(GroupName = "v1")]
	[ApiController]
	public class CompaniesController : ApiControllerBase
	{
		private readonly IServiceManager _serviceManager;
		private readonly ISender _sender;
        public CompaniesController(IServiceManager serviceManager, ISender sender)
        {
            this._serviceManager = serviceManager;	
			this._sender = sender;
        }

		[HttpOptions]
		public IActionResult GetCompaniesOptions()
		{
			Response.Headers["Alllow"] = "GET, OPTIONS, POST";
			return Ok();
		}

		/// <summary>
		/// Get the list of all companies
		/// </summary>
		/// <returns></returns>
		//[Authorize]
		[HttpGet(Name = "GetCompanies")]
		[MapToApiVersion("1.0")]
		public async Task<IActionResult> GetCompanies()
		{
			var companies = await _sender.Send(new GetCompaniesQuery(TrackChanges: false));
			return Ok(companies);
		}


		//[HttpGet("{id:guid}", Name = "CompanyById")]
		//[HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
		//[HttpCacheValidation(MustRevalidate = false)]
		//public async Task<IActionResult> GetCompany(Guid id)
		//{
		//	var baseResult = await _serviceManager.CompanyService.GetCompany(id, trackChanges: false);

		//	if (!baseResult.Success)
		//		return ProcessError(baseResult);

		//	var company = baseResult.GetResult<CompanyDto>();

		//	return Ok(company);
		//}


		//[HttpPost(Name = "CreateCompany")]
		//[ServiceFilter(typeof(ValidationFilterAttribute))]
		//public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
		//{
		//	var createdCompany = await _serviceManager.CompanyService.CreateCompanyAsync(company);
		//	return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
		//}


		//[HttpGet("collection/({ids})", Name = "CompanyCollection")]
		//public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
		//{
		//	var companies = await _serviceManager.CompanyService.GetByIdsAsync(ids, false);
		//	return Ok(companies);
		//}


		//[HttpPost("collection")]
		//public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
		//{
		//	var result = await _serviceManager.CompanyService.CreateCompanyCollectionAsync(companyCollection);
		//	return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
		//}


		//[HttpPut("{id:guid}")]
		//[ServiceFilter(typeof(ValidationFilterAttribute))]
		//public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
		//{
		//	await _serviceManager.CompanyService.UpdateCompanyAsync(id, company, true);
		//	return NoContent();
		//}


		//[HttpDelete]
		//public async Task<IActionResult> DeleteCompany(Guid id)
		//{
		//	await _serviceManager.CompanyService.DeleteCompanyAsync(id, false);
		//	return NoContent();
		//}
	}
}
