using AutoMapper;
using Contracts;
using Entities;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DataTransferObjects.EmployeeDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal sealed class EmployeeService : IEmployeeService
	{
		private readonly IRepositoryManager _repository;
		private readonly ILoggerManager _logger;
		private readonly IMapper _mapper;
        public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
		{
			_repository = repository;
			_logger = logger;
			_mapper = mapper;	
		}

		public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
		{
			var company = _repository.Company.GetCompany(companyId, trackChanges);
			if (company == null)
				throw new CompanyNotFoundException(companyId);

			var employeeEntity = _mapper.Map<Employee>(employeeForCreation);
			_repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
			_repository.Save();

			var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);
			return employeeToReturn;
		}

		public EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges)
		{
			var company = _repository.Company.GetCompany(companyId, trackChanges);

			if (company == null)
				throw new CompanyNotFoundException(companyId);

			var employeeDb = _repository.Employee.GetEmployee(companyId, id, trackChanges);
			if(employeeDb == null)
				throw new EmployeeNotFoundException(id);

			var employee = _mapper.Map<EmployeeDto>(employeeDb);
			return employee;
		}

		public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
		{
			var company = _repository.Company.GetCompany(companyId, trackChanges);
			if (company == null)
				throw new CompanyNotFoundException(companyId);
			
			var employees = _repository.Employee.GetEmployees(companyId, trackChanges);
			if(employees == null || !employees.Any())
				throw new EmployeesNotFoundException(companyId);

			var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

			return employeesDto;
		}
	}
}
