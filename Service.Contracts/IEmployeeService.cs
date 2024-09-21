using Entities;
using Entities.LinkModels;
using Shared.DataTransferObjects.CompanyDtos;
using Shared.DataTransferObjects.EmployeeDtos;
using Shared.RequestFeatures;
using Shared.RequestFeatures.Abstract;
using System.Dynamic;

namespace Service.Contracts
{
    public interface IEmployeeService
	{
		Task<(LinkResponse linkResponse, MetaData metaData)> GetEmployeesAsync(Guid companyId, LinkParameters linkParameters, bool trackChanges);
		Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);
		Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges);
		Task<(IEnumerable<EmployeeDto> employees, string ids)> CreateEmployeeCollectionForCompanyAsync(Guid companyId, IEnumerable<EmployeeForCreationDto> employeeCollection);
		Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges);
		Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges);
		Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id, bool CompTrackChanges, bool empTrackChanges);
		Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity);
		
	}
}
