using Entities.Responses.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Responses.Concrete
{
	public class CompanyNotFoundResponse : ApiNotFoundResponse
	{
		public CompanyNotFoundResponse(Guid id) : base($"Company with id: {id} is not found in db.")
		{

		}
	}
}
