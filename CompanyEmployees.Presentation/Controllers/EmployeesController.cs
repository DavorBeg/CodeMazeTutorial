
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.EmployeeDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		public async Task<IActionResult> GetEmployeesForCompany(Guid companyId)
        {
            var employees = await _serviceManager.EmployeeService.GetEmployeesAsync(companyId, false);
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee) 
        {

            if (employee is null)
                return BadRequest("Employee object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var employeeToReturn = await _serviceManager.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employee, false);
            return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeToReturn.Id }, employeeToReturn);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid employeeId) 
        {

            await _serviceManager.EmployeeService.DeleteEmployeeForCompanyAsync(companyId, employeeId, false);
            return NoContent();

        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
        {
            if (employee is null)
                return BadRequest("Employee for update object is null.");
            if(!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

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
