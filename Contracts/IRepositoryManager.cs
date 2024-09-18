using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Repositories;

namespace Contracts
{
    public interface IRepositoryManager
	{
		ICompanyRepository Company { get; }
		IEmployeeRepository Employee { get; }

		void Save();
	}

}
