using Contracts.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;
using Shared.RequestFeatures.Abstract;
using Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
	{
        public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
            
        }

		public void CreateEmployeeForCompany(Guid companyId, Employee employee)
		{
			employee.CompanyId = companyId;
			Create(employee);
		}

		public void DeleteEmployee(Employee employee) => Delete(employee);

		public async Task<Employee?> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges) =>
			await FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

		public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, bool trackChanges) 
			=> await FindByCondition(c => c.CompanyId.Equals(companyId), trackChanges)
				.OrderBy(o => o.Name)
				.ToListAsync();

		public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
		{
			var employees = await FindByCondition(e => e.CompanyId.Equals(companyId) && (e.Age >= employeeParameters.MinAge && e.Age <= employeeParameters.MaxAge), trackChanges)
				.FilterEmployees(employeeParameters.MinAge, employeeParameters.MaxAge)
				.Search(employeeParameters.SearchTerm)
				.OrderBy(e => e.Name)
				.Skip((employeeParameters.PageNumber - 1) * employeeParameters.PageSize)
				.Take(employeeParameters.PageSize)
				.ToListAsync();

			var count = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges).CountAsync();
			return new PagedList<Employee>(employees, count, employeeParameters.PageNumber, employeeParameters.PageSize);
		}
		
	}
}
