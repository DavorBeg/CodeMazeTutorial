using AutoMapper;
using Contracts;
using Entities;
using Entities.Exceptions;
using Entities.LinkModels;
using Service.Contracts;
using Shared.DataTransferObjects.CompanyDtos;
using Shared.DataTransferObjects.EmployeeDtos;
using Shared.RequestFeatures;
using Shared.RequestFeatures.Abstract;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
		private readonly IEmployeeLinks _employeeLinks;


		public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IEmployeeLinks employeeLinks)
		{
			_repository = repository;
			_logger = logger;
			_mapper = mapper;	
			_employeeLinks = employeeLinks;

		}

		private async Task CheckIfCompanyExist(Guid companyId, bool trackChanges)
		{
			var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
			if(company == null) throw new CompanyNotFoundException(companyId);
		}

		private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists(Guid companyId, Guid id, bool trackChanges)
		{
			var employeeDb = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);
			if (employeeDb == null) throw new EmployeeNotFoundException(id);

			return employeeDb;

		}

		public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
		{
			await CheckIfCompanyExist(companyId, trackChanges);

			var employeeEntity = _mapper.Map<Employee>(employeeForCreation);
			_repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
			await _repository.SaveAsync();

			var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);
			return employeeToReturn;
		}


		public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
		{
			await CheckIfCompanyExist(companyId, trackChanges);

			var employeeForCompany = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);

			if (employeeForCompany is null)
				throw new EmployeeNotFoundException(id);

			_repository.Employee.DeleteEmployee(employeeForCompany);
			await _repository.SaveAsync();
		}

		public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
		{
			await this.CheckIfCompanyExist(companyId, trackChanges);

			var employeeDb = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);
			if(employeeDb == null)
				throw new EmployeeNotFoundException(id);

			var employee = _mapper.Map<EmployeeDto>(employeeDb);
			return employee;
		}

		public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id, bool CompTrackChanges, bool empTrackChanges)
		{
			await CheckIfCompanyExist(companyId, CompTrackChanges);

			var employeeEntity = await _repository.Employee.GetEmployeeAsync(companyId, id, empTrackChanges);

			if (employeeEntity is null)
				throw new EmployeeNotFoundException(id);

			var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
			return (employeeToPatch, employeeEntity);

		}


		public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
		{
			_mapper.Map(employeeToPatch, employeeEntity);
			await _repository.SaveAsync();
		}

		public async Task<(LinkResponse linkResponse, MetaData metaData)> GetEmployeesAsync(Guid companyId, LinkParameters linkParameters, bool trackChanges)
		{
			if (!linkParameters.EmployeeParameters.ValidAgeRange)
				throw new MaxAgeRangeBadRequestException();

			await CheckIfCompanyExist(companyId, trackChanges);
			
			var employeesWithMetaData = await _repository.Employee.GetEmployeesAsync(companyId, linkParameters.EmployeeParameters, trackChanges);
			var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);
			var links = _employeeLinks.TryGenerateLinks(employeesDto, linkParameters.EmployeeParameters.Fields ?? throw new NullReferenceException(nameof(linkParameters.EmployeeParameters.Fields)), companyId, linkParameters.Context);


			return (linkResponse: links, metaData: employeesWithMetaData.MetaData);
		}

		public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges)
		{
			await CheckIfCompanyExist(companyId, compTrackChanges);

			var employeeEntity = await _repository.Employee.GetEmployeeAsync(companyId, id, empTrackChanges);

			if (employeeEntity is null)
				throw new EmployeeNotFoundException(id);

			_mapper.Map(employeeForUpdate, employeeEntity);
			await _repository.SaveAsync();
		}

		public async Task<(IEnumerable<EmployeeDto> employees, string ids)> CreateEmployeeCollectionForCompanyAsync(Guid companyId, IEnumerable<EmployeeForCreationDto> employeeCollection)
		{
			if (employeeCollection is null)
				// to do change
				throw new CompanyCollectionBadRequest();

			var employeeEntities = _mapper.Map<IEnumerable<Employee>>(employeeCollection);
			foreach (var employee in employeeEntities)
			{
				_repository.Employee.CreateEmployeeForCompany(companyId, employee);
			}

			await _repository.SaveAsync();

			var employeeCollectionToReturn = _mapper.Map<IEnumerable<EmployeeDto>>(employeeEntities);
			var ids = string.Join(",", employeeCollectionToReturn.Select(c => c.Id));

			return (employees: employeeCollectionToReturn, ids);
		}
	}
}
