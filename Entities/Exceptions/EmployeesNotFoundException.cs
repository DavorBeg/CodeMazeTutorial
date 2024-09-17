using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
	public sealed class EmployeesNotFoundException : NotFoundException
	{
        public EmployeesNotFoundException(Guid companyId) : base($"Employees not found inside company with id: {companyId}")
        {
            
        }
    }
}
