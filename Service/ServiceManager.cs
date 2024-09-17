using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	public sealed class ServiceManager : IServiceManager
	{
		public ICompanyService CompanyService => throw new NotImplementedException();
		public IEmployeeService EmployeeService => throw new NotImplementedException();
	}
}
