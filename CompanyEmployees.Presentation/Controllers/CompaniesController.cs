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
		public IActionResult GetCompanies()
		{
			var companies = _serviceManager.CompanyService.GetAllCompanies(false);
			return Ok(companies);
		}

		[HttpGet("{id:guid}", Name = "CompanyById")]
		public IActionResult GetCompany(Guid id)
		{
			var company = _serviceManager.CompanyService.GetCompany(id, false);
			return Ok(company);
		}

		[HttpPost]
		public IActionResult CreateCompany([FromBody] CompanyForCreationDto company)
		{
			if (company is null)
				return BadRequest("Object for creating company is null");

			var createdCompany = _serviceManager.CompanyService.CreateCompany(company);

			return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
		}

		[HttpGet("collection/({ids})", Name = "CompanyCollection")]
		public IActionResult GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
		{
			var companies = _serviceManager.CompanyService.GetByIds(ids, false);
			return Ok(companies);
		}

		[HttpPost("collection")]
		public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
		{
			var result = _serviceManager.CompanyService.CreateCompanyCollection(companyCollection);

			return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
		}

		[HttpPut("{id:guid}")]
		public IActionResult UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
		{
			if (company is null)
				return BadRequest("Update object company for update is null");

			_serviceManager.CompanyService.UpdateCompany(id, company, true);
			return NoContent();
		}

		[HttpDelete]
		public IActionResult DeleteCompany(Guid id)
		{
			_serviceManager.CompanyService.DeleteCompany(id, false);
			return NoContent();
		}
	}
}
