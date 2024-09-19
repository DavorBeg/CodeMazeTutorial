using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Presentation.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.CompanyDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies")]
	[ApiController]
	public class CompaniesController : ControllerBase
	{
		private readonly IServiceManager _serviceManager;
        public CompaniesController(IServiceManager serviceManager)
        {
            this._serviceManager = serviceManager;	
        }

        [HttpGet]
		public async Task<IActionResult> GetCompanies()
		{
			var companies = await _serviceManager.CompanyService.GetAllCompaniesAsync(false);
			return Ok(companies);
		}

		[HttpGet("{id:guid}", Name = "CompanyById")]
		public async Task<IActionResult> GetCompany(Guid id)
		{
			var company = await _serviceManager.CompanyService.GetCompanyAsync(id, false);
			return Ok(company);
		}


		[HttpPost]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
		{
			var createdCompany = await _serviceManager.CompanyService.CreateCompanyAsync(company);
			return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
		}


		[HttpGet("collection/({ids})", Name = "CompanyCollection")]
		public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
		{
			var companies = await _serviceManager.CompanyService.GetByIdsAsync(ids, false);
			return Ok(companies);
		}



		[HttpPost("collection")]
		public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
		{
			var result = await _serviceManager.CompanyService.CreateCompanyCollectionAsync(companyCollection);
			return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
		}



		[HttpPut("{id:guid}")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
		{
			await _serviceManager.CompanyService.UpdateCompanyAsync(id, company, true);
			return NoContent();
		}



		[HttpDelete]
		public async Task<IActionResult> DeleteCompany(Guid id)
		{
			await _serviceManager.CompanyService.DeleteCompanyAsync(id, false);
			return NoContent();
		}
	}
}
