
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
		public IActionResult GetEmployeesForCompany(Guid companyId)
        {
            var employees = _serviceManager.EmployeeService.GetEmployees(companyId, false);
            return Ok(employees);
        }

        [HttpPost]
        public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee) 
        {

            if (employee is null)
                return BadRequest("Employee object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var employeeToReturn = _serviceManager.EmployeeService.CreateEmployeeForCompany(companyId, employee, false);
            return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeToReturn.Id }, employeeToReturn);

        }

        [HttpDelete]
        public IActionResult DeleteEmployeeForCompany(Guid companyId, Guid employeeId) 
        {

            _serviceManager.EmployeeService.DeleteEmployeeForCompany(companyId, employeeId, false);
            return NoContent();

        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
        {
            if (employee is null)
                return BadRequest("Employee for update object is null.");
            if(!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            _serviceManager.EmployeeService.UpdateEmployeeForCompany(companyId, id, employee, false, true);
            return NoContent();
        }

        // its important to add to request header this:
		// Content-type: application/json-patch+json
		[HttpPatch("{id:guid}")]
        public IActionResult PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest("Patching docs sent from client is null");

            var result = _serviceManager.EmployeeService.GetEmployeeForPatch(companyId, id, false, true);

            patchDoc.ApplyTo(result.employeeToPatch, ModelState);
            TryValidateModel(result.employeeToPatch);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            _serviceManager.EmployeeService.SaveChangesForPatch(result.employeeToPatch, result.employeeEntity);
            return NoContent();
        }

	}
}
