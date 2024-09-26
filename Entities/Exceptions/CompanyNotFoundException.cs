using Entities.Responses.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
	public sealed class CompanyNotFoundException : ApiNotFoundResponse
	{
        public CompanyNotFoundException(Guid companyId) : base($"The company with id {companyId} does not exist in database!")
        {
            
        }
    }
}
