using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.EmployeeDtos
{
	public sealed record EmployeeForUpdateDto(string Name, int Age, string Position);
}
