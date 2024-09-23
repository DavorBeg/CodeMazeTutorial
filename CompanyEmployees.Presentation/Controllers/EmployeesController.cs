
using CompanyEmployees.Presentation.ActionFilters;
using Entities.LinkModels;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.CompanyDtos;
using Shared.DataTransferObjects.EmployeeDtos;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public EmployeesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


		[HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
		public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var employees = await _serviceManager.EmployeeService.GetEmployeeAsync(companyId, id, false);
            return Ok(employees);
        }


		[HttpGet(Name = "EmployeeCollection")]
		[ServiceFilter(typeof(ValidateMediaTypeAttribute))]
		public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
		{

			var linkParams = new LinkParameters(employeeParameters, HttpContext);
			var result = await _serviceManager.EmployeeService.GetEmployeesAsync(companyId, linkParams, trackChanges: false);
			Response.Headers["X-Pagination"] = JsonSerializer.Serialize(result.metaData);
            var returnResult = result.linkResponse.HasLinks ? Ok(result.linkResponse.LinkedEntities) : Ok(result.linkResponse.ShapedEntities);
            return returnResult;
		}


		[HttpPost]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee) 
        {

            var employeeToReturn = await _serviceManager.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employee, false);
	        return CreatedAtRoute(routeName: "GetEmployeeForCompany", routeValues: new { companyId, id = employeeToReturn.Id }, value: employeeToReturn);

		}


		[HttpPost("collection")]
		public async Task<IActionResult> CreateEmployeesForCompany(Guid companyId, [FromBody] IEnumerable<EmployeeForCreationDto> employees)
		{
			var result = await _serviceManager.EmployeeService.CreateEmployeeCollectionForCompanyAsync(companyId, employees);
			return CreatedAtRoute("GetEmployeeForCompany", new { result.ids }, result.employees);
		}

		[HttpDelete]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid employeeId) 
        {

            await _serviceManager.EmployeeService.DeleteEmployeeForCompanyAsync(companyId, employeeId, false);
            return NoContent();

        }

        [HttpPut("{id:guid}")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
        {
            await _serviceManager.EmployeeService.UpdateEmployeeForCompanyAsync(companyId, id, employee, false, true);
            return NoContent();
        }

        // its important to add to request header this:
		// Content-type: application/json-patch+json
		[HttpPatch("{id:guid}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest("Patching docs sent from client is null");

            var result = await _serviceManager.EmployeeService.GetEmployeeForPatchAsync(companyId, id, false, true);

            patchDoc.ApplyTo(result.employeeToPatch, ModelState);
            TryValidateModel(result.employeeToPatch);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _serviceManager.EmployeeService.SaveChangesForPatchAsync(result.employeeToPatch, result.employeeEntity);
            return NoContent();
        }

	}
}
